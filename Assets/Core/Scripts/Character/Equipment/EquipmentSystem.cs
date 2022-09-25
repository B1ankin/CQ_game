using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EquipmentSystem
{
    public Equipment Head;
    public Equipment Body;
    public Equipment MainHand;
    public Equipment OffHand;
    public Equipment Leg;
    public Equipment Hourse;


    public EquipmentSystem()
    {
        Head = null;
        Body = null;
        MainHand = null;
        OffHand = null;
        Leg = null;
        Hourse = null;
    }

    public EquipmentSystem(int head, int body, int mainHand, int offHand, int leg, int hourse)
    {
        //对应配置表生成生成装备
    }

    /// <summary>
    /// 更新装备内容
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="itemId"></param>
    public void UpdateEquipmentById(int pos, Equipment itemId)
    {
        if (pos == 0) Debug.Log("head");
        else if (pos == 1 ) Debug.Log("Body");
        else if (pos == 2 ) Debug.Log("MainHand");
        else if (pos == 3 ) Debug.Log("OffHand");
        else if (pos == 4 ) Debug.Log("Leg");
        else if (pos == 5 ) Debug.Log("Hourse");

    }

    public Dictionary<int, int> GetAllequipmentEntries()
    {
        Dictionary<int, int> ret = new Dictionary<int, int>();

        //一下子忘了这么搞个临时collection了
        calcAllEntriesSupport(ref ret, Head);
        calcAllEntriesSupport(ref ret, Body);
        calcAllEntriesSupport(ref ret, MainHand);
        calcAllEntriesSupport(ref ret, OffHand);
        calcAllEntriesSupport(ref ret, Leg);
        calcAllEntriesSupport(ref ret, Hourse);



        return ret;
    }

    public void calcAllEntriesSupport(ref Dictionary<int,int> retDict, Equipment equip)
    {
        if (equip != null)
        {
            foreach (var ele in equip.equipmentEntries)
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



