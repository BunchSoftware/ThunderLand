using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkill", menuName = "Skill/PassiveSkill")]
public class PassiveSkill : Skill
{
    public PassiveSkill() 
    {
        typeSkill = TypeSkill.PassiveSkill;
    }
}
