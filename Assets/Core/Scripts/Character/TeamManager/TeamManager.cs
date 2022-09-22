using UnityEngine;
using System;
using System.Collections.Generic;


public class TeamManager : MonoBehaviour
{
    private List<CharacterData> currentParty; // current up to 4 people party

    private List<CharacterData> allTeammates; // all selectable teammates via game

    private ItemSystem itemsystem; // teamownd Items


    private void Start()
    {
        DontDestroyOnLoad(this); // make current gameObject not destroy
    }

    


}
