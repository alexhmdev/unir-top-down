using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string hint;
    public Sprite itemSprite;
    public int itemID;
    public int itemValue;
    public bool isStackable;
    public int maxStack;
    public bool isConsumable;


}
