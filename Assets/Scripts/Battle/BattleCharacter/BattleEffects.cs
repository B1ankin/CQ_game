using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能发生器
/// </summary>
public class BattleEffects
{

    private EntryTable etable;

    public BattleEffects()
    {
        etable = new EntryTable();
    }


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

        if( actionToken.GetType() == typeof(SpecialToken))
        {
            var effects = new Dictionary<string, int>();
            if (actionToken.tokenId == 3)
            {
                Debug.Log("使用闪避");
                effects["evadeChance"] = caster.characterData.dex * 5;
                caster.buffSystem.addBuff(effects, "准备闪避");
            } else if(actionToken.tokenId ==4) {
                Debug.Log("准备格挡");
                effects["blockChance"] = caster.characterData.str * 5;
                caster.buffSystem.addBuff(effects, "准备格挡");
            }
        }


        // 统计效果
        // token combo check
        Dictionary<int, int> comboEffect = caster.characterData.tokenSystem.CheckCombo(supportTokens);
        if (comboEffect.Count != 0)
        {
            Debug.Log("存在combo:" + comboEffect);
        }

        // token effect sum
        Dictionary<string, int> tokenEffects = new Dictionary<string, int>();
        foreach (var i in supportTokens)
        {
            var j = (SupportToken)i;
            foreach (var k in j.tokenEffects)
            {
                var key = etable.GetStrByIndex(k.x);
                if (tokenEffects.ContainsKey(key))
                {
                    tokenEffects[key] += k.y;
                }
                else
                {
                    tokenEffects[key] = k.y;
                }

            }
        }

        // test log 

        // 执行效果 

        //对应效果配置表 -- 不应该引用battleController
        // move
        if(tokenEffects.ContainsKey("moveRight"))
        {
            BattleController.Instance.MoveToTile1(caster, BattleManager.Instance.GetTileByPos(caster.activeTile.gridPos + new Vector3Int(tokenEffects["moveRight"], 0, 0)));
        }

        if (tokenEffects.ContainsKey("moveLeft"))
        {
            BattleController.Instance.MoveToTile1(caster, BattleManager.Instance.GetTileByPos(caster.activeTile.gridPos + new Vector3Int(-1 * tokenEffects["moveLeft"], 0, 0)));
        }

        



        // 执行伤害
        DamageTarget(caster, targetTile.standon, tokenEffects);



        // 显示效果
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




    #region TokenEffects -- OnTokenOnly


    #region DamageSupport
    /// <summary>
    /// Test Damage process -- passed
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool TestDamage(BattleCharacter caster, BattleCharacter target)
    {
        bool ret = target.characterData.healthSystem.HealthUpdate(-1 * caster.GetDamage());
        Debug.Log($"{target.characterData.CharacterName}受到了{caster.characterData.CharacterName}{caster.GetDamage()}点伤害，还剩下{target.characterData.healthSystem.health}点血");


        if (target.characterData.healthSystem.IsDead())
        {
            target.gameObject.transform.Rotate(new Vector3(0, 90, 0));
            Debug.Log(target.characterData.CharacterName + "被击杀");
        }

        return ret;
    }

    private void DamageTarget(BattleCharacter caster, BattleCharacter target, Dictionary<string, int> tokenEffects)
    {
        // 
        int acc = caster.characterData.acc;
        if (tokenEffects.ContainsKey("accuracy")) acc += tokenEffects["accuracy"];
        if ( target.GetEvade() < Random.Range(0, 100 + acc) || tokenEffects.ContainsKey("ignoreEvade"))
        {
            if (target.GetTaunt() < Random.Range(1, 100) || tokenEffects.ContainsKey("ignoreTaunt"))
            {
                int dmg = caster.GetDamage();
                if (tokenEffects.ContainsKey("damagePercent"))
                {
                    dmg *= (100 + tokenEffects["damagePercent"]) / 100;
                }


                if (target.characterData.critChance >= Random.Range(0, 100))
                {
                    dmg = dmg * target.characterData.critMulti / 100;
                }

                // onHit Check
                if (tokenEffects.ContainsKey("extraSanDamage"))
                {
                    target.characterData.sanitySystem.UpdateSanity(tokenEffects["extraSanDamage"]);
                        }
                // damage result
                bool ret = target.characterData.healthSystem.HealthUpdate(-1 * caster.GetDamage());
                Debug.Log($"{target.characterData.CharacterName}受到了{caster.characterData.CharacterName}{caster.GetDamage()}点伤害，还剩下{target.characterData.healthSystem.health}点血");


                // hurt check
                

                if (target.characterData.healthSystem.IsDead())
                {
                    target.gameObject.transform.Rotate(new Vector3(0, 90, 0));
                    Debug.Log(target.characterData.CharacterName + "被击杀");
                }

            }
            else
            {
                Debug.Log($"{target.name} {target.GetTaunt()} 成功格挡");
                // check taunt (caster, target)
                
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