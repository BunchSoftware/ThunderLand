using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : ScriptableObject
{
    public string NameSkill = "";
    public string DescriptionSkill;
    public int levelSkill;
    public float timeRecoverySkill;
    public float timeCreationSkill;
    public Sprite IconSkill;

    protected private TypeSkill typeSkill;
    [HideInInspector] public TypeSkill TypeSkill { get => typeSkill; }

    public Action<Skill> UsedSkill;

    public void UseSkill()
    {
        UsedSkill?.Invoke(this);
    }
}

public enum TypeSkill
{
    PassiveSkill = 0,
    ActiveSkill = 1,
    ActionSkill = 2,
}
