using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace SigmaResearch
{
    [DefOf]
    public static class SigmaResearchDefOf
    {
        public static ResearchTabDef
            Sigma_Animal,
            Sigma_Neolithic,
            Sigma_Medieval,
            Sigma_Industrial,
            Sigma_Spacer,
            Sigma_Ultratech;

        static SigmaResearchDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SigmaResearchDefOf));
        }
    }
}
