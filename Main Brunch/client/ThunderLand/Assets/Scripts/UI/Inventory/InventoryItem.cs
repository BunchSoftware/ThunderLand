using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string NameItem = "";
    public string DescriptionItem;
    public int levelItem;
    public Sprite IconItem;
}
