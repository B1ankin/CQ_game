using UnityEngine;
using System.Collections.Generic;

public class BattleTile: MonoBehaviour
{
    //pathfinding support
    public int G;
    public int H;

    public int F { get { return G + H; } }
    public bool isBlocked;

    public BattleTile previous;
    public Vector3Int gridPos;

    public BattleCharacter standon; // character or obstuct

    private void Start()
    {
        isBlocked = false;
        standon = null;
    }



    public void ShowTile()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue ;
    }

    public void HideTile()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void ShowPath()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }



}