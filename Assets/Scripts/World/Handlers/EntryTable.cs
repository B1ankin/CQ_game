using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EntryTable 
{
    Dictionary<int, string> entryDictionary;


    public EntryTable()
    {
        entryDictionary = new Dictionary<int, string>();
        entryDictionary[1] = "healthMax";
        entryDictionary[2] = "sanityMax";
        entryDictionary[3] = "str";
        entryDictionary[4] = "dex";
        entryDictionary[5] = "critChance";
        entryDictionary[6] = "critMulti";
        entryDictionary[7] = "damage";
        entryDictionary[8] = "defense";
        entryDictionary[9] = "accuracy";
        entryDictionary[10] = "speed";
        entryDictionary[11] = "moveRange";
        entryDictionary[12] = "blockChance";
        entryDictionary[13] = "evadeChance";
        entryDictionary[14] = "tokenSlot";
        entryDictionary[15] = "moveUp";
        entryDictionary[16] = "moveDown";
        entryDictionary[17] = "moveRight";
        entryDictionary[18] = "moveLeft";
        entryDictionary[19] = "moveUpRight";
        entryDictionary[20] = "moveUpLeft";
        entryDictionary[21] = "moveDownRight";
        entryDictionary[22] = "moveDownLeft";
        entryDictionary[23] = "extraSanDamage";
        entryDictionary[24] = "extraHealthDamage";
        entryDictionary[25] = "extraPieceDamage";
        entryDictionary[26] = "sanityCostReduction";
        entryDictionary[27] = "ignoreTaunt";
        entryDictionary[28] = "ignoreEvade";
        entryDictionary[29] = "ignoreArmor";
        entryDictionary[30] = "echo";
        entryDictionary[31] = "sanityHealOnHit";
        entryDictionary[32] = "sanityHealOnCrit";
        entryDictionary[33] = "counter";
        entryDictionary[34] = "ignoreCounter";
        entryDictionary[35] = "onBlockedDamage";
        entryDictionary[36] = "onHitBuff";
        entryDictionary[37] = "onCritBuff";
        entryDictionary[38] = "onEvadeBuff";
        entryDictionary[39] = "onTauntBuff";
        entryDictionary[40] = "sanityHealOnTurnStart";
        entryDictionary[41] = "healthHealOnTurnStart";
        entryDictionary[42] = "cannotMoveOnTurnStart";
        entryDictionary[43] = "cannotActionOnTurnStart";
        entryDictionary[44] = "sanityHealOnTurnEnd";
        entryDictionary[45] = "healthHealOnTurnEnd";
        entryDictionary[46] = "sanityDamageOnTurnEnd";
        entryDictionary[47] = "healthDamageOnTurnEnd";
        entryDictionary[48] = "damagePercent";
        entryDictionary[49] = "defensePercent";

    }


    public string GetStrByIndex(int id)
    {
        return entryDictionary[id];
    }

    public List<Vector2Int> GetShapeMatrix(int index)
    {
        List<Vector2Int> ret = new List<Vector2Int>();
        if( index < 20)
        {
            ret.Add(new Vector2Int(0, 0));
            if (index > 11)
            {
                ret.Add(new Vector2Int(1, 0));
                ret.Add(new Vector2Int(1, 1));
                ret.Add(new Vector2Int(1, -1));
            }
            if( index > 12)
            {
                ret.Add(new Vector2Int(2, 0));
                ret.Add(new Vector2Int(2, 1));
                ret.Add(new Vector2Int(2, -1));
                ret.Add(new Vector2Int(2, 2));
                ret.Add(new Vector2Int(2, -2));
            }


        }
        
        else if ( index < 30)
        {
            ret.Add(new Vector2Int(0, 0));
            if (index > 21)
            {
                ret.Add(new Vector2Int(1, 0));
            }
            if (index > 22)
            {
                ret.Add(new Vector2Int(2, 0));
            }
        }

        else if ( index < 40)
        {
            ret.Add(new Vector2Int(0, 0));
            ret.Add(new Vector2Int(-2, 0));
            ret.Add(new Vector2Int(0, 1));
            ret.Add(new Vector2Int(-1, 1));
            ret.Add(new Vector2Int(-2, 1));
            ret.Add(new Vector2Int(0, -1));
            ret.Add(new Vector2Int(-1, -1));
            ret.Add(new Vector2Int(-2, -1));
            if (index > 21)
            {
                ret.Add(new Vector2Int(1, 0));
                ret.Add(new Vector2Int(1, -1));
                ret.Add(new Vector2Int(1, -2));
                ret.Add(new Vector2Int(1, 1));
                ret.Add(new Vector2Int(1, 2));
                ret.Add(new Vector2Int(-3, 0));
                ret.Add(new Vector2Int(-3, -1));
                ret.Add(new Vector2Int(-3, -2));
                ret.Add(new Vector2Int(-3, 1));
                ret.Add(new Vector2Int(-3, 2));
            }

        }

        else if( index < 50)
        {
            if( Random.Range(0,100) > 80)
            {
                Debug.Log("射飘了");
                int i = Random.Range(1, 4);
                if ( i == 1)
                {
                    ret.Add(new Vector2Int(0, 1));

                } else if ( i == 2)
                {
                    ret.Add(new Vector2Int(0, -1));
                }
                else if (i == 3)
                {
                    ret.Add(new Vector2Int(1, 0));
                }
                else if (i == 4)
                {
                    ret.Add(new Vector2Int(-1, 0));
                }
            } else
            {
                ret.Add(new Vector2Int(0, 0));
            }
        }


        return ret;
    }
}

