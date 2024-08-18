using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
   public int indexConstruction;
   public int x;
   public int y;
   public Renderer MainRenderer;
   public Vector2Int Size = Vector2Int.one;

   public void SetTransperent(bool available)
    {
        if (available)
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        MainRenderer.material.color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if((x+y)%2 == 0) Gizmos.color = Color.blue;
                else Gizmos.color = Color.red;

                Gizmos.DrawCube(transform.position + new Vector3(x,0,y), new Vector3(1,0.1f,1));
            }
        }
    }
}
