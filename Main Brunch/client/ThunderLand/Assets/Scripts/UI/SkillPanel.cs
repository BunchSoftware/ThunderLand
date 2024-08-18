using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public List<RowSkillPanel> rowSkillPanels;
    public List<Cell> cellSkills = new List<Cell>();
    [SerializeField] private Transform container;
    [SerializeField] private Transform dragingParent;
    [SerializeField] private Transform freeDragingParent;
    public Text textSkill;

    public event Action<Cell> ItemAdded;
    public event Action<Cell> ItemRemoved;

    private void OnEnable()
    {
        
        Render(cellSkills);
    }

    public void Render(List<Cell> cellSkills)
    {
        for (int i = 0; i < rowSkillPanels.Count; i++)
        {
            for (int j = 0; j < rowSkillPanels[i].transform.childCount; j++)
            {
                Cell cell = rowSkillPanels[i].transform.GetChild(j).GetComponent<Cell>();
                cell.Init(dragingParent, freeDragingParent, this);
                cell.Render(false);
                cell.SkillPressed += (skill) =>
                {
                    if(skill != null)
                        textSkill.text = cell.KeyCodeCombination.ToString() + " " + skill.NameSkill;
                };
                cellSkills.Add(cell);
            }
        }
    }
}
