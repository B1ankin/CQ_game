using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "SO/talent")]
public class Talent : ScriptableObject
{
    public int talentId;
    public string talentName;
    public int party; // 0 = general, 1-4 对应 亢奋等4个

    public List<int> unlocks; // 对应思潮解锁效果表，功能包括解锁代币，替换代币
    public List<Vector2Int> entries; //思潮增加人物数值或效果

}