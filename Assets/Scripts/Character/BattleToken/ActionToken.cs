using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SO/actionToken")]
public class ActionToken : Token
{

    // action token 独有属性
    public int acc;
    public int dmgMulti;
    public bool isWait;

    /// <summary>
    /// 攻击区域
    /// 
    /// </summary>
    public int shape;
}

