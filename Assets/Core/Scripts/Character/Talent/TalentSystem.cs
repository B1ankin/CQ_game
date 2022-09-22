using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 总共可以带8个天赋/思潮
/// </summary>
public class TalentSystem
{
    private int party;

    public List<Talent> carriedTalents; //后续分解锁和携带

    public TalentSystem(List<int> talents)
    {
        carriedTalents = new List<Talent>();
        // 读取携带天赋
        // 会分一个单独的handler来处理这个管理
        //Talent[] alltalents = Resources.FindObjectsOfTypeAll<Talent>();
        
        /*foreach (var _talent in alltalents)
        {
            if (talents.Contains(_talent.talentId))
            {
                carriedTalents.Add(_talent);
                Debug.Log("导入思潮:"+_talent.talentName );
            } 
        }*/

        foreach (int talentid in talents)
        {
            Talent a = (Talent) Resources.Load("Data/TalentSO/talent " + talentid);
            if (a != null)
            {
                carriedTalents.Add(a);
                Debug.Log("导入思潮:" + a.talentName);
            }
        }
        
        
    }


    /// <summary>
    /// 得到全部的携带天赋对应解锁内容， 具体内容参照配置表
    /// </summary>
    /// <returns></returns>
    public List<int> GetAllUnlocks()
    {
        List<int> ret = new List<int>();
        foreach(var a in carriedTalents)
        {
            foreach(var b in a.unlocks)
            {
                if (!ret.Contains(b))  ret.Add(b);
            }
        }

        return ret;
    }

    public Dictionary<int,int> GetAllEntries()
    {
        Dictionary<int,int> ret = new Dictionary<int,int>();
        foreach(var a in carriedTalents)
        {
            this.calcAllEntriesSupport(ref ret, a);
        }
        return ret;
    }


    public void calcAllEntriesSupport(ref Dictionary<int, int> retDict, Talent talent)
    {
        if (talent != null)
        {
            foreach (var ele in talent.entries)
            {
                if (retDict.ContainsKey(ele.x))
                {
                    retDict[ele.x] += ele.y;
                }
                else
                {
                    retDict[ele.x] = ele.y;
                }

            }
        }
    }





}


