using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using UnityEngine;

public class GenerationMap : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, MeshWithTexture, MeshWithoutTexture };

    public const int mapChunkSize  = 241;

    [SerializeField] private DrawMode drawMode;
    [SerializeField] private float noiseScale;
    [Range(0,6)]
    [SerializeField] private int levelOfDetail;
    [SerializeField] private int octaves;
    [Range(0, 1)]
    [SerializeField] private float persistance;
    [SerializeField] private float lacunarity;
    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private AnimationCurve meshHeightCurve;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;
    [SerializeField] private TerrainType[] regions;

    public bool autoUpdate;

    Queue<MapThreadInfo<MapData>> mapDataThreadinfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadinfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGeneration.TextureFromHeightMap(mapData.heightMap));
        else if (drawMode == DrawMode.ColourMap)
            display.DrawTexture(TextureGeneration.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.MeshWithTexture)
            display.DrawMeshWithTexture(MeshGeneration.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGeneration.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.MeshWithoutTexture)
            display.DrawMeshWithoutTexture(MeshGeneration.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail));
    }

    public void RequestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback);
        };
        new Thread(threadStart).Start();
    }
    private void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenerateMapData();
        lock (mapDataThreadinfoQueue)
        {
            mapDataThreadinfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }
    public void RequestMeshData(MapData mapData, Action<MeshData> callback)
    {
        
    }

    private void MeshDataThread(MapData mapData, Action<MeshData> callback)
    {
        MeshData meshData = MeshGeneration.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
        lock (meshDataThreadinfoQueue)
        {
            meshDataThreadinfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if(mapDataThreadinfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadinfoQueue.Count; i++)
            {
               MapThreadInfo<MapData> threadInfo = mapDataThreadinfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if(meshDataThreadinfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadinfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadinfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData()
    {
        float[,] noisemap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noisemap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        return new MapData(noisemap, colourMap);
    }
    private void OnValidate()
    {
        if(lacunarity < 1)
            lacunarity = 1;
        if(octaves < 0)
            octaves = 0;
    }

    struct MapThreadInfo<T> 
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
[System.Serializable]
public struct MapData 
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}

