using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character SO model that used in game
/// </summary>
[CreateAssetMenu(menuName = "SO/Character")]
public class Character : ScriptableObject
{
    public int characterId;
    public string characterName;

    // portrait
    public Sprite characterSprite;

    // battle animation
    public Animator characterAnimator;


    public int hpMax;
    public int spMax;

    public int str;
    public int dex;

    public int critChance;
    public int critMulti;

    public int spd;
    public int acc;
    public int moveRange;

    // index list
    public List<int> Talents;
    
    // 人物自带token - 现在包括见闻来源
    public List<int> Tokens;

    // deserilized by get equip by index
    public int Head;
    public int Body;
    public int MainHand;
    public int OffHand;
    public int Leg;
    public int Hourse;

    #region out battle field
    public List<int> EventNodes; // record character's events relats
    public int dialogId; // index of each dialog file it matches

    #endregion 
}
