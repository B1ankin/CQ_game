using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SO/Equipment")]
public class Equipment : Item
{
    // id, itemName, sprite, ItemType, worth

    public List<string> equipmentTag;

    // 词条
    public List<Vector2Int> equipmentEntries; 


}
