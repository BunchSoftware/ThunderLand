using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ActiveSkill", menuName = "Skill/ActiveSkill")]
public class ActiveSkill : Skill
{
    public int ExpensesMana;
    public float Distance;

    public ActiveSkill()
    {
        typeSkill = TypeSkill.ActiveSkill;
    }
}
