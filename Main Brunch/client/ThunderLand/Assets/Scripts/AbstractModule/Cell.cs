using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Cell : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public KeyCodeCombination KeyCodeCombination;
    public Skill currentSkillInCell;
    [SerializeField] private Button buttonSkill;
    [SerializeField] private Transform nullCell;

    public Action<Skill> SkillPressed;

    private Transform draggingParent;
    private Transform originalParent;
    private Transform freeDragingParent;
    private SkillPanel skillPanel;
    private GameObject pointerWithObject;

    public void Init(Transform draggingParent, Transform freeDragingParent, SkillPanel skillPanel)
    {
        this.draggingParent = draggingParent;
        this.skillPanel = skillPanel;
        originalParent = transform.parent;
        this.freeDragingParent = freeDragingParent;
    }

    public void Render(bool isNull)
    {
        if (isNull == false && currentSkillInCell != null)
        {
            buttonSkill.gameObject.SetActive(true);
            nullCell.gameObject.SetActive(false);

            buttonSkill.GetComponent<Image>().sprite = currentSkillInCell.IconSkill;
            buttonSkill.onClick.RemoveAllListeners();
            buttonSkill.onClick.AddListener(
    () => {
        currentSkillInCell.UseSkill();
        SkillPressed?.Invoke(currentSkillInCell);
    }
    );
        }
        else if(isNull == true)
        {
            buttonSkill.gameObject.SetActive(false);
            nullCell.gameObject.SetActive(true);
        }
    }

    public void SkillPress()
    {
        if(currentSkillInCell != null)
            currentSkillInCell.UseSkill();
        SkillPressed?.Invoke(currentSkillInCell);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentSkillInCell != null)
            pointerWithObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (In((RectTransform)freeDragingParent, Input.mousePosition))
        {
            InsertPanelSkill();
        }
        else
        {
            currentSkillInCell = null;
            Render(true);
            Destroy(pointerWithObject);
        }
    }

    private bool In(RectTransform transform, Vector3 point)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(((RectTransform)transform), point);
    }

    public void InsertPanelSkill()
    {
        if (currentSkillInCell != null && pointerWithObject != null)
        {
            int closesIndex = 0;
            for (int i = 0; i < skillPanel.cellSkills.Count; i++)
            {
                if (Vector3.Distance(pointerWithObject.transform.position, skillPanel.cellSkills[i].transform.position) <=
                    Vector3.Distance(pointerWithObject.transform.position, skillPanel.cellSkills[closesIndex].transform.position)
                    )
                {
                    closesIndex = i;
                }
            }


            skillPanel.cellSkills[closesIndex].currentSkillInCell = pointerWithObject.GetComponent<Cell>().currentSkillInCell;
            skillPanel.cellSkills[closesIndex].Render(false);

            currentSkillInCell = null;
            Render(true);
            Destroy(pointerWithObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSkillInCell != null && pointerWithObject == null)
        {
            pointerWithObject = Instantiate(new GameObject(), draggingParent);
            pointerWithObject.AddComponent<Cell>().currentSkillInCell = currentSkillInCell;
            pointerWithObject.AddComponent<Image>().sprite = currentSkillInCell.IconSkill;
            pointerWithObject.name = "PointerWithObject";
            Render(true);
        }
    }
}
