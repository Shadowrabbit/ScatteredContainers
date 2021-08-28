// ******************************************************************
//       /\ /|       @file       IncidentWorkerContainerPostGenerate.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-24 07:55:17
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.ScatteredContainers
{
    [UsedImplicitly]
    public class IncidentWorkerContainerPostGenerate : IncidentWorker
    {
        private const int MinDist = 2; //生成在玩家最近几格
        private const int MaxDist = 7; //生成在玩家最远几格

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var sitePartList = new List<SitePartDef> {SitePartDefOf.SrContainerPost};
            //找世界坐标对应的id
            TileFinder.TryFindNewSiteTile(out var tileId, MinDist, MaxDist);
            //生成默认部分场地参数
            SiteMakerHelper.GenerateDefaultParams(parms.points, tileId, null, sitePartList,
                out var sitePartDefsWithParams);
            //场地
            var siteFactionWarContention = (Site) WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
            siteFactionWarContention.Tile = tileId;
            siteFactionWarContention.factionMustRemainHostile = false;
            if (sitePartDefsWithParams != null)
            {
                foreach (var sitePart in sitePartDefsWithParams)
                {
                    siteFactionWarContention.AddPart(new SitePart(siteFactionWarContention, sitePart.def,
                        sitePart.parms));
                }
            }

            siteFactionWarContention.desiredThreatPoints = siteFactionWarContention.ActualThreatPoints;
            Find.WorldObjects.Add(siteFactionWarContention);
            //中立信件通知
            var letter = LetterMaker.MakeLetter("SrLabelContainerPostGenerate".Translate(),
                "SrDescContainerPostGenerate".Translate(), parms.customLetterDef ?? LetterDefOf.NeutralEvent,
                siteFactionWarContention, parms.faction, parms.quest, parms.letterHyperlinkThingDefs);
            Find.LetterStack.ReceiveLetter(letter);
            return true;
        }
    }
}