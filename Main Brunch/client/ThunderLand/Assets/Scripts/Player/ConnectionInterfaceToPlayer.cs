using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionInterfaceToPlayer : MonoBehaviour
{
    [SerializeField] private ImageBar HealthBar;
    [SerializeField] private ImageBar ManaBar;
    [SerializeField] private ImageBar PvPPointBar;
    [SerializeField] private ImageBar ExperienceBar;
    [SerializeField] private Text MoneyCount;
    [SerializeField] private Text CountSpaceOfInventory;
    [SerializeField] private Text Level;
    [SerializeField] private Text Name;

    Player player; 

    private void Awake()
    {
        player = GetComponent<Player>();
        player.OnHealthChanged += (value) => { HealthBar.Value = value; };
        player.OnManaChanged += (value) => { ManaBar.Value = value; };
        player.OnPvPPointChanged += (value) => { PvPPointBar.Value = value; };
        player.OnExperienceChanged += (value) => { ExperienceBar.Value = value; };
        player.OnMoneyChanged += (value) => { MoneyCount.text = value.ToString(); };
        player.OnSpaceOfInventoryChanged += (value) => { CountSpaceOfInventory.text = value.ToString() + "/" + player.maxSpaceInInventory.ToString(); };
        player.OnLevelChanged += (value) => { Level.text = value.ToString(); };
        Name.text = player.Name;
    }

    //private void OnValidate()
    //{
    //    if (HealthBar != null)
    //        print(1);
    //}
}
