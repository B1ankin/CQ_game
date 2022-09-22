using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 内存中的人物数据
/// </summary>
public class CharacterData
{
    // Character base Stat
    int CharacterId;
    public string CharacterName;

    //animation, image, audio folder
    //string characterResource;
    //Animator charAnimator;
    Sprite characterImage;

    public HealthSystem healthSystem; // Health  
    public SanitySystem sanitySystem; // Sanity & Stamina

    public TokenSystem tokenSystem; // tokens
    public EquipmentSystem equipmentSystem; // Equipments
    public TalentSystem talentSystem;

    // base stats
    public int str;
    public int dex;

    public int critChance;
    public int critMulti;

    public int spd;
    public int acc;
    public int moveRange;



    public CharacterData(Character character)
    {
        CharacterId = character.characterId;
        CharacterName = character.characterName;

        Debug.Log("导入人物:"+character.characterName);
        // character resources
        characterImage = character.characterSprite;

        // hp and sp
        healthSystem = new HealthSystem(character.hpMax);
        sanitySystem = new SanitySystem(character.spMax);

        // base stats
        str = character.str;
        dex = character.dex;
        critChance = character.critChance;
        critMulti = character.critMulti;
        spd = character.spd;
        acc = character.acc;
        moveRange = character.moveRange;

        // equipment - TODO
        equipmentSystem = new EquipmentSystem();
        Dictionary<int,int> equipmentEntries = equipmentSystem.GetAllequipmentEntries();

        // talent - checked
        talentSystem = new TalentSystem(character.Talents);
        List<int> talentUnlocks = talentSystem.GetAllUnlocks();
        Dictionary<int, int> talentEntries = talentSystem.GetAllEntries();

        // tokens - TODO
        tokenSystem = new TokenSystem();
        tokenSystem.LoadTokens(talentUnlocks); //天赋来源 -TODO
        tokenSystem.LoadTokens(character.Tokens); //见闻来源 - checked

        // stories
        // TODO

    }



    #region support methods



    #endregion






}

