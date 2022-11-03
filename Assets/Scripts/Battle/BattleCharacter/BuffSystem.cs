using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem 
{
    public List<Buff> buffList; // debuff & pos buff make stats down up
    private List<Buff> activeBuffs; // the buffs that has turn based effect such as bleed, fire...

    // Constructor
    public BuffSystem()
    {
        buffList = new List<Buff>();
        activeBuffs = new List<Buff>();
    }

    // buff management

    /// <summary>
    /// add new buff into, will be called by talent, events, and skills
    /// </summary>
    /// <param name="effects"> the table of buff effects and strength</param>
    /// <param name="source"> the buff source -- skill name;; used to avoid duplicate</param>
    public void addBuff(Dictionary<string, int> effects, string source)
    {
        foreach(Buff buff in buffList)
        {
            if (buff.source == source)
            {
                continue;
            }
            else
            {
                buffList.Add(new Buff(effects, source, 2));
            }
        }
    }

    public void BuffEffectCast(BattleCharacter character)
    {
        foreach(var buff in activeBuffs)
        {
            // character .. take actions
        }

        // nth happen if active buffs is empty
    }

    public void TurnEffectUpdate()
    {
        foreach( var buff in buffList)
        {
            buff.duration--;
            if (buff.duration <= 0)
            {
                buffList.Remove(buff); // clear buff from character's buff pool
                if ( activeBuffs.Contains(buff)) activeBuffs.Remove(buff); // clear buff from active pool as well
            }
        }
    }


    public string GetBuffDescription(Buff buff)
    {
        string str = "";
        foreach(var effect in buff.effects.Keys)
        {
            // add a buff and debuff check later for UI display
            str += $"{effect}:{buff.effects[effect]}";
        }
        str += $"剩余回合数：{buff.duration}";
        return str;
    }


}



/// <summary>
/// Buff instance
/// </summary>
public class Buff
{
    public Dictionary<string, int> effects;
    public string source; // can be skill or talent or event
    public int duration;

    public Buff(Dictionary<string, int> effects,  string source, int duration)
    {
        this.effects = effects;
        this.source = source;
        this.duration = duration;
    }
}