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


    // 装备和天赋（思潮）带来的属性提升
    private EntryTable etable;
    public Dictionary<string, int> entryDict;
    public int tokenSlots;

    public int faceDirection;

    private void Awake()
    {
        entryDict = new Dictionary<string, int>();
        etable = new EntryTable();
        LoadCharacter(null); //test-only
        faceDirection = 1;
    }

    //导入人物数据
    void LoadCharacter(Character character)
    {
        this.characterData = new CharacterData(cc); //test-only

        UpdateAllAddonEntries();

        //this.characterData = new CharacterData(character);
        //动态数值更新

        maxSteps = characterData.moveRange;
        tokenSlots = characterData.tokenSlotAmount;
    }

    /// <summary>
    /// 回复全部步数
    /// </summary>
    public void restoreSteps()
    {
        steps = maxSteps;
    }

    public void restoreSlots()
    {
        tokenSlots = characterData.tokenSlotAmount;
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
    /// <summary>
    /// AddOn stat  by entries
    /// </summary>
    /// <returns></returns>
    public void UpdateAllAddonEntries()
    {
        // equips
        foreach (var key in characterData.GetEquipmentEntries().Keys)
        {
            int entry_value = characterData.GetEquipmentEntries()[key];
            string t_name = etable.GetStrByIndex(key);

            if (entryDict.ContainsKey(t_name))
            {
                entryDict[t_name] += entry_value;
            } else
            {
                entryDict[t_name] = entry_value;
            }
        }

        //talents
        foreach (var key in characterData.GetTalentEntries().Keys)
        {
            int entry_value = characterData.GetEquipmentEntries()[key];
            string t_name = etable.GetStrByIndex(key);

            if (entryDict.ContainsKey(t_name))
            {
                entryDict[t_name] += entry_value;
            }
            else
            {
                entryDict[t_name] = entry_value;
            }
        }


        // test Debug
        foreach(var key in entryDict.Keys)
        {
            Debug.Log($"testEntry: {key}++{entryDict[key]}");
        }
    }

    public float GetHealthPercent()
    {
        return characterData.healthSystem.GetHealthPercent();
    }

    public float GetSanityPercent()
    {
        return characterData.sanitySystem.GetSanityPercent();
    }

    public int GetDamage()
    {
        int dmg = 0;
        if (entryDict.ContainsKey("damage"))
        {
            dmg += entryDict["damage"];
        }

        // 增加属性补正
        dmg += (characterData.str + characterData.dex) / 2; 

        if (dmg < 0) return 0;
        return dmg;
    }

    public int GetEvade()
    {
        //TODO + buff, + talent
        int ret = 0;
        if (entryDict.ContainsKey("evadeChance"))
        {
            ret += entryDict["evadeChance"];
        }
        return ret;
    }

    public int GetTaunt()
    {
        int ret = 0;
        if (entryDict.ContainsKey("blockChance"))
        {
            ret += entryDict["blockChance"];
        }
        //TODO + buff, + talent
        return ret;
    }


    #endregion 

    public int GetDistance(BattleCharacter target)
    {
        var vec = this.activeTile.gridPos - target.activeTile.gridPos;

        return Mathf.Abs(vec.x) + Mathf.Abs(vec.y);
    }

    public bool TestDamage(BattleCharacter target)
    {
        if(target.GetEvade() > Random.Range(0, 100))
        {
            Debug.Log(target.characterData.CharacterName + "闪避了伤害");
        } else
        {
            if( target.GetTaunt() > Random.Range(0, 100))
            {
                Debug.Log(target.characterData.CharacterName + "格挡了伤害");
            }
            else
            {
                int dmg = 4;
                if (this.characterData.critChance > Random.Range(0, 100))
                {
                    dmg = 6;
                }
                bool ret = target.characterData.healthSystem.HealthUpdate(-1 * dmg);
                Debug.Log($"{target.characterData.CharacterName}受到了{dmg}点伤害，还剩下{target.characterData.healthSystem.health}点血");

                if( !ret)
                {
                    target.gameObject.transform.Rotate(new Vector3(0, 90, 0));
                }
            }

        }

        return true;
    }


    public bool IsDead()
    {
        return characterData.healthSystem.IsDead();
    }




}