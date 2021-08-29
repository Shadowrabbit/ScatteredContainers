// ******************************************************************
//       /\ /|       @file       IncidentWorkerContainerDrop.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-29 09:46:09
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace RabiSquare.ScatteredContainers
{
    [UsedImplicitly]
    public class IncidentWorkerContainerDrop : IncidentWorker
    {
        private static ThingDef RandomContainerDef => GetRandomContainerDef();

        private static readonly Pair<int, float>[] CountChance =
        {
            new Pair<int, float>(1, 1f),
            new Pair<int, float>(2, 0.95f),
            new Pair<int, float>(3, 0.7f),
            new Pair<int, float>(4, 0.4f)
        };

        private static int RandomCountToDrop
        {
            get
            {
                var timePassedFactor =
                    Mathf.Clamp(GenMath.LerpDouble(0.0f, 1.2f, 1f, 0.1f,
                        Find.TickManager.TicksGame / 3600000f), 0.1f, 1f);
                return CountChance.RandomElementByWeight(x => x.First == 1 ? x.Second : x.Second * timePassedFactor)
                    .First;
            }
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            var target = (Map) parms.target;
            return TryFindContainerDropCell(target.Center, target, 999999, out _);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var target = (Map) parms.target;
            if (!TryFindContainerDropCell(target.Center, target, 999999, out var pos))
                return false;
            DropContainers(pos, target, RandomCountToDrop);
            Messages.Message("SrMessageContainerDrop".Translate(), new TargetInfo(pos, target),
                MessageTypeDefOf.NeutralEvent);
            return true;
        }

        private static void DropContainers(IntVec3 firstChunkPos, Map map, int count)
        {
            DropContainer(firstChunkPos, map);
            for (var index = 0; index < count - 1; ++index)
            {
                if (TryFindContainerDropCell(firstChunkPos, map, 5, out var pos))
                    DropContainer(pos, map);
            }
        }

        private static void DropContainer(IntVec3 pos, Map map)
        {
            SkyfallerMaker.SpawnSkyfaller(ThingDefOf.SrContainerIncoming, RandomContainerDef, pos, map);
        }

        private static bool TryFindContainerDropCell(IntVec3 nearLoc, Map map, int maxDist, out IntVec3 pos) =>
            CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.SrContainerIncoming, map, out pos, nearLoc: nearLoc,
                nearLocMaxDist: maxDist);

        private static ThingDef GetRandomContainerDef()
        {
            var containerList =
                DefDatabase<ThingDef>.AllDefsListForReading.Where(thingDef =>
                    thingDef.building?.buildingTags != null &&
                    thingDef.building.buildingTags.Any(tag => tag.Equals("SrContainer"))).ToList();
            containerList.TryRandomElement(out var targetContainer);
            return targetContainer;
        }
    }
}