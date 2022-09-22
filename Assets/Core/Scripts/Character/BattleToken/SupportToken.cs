using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SO/supportToken")]
public class SupportToken : Token
{
    public int sanCost;
    public List<Vector2Int> tokenEffects;
}
