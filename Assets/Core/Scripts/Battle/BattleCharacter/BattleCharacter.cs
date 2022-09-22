using UnityEngine;
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


    /// <summary>
    /// 攻击判定
    /// </summary>
    /// <param name="activeToken"></param>
    /// <param name="supportTokens"></param>
    /// <param name="direction"></param>
    /// <param name="caster"></param>
    /// <param name="targetTile"></param>
    public void tokenProcess(Token actionToken, List<Token> supportTokens, int direction,BattleCharacter caster, BattleTile targetTile)
    {

        #region debuglog
        string testout = "";
        foreach(var i in supportTokens)
        {
            testout += i.tokenName + " ";
        }
        Debug.Log($"action:[{actionToken.tokenName}] support:[{testout}] ");
        #endregion

        // check if exist combo in support Tokens -- > trogger combo effect
        Dictionary<int, int> comboEffect = characterData.tokenSystem.CheckCombo(supportTokens);
        if ( comboEffect.Count != 0 )
        {
            Debug.Log("存在combo:" + comboEffect);
        } 
        // arrange the number of real targets based on activetoken effect area and target tile
        // if has move token
        foreach ( var token in supportTokens)
        {
            if (token.tokenTags.Contains("move"))
            {

            }
        }
        // perform move action

        // check target has onclose token or talent

        // start perform attack
        // check target evade

        // check target block

        // check crit

        // check hit

    }


    /// <summary>
    /// buff系统
    /// </summary>
    /// <param name="effects"></param>
    /// <param name="skillName"></param>
    /// <param name="targetTile"></param>
    public void AddBuff(Dictionary<string, int> effects, string skillName, BattleTile targetTile)
    {
        if ( targetTile.standon != null)
        {
            targetTile.standon.buffSystem.addBuff(effects, skillName );
        }
    }



}