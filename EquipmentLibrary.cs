using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLibrary : MonoBehaviour
{
    public List<EquipmentLibraryItem> content = new List<EquipmentLibraryItem>();
}


[System.Serializable]
public class EquipmentLibraryItem
{
    public ItemData itemData;
    public GameObject itemprefab;
    public GameObject[] elementsToDisable;
}
