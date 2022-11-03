using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;


/// <summary>
/// Token
/// </summary>
public class Token : ScriptableObject
{
    public int tokenId;
    public string tokenName;

    public Sprite tokenSprite;

    /// <summary>
    /// 用于判断token类型和绑定类型
    /// </summary>
    public List<string> tokenTags;
    public int TokenCost;
}


