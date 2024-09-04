using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField]
    public Ressource[] harvestableItems;

    [Header("Options")]
    public ItemData requiredItem;
    public bool disableKinematicOnHarvest;
    public float destroyDelay;

}

[System.Serializable]
public class Ressource
{
    public ItemData itemData;

    [Range(0, 100)]
    public int dropChance;
}

public enum Tool
{
    Pickaxe,
    Axe,
    Wood
}
