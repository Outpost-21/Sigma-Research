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
    public class SigmaResearchSettings : ModSettings
    {
        public bool verboseLogging = false;

        public bool removeEmptyTabs = true;

        public bool moveUnpatchedResearches = true;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref removeEmptyTabs, "removeEmptyTabs", true);
            Scribe_Values.Look(ref moveUnpatchedResearches, "moveUnpatchedResearches", true);
        }
    }
}
