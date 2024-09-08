using System;
using System.Collections.Generic;
using System.Text;
using GrapeNetwork.Client.Core;
using GrapeNetwork.Server.Core;
using GrapeNetwork.UnitWorld.Object;
using GrapeNetwork.UnitWorld.Type;

namespace GrapeNetwork.Server.Type.MapService
{
    public class Location
    {
        public int IDLocation;
        public Vector2 sizeLocation;
        public List<GameObject> gameObjects = new List<GameObject>();
        public Action OnLocationChanged;

        public void CreateGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }
        public GameObject FindGameObject(GameObject gameObject)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] == gameObject)
                    return gameObjects[i];
            }
            return null;
        }
        public void DestroyGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
    }
}
