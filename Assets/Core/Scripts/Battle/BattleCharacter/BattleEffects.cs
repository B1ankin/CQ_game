using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffects
{
    /// <summary>
    /// 攻击判定
    /// </summary>
    /// <param name="activeToken"></param>
    /// <param name="supportTokens"></param>
    /// <param name="direction"></param>
    /// <param name="caster"></param>
    /// <param name="targetTile"></param>
    public void tokenProcess(Token actionToken, List<Token> supportTokens, int direction, BattleCharacter caster, BattleTile targetTile)
    {

        #region debuglog
        string testout = "";
        foreach (var i in supportTokens)
        {
            testout += i.tokenName + " ";
        }
        Debug.Log($"action:[{actionToken.tokenName}] support:[{testout}] ");
        #endregion

        // 统计效果
        // token combo check
        Dictionary<int, int> comboEffect = caster.characterData.tokenSystem.CheckCombo(supportTokens);
        if (comboEffect.Count != 0)
        {
            Debug.Log("存在combo:" + comboEffect);
        }

        // token effect sum
        Dictionary<int, int> tokenEffects = new Dictionary<int, int>();
        foreach (var i in supportTokens)
        {
            var j = (SupportToken)i;
            foreach (var k in j.tokenEffects)
            {
                if (tokenEffects.ContainsKey(k.x))
                {
                    tokenEffects[k.x] += k.y;
                }
                else
                {
                    tokenEffects[k.x] = k.y;
                }

            }
        }
        foreach (var i in tokenEffects.Keys)
        {
            Debug.Log($"拥有效果{i}：{tokenEffects[i]}");
        }


        // 执行效果 & 显示效果

        //对应效果配置表
        foreach (var i in tokenEffects.Keys)
        {

        }

    }



    /// <summary>
    /// buff系统
    /// </summary>
    /// <param name="effects"></param>
    /// <param name="skillName"></param>
    /// <param name="targetTile"></param>
    public void AddBuff(Dictionary<string, int> effects, string skillName, BattleTile targetTile)
    {
        if (targetTile.standon != null)
        {
            targetTile.standon.buffSystem.addBuff(effects, skillName);
        }
    }




    #region effects


    #endregion
}