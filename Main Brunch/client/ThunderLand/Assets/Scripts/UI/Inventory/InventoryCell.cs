using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public InventoryItem currentItemInCell;
    [SerializeField] private Button buttonItem;
    [SerializeField] private Transform nullCell;

    public Action<InventoryItem> Pressed;

    private Transform draggingParent;
    private Transform originalParent;
    private Transform freeDragingParent;
    private Inventory invetory;
    private GameObject pointerWithObject;

    public void Init(Transform draggingParent, Transform freeDragingParent, Inventory invetory)
    {
        this.draggingParent = draggingParent;
        this.invetory = invetory;
        originalParent = transform.parent;
        this.freeDragingParent = freeDragingParent;
    }

    public void Render(bool isNull)
    {
        if (isNull == false && currentItemInCell != null)
        {
            buttonItem.gameObject.SetActive(true);
            nullCell.gameObject.SetActive(false);

            buttonItem.GetComponent<Image>().sprite = currentItemInCell.IconItem;
            buttonItem.onClick.RemoveAllListeners();
            buttonItem.onClick.AddListener(
    () => {
        //currentItemInCell.UseSkill();
        Pressed?.Invoke(currentItemInCell);
    }
    );
        }
        else if (isNull == true)
        {
            buttonItem.gameObject.SetActive(false);
            nullCell.gameObject.SetActive(true);
        }
    }

    public void InventoryPress()
    {
        if (currentItemInCell != null)
            //currentItemInCell.UseSkill();
        Pressed?.Invoke(currentItemInCell);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItemInCell != null)
            pointerWithObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (In((RectTransform)freeDragingParent, Input.mousePosition))
        {
            InsertInventory();
        }
        else
        {
            currentItemInCell = null;
            Render(true);
            Destroy(pointerWithObject);
        }
    }

    private bool In(RectTransform transform, Vector3 point)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(((RectTransform)transform), point);
    }

    public void InsertInventory()
    {
        if (currentItemInCell != null && pointerWithObject != null)
        {
            int closesIndex = 0;
            for (int i = 0; i < invetory.cells.Count; i++)
            {
                if (Vector3.Distance(pointerWithObject.transform.position, invetory.cells[i].transform.position) <=
                    Vector3.Distance(pointerWithObject.transform.position, invetory.cells[closesIndex].transform.position)
                    )
                {
                    closesIndex = i;
                }
            }


            invetory.cells[closesIndex].currentItemInCell = pointerWithObject.GetComponent<InventoryCell>().currentItemInCell;
            invetory.cells[closesIndex].Render(false);

            currentItemInCell = null;
            Render(true);
            Destroy(pointerWithObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItemInCell != null && pointerWithObject == null)
        {
            pointerWithObject = Instantiate(new GameObject(), draggingParent);
            pointerWithObject.AddComponent<InventoryCell>().currentItemInCell = currentItemInCell;
            pointerWithObject.AddComponent<Image>().sprite = currentItemInCell.IconItem;
            pointerWithObject.name = "PointerWithObject";
            Render(true);
        }
    }
}
