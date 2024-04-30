using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Essense : MonoBehaviour
{
    #region Event
    public Action OnDied;
    public Action OnEndMana;

    public delegate void MaxHealthRestored();
    public event MaxHealthRestored OnMaxHealthRestored;

    public delegate void ParameterWithSlider(int value);
    public event ParameterWithSlider OnHealthChanged;
    public event ParameterWithSlider OnManaChanged;

    #endregion

    #region Characteristics
    [HideInInspector] public int ID = 0;

    public string Name = "";

    [Header("Характеристики сущности")]
    public int maxHealth = 100;

    protected private int health = 0;
    [HideInInspector] public int Health { get => health; }

    public int maxMana = 100;

    protected private int mana = 0;
    [HideInInspector] public int Mana { get => mana; }

    protected private int physicalAttack = 0;
    [HideInInspector] public int PhysicalAttack { get => physicalAttack; }

    protected private int physicalDefense = 0;
    [HideInInspector] public int PhysicalDefense { get => physicalDefense; }

    protected private int magicalAttack = 0;
    [HideInInspector] public int MagicalAttack { get => magicalAttack; }

    protected private int magicalDefense = 0;
    [HideInInspector] public int MagicalDefense { get => magicalDefense; }

    protected private int accuracy = 0;
    [HideInInspector] public int Accuracy { get => accuracy; }

    protected private int dodge = 0;
    [HideInInspector] public int Dodge { get => dodge; }

    protected private int criticalAttack = 0;
    [HideInInspector] public int CriticalAttack { get => criticalAttack; }

    protected private int attackSpeed = 0;
    [HideInInspector] public int AttackSpeed { get => attackSpeed; }

    protected private int magicalSpeed = 0;
    [HideInInspector] public int MagicalSpeed { get => magicalSpeed; }

    public int maxLevel = 100;

    protected private int level = 1;
    [HideInInspector] public int Level { get => level; }

    public Agressive Agressive = Agressive.Neutral;

    public List<Skill> skills;

    #endregion

    public virtual void ActivateSkill(Skill skill)
    {

    }

    public void RecountHealth(int health)
    {
        this.health += health;
        OnHealthChanged(this.health);

        if (this.health > maxHealth)
            this.health = maxHealth;
        if (this.health <= 0)
            OnDied?.Invoke();
    }

    public void RecountMana(int mana)
    {
        this.mana += mana;
        OnManaChanged(this.mana);

        if (this.mana > maxMana)
            this.mana = maxMana;
        if (this.mana <= 0)
            OnEndMana?.Invoke();
    }
}

public enum Agressive
{
    NoAgressive = 0,
    Neutral = 1,
    Agressive = 2,
}