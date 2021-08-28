// ******************************************************************
//       /\ /|       @file       SymbolResolverGuard.cs
//       \ V/        @brief      守卫生成
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-27 02:42:00
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using RimWorld.BaseGen;
using Verse.AI.Group;

namespace RabiSquare.ScatteredContainers
{
    [UsedImplicitly]
    public class SymbolResolverGuard : SymbolResolver
    {
        private const int Radius = 10;

        public override void Resolve(ResolveParams rp)
        {
            //随机敌对派系守卫
            var map = BaseGen.globalSettings.map;
            var incidentParms = new IncidentParms
                {points = rp.workSitePoints, faction = rp.faction, target = map, spawnCenter = rp.rect.CenterCell};
            var pawnGroupMakerParms =
                IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms);
            var pawnList = PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms).ToList();
            PawnSpawnUtil.SpawnPawns(pawnList, map, Radius, map.Center);
            var lordJob = new LordJob_DefendPoint(map.Center);
            LordMaker.MakeNewLord(rp.faction, lordJob, map, pawnList);
        }
    }
}