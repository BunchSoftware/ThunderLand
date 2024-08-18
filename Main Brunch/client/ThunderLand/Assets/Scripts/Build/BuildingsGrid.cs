using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);

    private bool isEditable = false;
    private bool isCreating = false;
    private BuildingConstruction[,] grid;
    private BuildingConstruction currentBuildingConstruction;
    private Camera mainCamera;

    private void Awake()
    {
        grid = new BuildingConstruction[GridSize.x,GridSize.y];
        mainCamera = Camera.main;
    }

    public void StartPlacingBuilding(BuildingConstruction buildingPrefab)
    {
        if (buildingPrefab != null)
        {
            Destroy(currentBuildingConstruction);
        }
        currentBuildingConstruction = Instantiate(buildingPrefab, this.transform);
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Gizmos.color = Color.green;

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
            }
        }
    }

    private void Update()
    {
        if (currentBuildingConstruction != null)
        {
            isCreating = true;
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if(x < 0 || x > GridSize.x - currentBuildingConstruction.Size.x) available = false;
                if(y < 0 || y > GridSize.y - currentBuildingConstruction.Size.y) available = false;

                if(available && IsPlaceTaken(x,y)) available = false;

                currentBuildingConstruction.transform.position = new Vector3(x, 0, y);
                currentBuildingConstruction.SetTransperent(available);

                if(available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingConstruction(x, y);

                    isCreating = false;
                    isEditable = false;
                }
            }
        }
        if (isCreating == false && isEditable == false && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.gameObject.tag == "Building")
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i) == hit.transform)
                    {
                        currentBuildingConstruction = transform.GetChild(i).gameObject.GetComponent<BuildingConstruction>();
                        grid[currentBuildingConstruction.x, currentBuildingConstruction.y] = null;
                        isEditable = true;
                    }
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            RotateY();
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < currentBuildingConstruction.Size.x; x++)
        {
            for (int y = 0; y < currentBuildingConstruction.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }

    private void PlaceFlyingConstruction(int placeX, int placeY)
    {
        for (int x = 0; x < currentBuildingConstruction.Size.x; x++)
        {
            for (int y = 0; y < currentBuildingConstruction.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = currentBuildingConstruction;
            }
        }
        currentBuildingConstruction.x = placeX;
        currentBuildingConstruction.y = placeY;
        currentBuildingConstruction.SetNormal();
        currentBuildingConstruction = null;
    }
    private void RotateY()
    {
        if(currentBuildingConstruction != null)
        {
            currentBuildingConstruction.transform.Rotate(new Vector3(0, 90, 0));
        }
    }
}
