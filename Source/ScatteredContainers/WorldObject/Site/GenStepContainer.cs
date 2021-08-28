// ******************************************************************
//       /\ /|       @file       GenStepContainer.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-24 08:02:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace RabiSquare.ScatteredContainers
{
    [UsedImplicitly]
    public class GenStepContainer : GenStep
    {
        public override int SeedPart => Rand.Int;
        private IntRange _raidPointsRange = new IntRange(500, 1500); //袭击点数

        private static IEnumerable<Faction> CandidateFactions() =>
            Find.FactionManager.AllFactionsListForReading.Where((f) =>
                !f.IsPlayer && f.HostileTo(Faction.OfPlayer) && f.def.pawnGroupMakers != null &&
                f.def.pawnGroupMakers.Any(x => x.kindDef == PawnGroupKindDefOf.Combat));

        public override void Generate(Map map, GenStepParams parms)
        {
            //感兴趣的区域不存在
            if (!MapGenerator.TryGetVar("RectOfInterest", out CellRect cellCenter))
                //从中心取个矩形
                cellCenter = CellRect.SingleCell(map.Center);
            //使用中的区域
            if (!MapGenerator.TryGetVar("UsedRects", out List<CellRect> var2))
            {
                var2 = new List<CellRect>();
                MapGenerator.SetVar("UsedRects", var2);
            }

            //随机敌对派系
            CandidateFactions().TryRandomElement(out var faction);
            var resolveParams = new ResolveParams
            {
                rect = cellCenter,
                faction = faction,
                workSitePoints = _raidPointsRange.RandomInRange
            };
            BaseGen.globalSettings.map = map;
            //生成集装箱
            BaseGen.symbolStack.Push("SrContainerPost", resolveParams);
            //生成守卫者
            BaseGen.symbolStack.Push("SrGuard", resolveParams);
            BaseGen.Generate();
        }
    }
}