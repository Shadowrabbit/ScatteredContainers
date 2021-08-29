// ******************************************************************
//       /\ /|       @file       SymbolResolverContainerPost.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-27 02:21:43
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using JetBrains.Annotations;
using RimWorld.BaseGen;
using Verse;

namespace RabiSquare.ScatteredContainers
{
    [UsedImplicitly]
    public class SymbolResolverContainerPost : SymbolResolver
    {
        private IntRange _containerCountRange = new IntRange(1, 4);

        public override void Resolve(ResolveParams rp)
        {
            var map = BaseGen.globalSettings.map;
            //生成集装箱
            var containerList =
                DefDatabase<ThingDef>.AllDefsListForReading.Where(def =>
                    def.building?.buildingTags != null &&
                    def.building.buildingTags.Any(tag => tag.Equals("SrContainer"))).ToList();
            for (var i = 0; i < _containerCountRange.RandomInRange; i++)
            {
                var randomCell = rp.rect.RandomCell;
                //位置被占据了
                if (!randomCell.Standable(map) || randomCell.GetFirstItem(map) != null ||
                    randomCell.GetFirstPawn(map) != null || randomCell.GetFirstBuilding(map) != null)
                {
                    continue;
                }

                containerList.TryRandomElement(out var targetContainer);
                BaseGenUtility.CheckSpawnBridgeUnder(targetContainer, randomCell, Rot4.North);
                GenSpawn.Spawn(targetContainer, randomCell, map);
            }
        }
    }
}