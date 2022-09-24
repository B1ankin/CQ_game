﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 战斗人物
/// </summary>
public class BattleCharacter:MonoBehaviour 
{
    // data storage
    public CharacterData characterData;
    public Character cc; //test-only 

    public int Team;

    public BattleTile activeTile; // 人物站立地点

    public int maxSteps = 5; // 回合最大步数
    public int steps = 0; // 剩余步数

    public BuffSystem buffSystem;

    //private BattleActions battleActions = new BattleActions();


    private void Awake()
    {
        LoadCharacter(null); //test-only
    }

    //导入人物数据
    void LoadCharacter(Character character)
    {
        this.characterData = new CharacterData(cc); //test-only

        //this.characterData = new CharacterData(character);
        maxSteps = characterData.moveRange;


    }

    /// <summary>
    /// 回复全部步数
    /// </summary>
    public void restoreSteps()
    {
        steps = maxSteps;
    }

    /// <summary>
    /// 获得角色速度
    /// </summary>
    /// <returns>角色速度</returns>
    public int GetSpeed()
    {
        return characterData.spd;
    }

    public List<ActionToken> GetActionTokens()
    {
        return characterData.tokenSystem.actionTokens;
    }

    public List<SupportToken> GetSupportTokens()
    {
        return characterData.tokenSystem.supportTokens;
    }

    public List<SpecialToken> GetSpecialTokens()
    {
        return characterData.tokenSystem.specialTokens;
    }


    #region get base stat
    public int GetDamage()
    {
        int dmg = 0;
        if (characterData.equipmentSystem.GetAllequipmentEntries().ContainsKey(34)){ // 存在伤害值
            dmg += characterData.equipmentSystem.GetAllequipmentEntries()[34];
        }

        dmg += (characterData.str + characterData.dex) / 2; 

        if (dmg < 0) return 0;
        return dmg;
    }

    public int GetEvade()
    {
        //TODO + buff, + talent

        return characterData.dex * 5;
    }

    public int GetTaunt()
    {
        //TODO + buff, + talent
        return characterData.str * 5;
    }

    #endregion 






}