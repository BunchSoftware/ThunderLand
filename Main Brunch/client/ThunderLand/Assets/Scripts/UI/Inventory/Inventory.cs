using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject Content;
    [SerializeField] private InventoryCell prefabInventoryCell;
    [SerializeField] private Transform container;
    [SerializeField] private Transform dragingParent;
    [SerializeField] private Transform freeDragingParent;
    public int CountRow;
    public int CountColumn;
    [HideInInspector][SerializeField] public List<InventoryCell> cells;

    private void Awake()
    {
        Initilization();
    }

    public void Initilization()
    {
        if (Content != null)
        {
            cells.Clear();
            for (int i = 0; i < Content.transform.childCount; i++)
            {
                Destroy(Content.transform.GetChild(i).gameObject);
            }
            for (int x = 0; x < CountRow; x++)
            {
                for (int y = 0; y < CountColumn; y++)
                {
                    InventoryCell cell = Instantiate(prefabInventoryCell, Content.transform);
                    cell.Init(dragingParent, freeDragingParent, this);
                    cell.Render(false);
                    cells.Add(cell);
                }
            }
        }
    }
}
