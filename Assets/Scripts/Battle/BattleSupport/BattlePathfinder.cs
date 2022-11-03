using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BattlePathfinder
{
    public List<BattleTile> FindPath(BattleTile start, BattleTile end)
    {
        List<BattleTile> openList = new List<BattleTile>();
        List<BattleTile> closedList = new List<BattleTile>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            BattleTile currentTile = openList.OrderBy(x =>x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if( currentTile == end)
            {


                return GetFinishedList(start, end);
            }

            var neighbourTiles = BattleManager.Instance.GetNeightbourTiles(currentTile);

            foreach ( var neighbour in neighbourTiles)
            {
                if(neighbour.isBlocked || closedList.Contains(neighbour)) // if jump check other jump related variable or other condition
                {
                    if(neighbour == end)
                    {
                       /* neighbour.G = GetManhattenDistance(start, neighbour);
                        neighbour.H = GetManhattenDistance(end, neighbour);
                        neighbour.previous = currentTile;*/
                        return GetFinishedList(start, currentTile);
                    }
                    continue;
                    
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previous = currentTile;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }





        return new List<BattleTile>();
    }

    private List<BattleTile> GetFinishedList(BattleTile start, BattleTile end)
    {
        List<BattleTile> path = new List<BattleTile>();

        BattleTile currentTile = end;
        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.previous;
        }

        path.Reverse();
        return path;

    }

    public int GetManhattenDistance(BattleTile start, BattleTile end)
    {
        return Mathf.Abs(start.gridPos.x - end.gridPos.x) + Mathf.Abs(start.gridPos.y - end.gridPos.y);
    }

 

    public List<BattleTile> GetTilesInRange(BattleTile startingTiles, int range)
    {
        var inRangeTiles = new List<BattleTile>();
        int stepCount = 0;

        BattleManager bm = BattleManager.Instance; 
        inRangeTiles.Add(startingTiles);

        var tilForPreviousStep = new List<BattleTile>();
        tilForPreviousStep.Add(startingTiles);

        while (stepCount < range)
        {
            var surroundingTiles = new List<BattleTile>();

            foreach(var item in tilForPreviousStep)
            {
                surroundingTiles.AddRange(bm.GetNeightbourTiles(item));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tilForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }


        return inRangeTiles.Distinct().ToList();
    }


}