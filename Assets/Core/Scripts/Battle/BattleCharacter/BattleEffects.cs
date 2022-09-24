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
    public void tokenProcess(Token actionToken, List<Token> supportTokens, BattleCharacter caster, BattleTile targetTile)
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


        // 执行效果 

        //对应效果配置表
        foreach (var i in tokenEffects.Keys)
        {
            // 调取下方函数
        }



        // 执行伤害



        // 显示效果
        DamageTarget(caster, targetTile.standon, tokenEffects);
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


    #region DamageSupport

    private void DamageTarget(BattleCharacter caster, BattleCharacter target, Dictionary<int, int> tokenEffects)
    {
        // 
        if (caster.characterData.acc - target.GetEvade() >= Random.Range(0, 100) || tokenEffects.ContainsKey(1))
        {
            if (target.GetTaunt() >= Random.Range(0, 100))
            {
                int dmg = caster.GetDamage();
                if(target.characterData.critChance >= Random.Range(0, 100))
                {
                    dmg = dmg * target.characterData.critMulti / 100;
                }
                // onHit Check
                if (tokenEffects.ContainsKey(4))
                {
                    // 击中回蓝回血。。
                }
                // hurt check
                if (tokenEffects.ContainsKey(5))
                {
                    // caster effects
                }

            }
            else
            {
                Debug.Log($"{target.name} 成功格挡");
                // check taunt (caster, target)
                if (tokenEffects.ContainsKey(2))
                {

                }
            }

        }
        else
        {
            Debug.Log($"{target.name} 成功闪避");
            // check evade
            /*if (tokenEffects.ContainsKey(1))
            {
                ResultDamageSupport(tokenEffects);
            }*/
        }

    }




    #endregion

    #endregion
}