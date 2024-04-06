using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;
using RimWorld;

namespace LFSGenesExpandedSkin
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatch
    {
        private static readonly Type patchType = typeof(HarmonyPatch);

        static HarmonyPatch()
        {
            Harmony harmony = new Harmony("GeneSpawner");
            harmony.Patch(AccessTools.Method(typeof(PawnGenerator), typeof(PawnGenerator).GetMethod(
                        "GenerateGenes",
                        BindingFlags.Static | BindingFlags.NonPublic).Name,
                    null,
                    null),
                null,
                new HarmonyMethod(HarmonyPatch.patchType, "PostfixGenerator", null), null, null);
            Log.Message("[GeneExpandedSkins] harmony patch succeeded.");
        }

        public static void PostfixGenerator(Pawn pawn, XenotypeDef xenotype, PawnGenerationRequest request)
        {
            var customXeno = pawn.genes.UniqueXenotype;
            var isBaby = request.AllowedDevelopmentalStages == DevelopmentalStage.Newborn;

            if (isBaby) return;
            if (customXeno) return;

            GeneGroups possibleEndotypes =
                DefDatabase<GeneGroups>.AllDefs.FirstOrDefault((GeneGroups x) =>
                    string.Equals(x.defName, pawn.genes.Xenotype.defName,
                        StringComparison.CurrentCultureIgnoreCase));

            var doesHaveEndoType = possibleEndotypes != null;

            if (!doesHaveEndoType) return;

            var rnd = new Random();
            foreach (var geneGroup in possibleEndotypes.geneGroups)
            {
                var randomNum = rnd.Next(0, geneGroup.Endogenes.Count);
                pawn.genes.AddGene(geneGroup.Endogenes[randomNum].geneDef,
                    possibleEndotypes.isXenoGene);
            }
        }
    }
}