using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manage token use in battle
/// </summary>
public class TokenSystem
{
    private int tokenSlots;

    public List<ActionToken> actionTokens;
    public  List<SupportToken> supportTokens;
    public List<SpecialToken> specialTokens;

    public TokenSystem()
    {
        actionTokens = new List<ActionToken>();
        supportTokens = new List<SupportToken>();
        specialTokens = new List<SpecialToken>();
    }


    public Dictionary<int,int> CheckCombo(List<Token> supportTokens)
    {
        var ret = new Dictionary<int, int>();
        // TODO add a combo handler to manage and check the combos


        return ret;
    }

    /// <summary>
    /// 通过人物数据更新人物代币池
    /// </summary>
    /// <param name="tokenIds"></param>
    public void LoadTokens(List<int> tokenIds)
    {
        // 根据Id添加token到token池子
        foreach (int tokenId in tokenIds)
        {
            var a = Resources.Load("Data/TokenSO/token " + tokenId);
            if (a != null)
            {
                if ( a.GetType() == typeof(ActionToken))
                {
                    var b = (ActionToken) a;
                    actionTokens.Add(b);
                    Debug.Log(b.tokenName);
                }
                else if ( a.GetType() == typeof(SupportToken))
                {
                    var c = (SupportToken) a;
                    supportTokens.Add(c);
                    Debug.Log(c.tokenName);
                }
                else if ( a.GetType() == typeof(SpecialToken))
                {
                    var d = (SpecialToken)a;
                    specialTokens.Add(d);
                    Debug.Log(d.tokenName);
                }
                //carriedTalents.Add(a);
                //Debug.Log("导入代币:" + a.talentName);
            }
        }
    }
}




