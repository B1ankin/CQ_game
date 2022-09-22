using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "SO/tokenCombo")]

public class TokenCombo : ScriptableObject
{
    public int comboId;
    public string comboName;
    public List<int> comboMem;

    public List<Vector2Int> comboEntries;
}