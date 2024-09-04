using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]
    public int itemID;
    public string name;
    public string description;
    public Sprite visual;
    public GameObject prefab;
    public bool stackable;
    public int maxStack;

    [Header("Effects")]
    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;



    [Header("armor stats")]
    public float armorPoints;
    [Header("attack stats")]
    public float attackPoints;
    [Header("Item Type")]
    public ItemType itemType;
    public EquipmentType equipmentType;
    public Tool tool;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ItemData other = (ItemData)obj;
        return itemID == other.itemID;
    }

    public override int GetHashCode()
    {
        return itemID;
    }

}

public enum ItemType
{
    Ressource,
    Equipment,
    Consumable,
    Tool
}

public enum EquipmentType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feet,
    Weapon
}

