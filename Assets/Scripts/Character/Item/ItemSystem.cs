using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ItemSystem
{
    private List<Item> ownedItems;

    //Constructor and loader



/*    public ItemSystem(CharacterStat character)
    {
        if (character == null)
        {

        }
        else
        {
            ownedItems = new List<Item>();
        }
    }*/


    public bool addItem(int itemId)
    {
        // find item from allItemHandler



        return true;
    }

    public bool rmItem(Item item)
    {
        ownedItems.Remove(item);
        return true;
    }


    //Later trade methods
    #region trade relats


    #endregion



}

public enum ItemQuality
{
    broken,
    common,
    rare,
    legendary
}


public class Item:ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite sprite;
    public int ItemType; // weapon, equipment, consumable, event

    public ItemQuality quality = ItemQuality.broken;
    public int worth;
}
