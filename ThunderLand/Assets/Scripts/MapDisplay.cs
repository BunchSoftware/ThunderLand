using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture2D)
    {
        renderer.sharedMaterial.mainTexture = texture2D;
        renderer.transform.localScale = new Vector3(texture2D.width, 1, texture2D.height);
    }

    public void DrawMeshWithTexture(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
    public void DrawMeshWithoutTexture(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = null;
    }
}
