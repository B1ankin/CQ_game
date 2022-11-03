using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SO/Equipment")]
public class Equipment : Item
{
    // id, itemName, sprite, ItemType, worth


    public EquipMaterial material = EquipMaterial.wood;

    public List<string> equipmentTag; //indicate type

    // 词条
    public List<Vector2Int> equipmentEntries; 


}

public enum EquipMaterial
{
    wood,
    steel,
    test
}


