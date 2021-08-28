// ******************************************************************
//       /\ /|       @file       PawnSpawnUtil.cs
//       \ V/        @brief      角色生成工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-27 01:28:50
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RabiSquare.ScatteredContainers
{
    public static class PawnSpawnUtil
    {
        public static void SpawnPawns(IEnumerable<Pawn> pawns, Map map, int radius, IntVec3 spawnPos)
        {
            var spawnRotation = Rot4.FromAngleFlat(spawnPos.AngleFlat);
            foreach (var pawn in pawns)
            {
                var loc = CellFinder.RandomClosewalkCellNear(spawnPos, map, radius);
                GenSpawn.Spawn(pawn, loc, map, spawnRotation);
            }
        }
    }
}