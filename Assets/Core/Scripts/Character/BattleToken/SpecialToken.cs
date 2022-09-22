using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Speical Token
/// 代表特殊代币， 更多拓展后续增加
/// 1. 特殊动作，带来一个一回合的buff eg. 闪避和格挡
/// 2. 道具，无消耗的一次性技能
/// </summary>
[CreateAssetMenu(menuName = "SO/specialToken")]
public class SpecialToken : Token
{
    public int itemType; //turn-flip, battle-flip, exhausted
    public int effectShape;
    public int targetingType; // direction or specific target for range attack
    public int acc;
    public List<Vector2Int> tokenEffects;


}