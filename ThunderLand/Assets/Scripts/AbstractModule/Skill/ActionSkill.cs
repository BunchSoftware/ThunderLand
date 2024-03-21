using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ActionSkill", menuName = "Skill/ActionSkill")]
public class ActionSkill : Skill
{
    public string textComand;

    public ActionSkill()
    {
        typeSkill = TypeSkill.ActionSkill;
    }
}
