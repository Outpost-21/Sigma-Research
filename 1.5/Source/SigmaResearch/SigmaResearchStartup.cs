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
    [StaticConstructorOnStartup]
    public static class SigmaResearchStartup
    {
        public static SigmaResearchSettings GetSettings => SigmaResearchMod.settings;

        public static Dictionary<Vector2, string> animalProjectsMapping = new Dictionary<Vector2, string>();
        public static Dictionary<Vector2, string> neolithicProjectsMapping = new Dictionary<Vector2, string>();
        public static Dictionary<Vector2, string> medievalProjectsMapping = new Dictionary<Vector2, string>();
        public static Dictionary<Vector2, string> industrialProjectsMapping = new Dictionary<Vector2, string>();
        public static Dictionary<Vector2, string> spacerProjectsMapping = new Dictionary<Vector2, string>();
        public static Dictionary<Vector2, string> ultraProjectsMapping = new Dictionary<Vector2, string>();

        static SigmaResearchStartup()
        {
            LogUtil.LogMessage("Mapping Pre-patched Research Projects...");
            MapExistingResearchProjects();
            LogUtil.LogMessage("Mapping Complete.");
            if (GetSettings.moveUnpatchedResearches)
            {
                LogUtil.LogMessage("Remapping Unpatched Reseearch Projects...");
                RepositionToTabAndGrid();
                LogUtil.LogMessage("Remapping Complete.");
            }
            if (GetSettings.removeEmptyTabs)
            {
                LogUtil.LogMessage("Removing Empty Tabs...");
                RemoveEmptyTabs();
                LogUtil.LogMessage("Removals Complete.");
            }
            if (ModLister.AnomalyInstalled)
            {
                // Remove then add Anomaly to shove it to the end of the listing.
                DefDatabase<ResearchTabDef>.Remove(ResearchTabDefOf.Anomaly);
                DefDatabase<ResearchTabDef>.Add(ResearchTabDefOf.Anomaly);
            }
        }

        public static void MapExistingResearchProjects()
        {
            foreach(ResearchProjectDef project in DefDatabase<ResearchProjectDef>.AllDefs)
            {
                if (project.tab == ResearchTabDefOf.Anomaly) { continue; }
                Vector2 coords = new Vector2(project.ResearchViewX, project.ResearchViewY);
                if (project.tab == SigmaResearchDefOf.Sigma_Animal)
                {
                    if (animalProjectsMapping.NullOrEmpty()) { animalProjectsMapping = new Dictionary<Vector2, string>(); }
                    if (animalProjectsMapping.ContainsKey(coords)) 
                    { 
                        LogUtil.LogError($"Attempted to map coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); 
                        return; 
                    }
                    animalProjectsMapping.Add(coords, project.defName);
                }
                else if (project.tab == SigmaResearchDefOf.Sigma_Neolithic)
                {
                    if (neolithicProjectsMapping.NullOrEmpty()) { neolithicProjectsMapping = new Dictionary<Vector2, string>(); }
                    if (neolithicProjectsMapping.ContainsKey(coords))
                    {
                        LogUtil.LogError($"Attempted to map coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {neolithicProjectsMapping[coords]}.");
                        return;
                    }
                    neolithicProjectsMapping.Add(coords, project.defName);
                }
                else if (project.tab == SigmaResearchDefOf.Sigma_Medieval)
                {
                    if (medievalProjectsMapping.NullOrEmpty()) { medievalProjectsMapping = new Dictionary<Vector2, string>(); }
                    if (medievalProjectsMapping.ContainsKey(coords))
                    {
                        LogUtil.LogError($"Attempted to map coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {medievalProjectsMapping[coords]}.");
                        return;
                    }
                    medievalProjectsMapping.Add(coords, project.defName);
                }
                else if (project.tab == SigmaResearchDefOf.Sigma_Industrial)
                {
                    if (industrialProjectsMapping.NullOrEmpty()) { industrialProjectsMapping = new Dictionary<Vector2, string>(); }
                    if (industrialProjectsMapping.ContainsKey(coords))
                    {
                        LogUtil.LogError($"Attempted to map coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {industrialProjectsMapping[coords]}.");
                        return;
                    }
                    industrialProjectsMapping.Add(coords, project.defName);
                }
                else if (project.tab == SigmaResearchDefOf.Sigma_Spacer)
                {
                    if (spacerProjectsMapping.NullOrEmpty()) { spacerProjectsMapping = new Dictionary<Vector2, string>(); }
                    if (spacerProjectsMapping.ContainsKey(coords))
                    {
                        LogUtil.LogError($"Attempted to map coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {spacerProjectsMapping[coords]}.");
                        return;
                    }
                    spacerProjectsMapping.Add(coords, project.defName);
                }
                else if (project.tab == SigmaResearchDefOf.Sigma_Ultratech)
                {
                    if (ultraProjectsMapping.NullOrEmpty()) { ultraProjectsMapping = new Dictionary<Vector2, string>(); }
                    if (ultraProjectsMapping.ContainsKey(coords))
                    {
                        LogUtil.LogError($"Attempted to map coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {ultraProjectsMapping[coords]}.");
                        return;
                    }
                    ultraProjectsMapping.Add(coords, project.defName);
                }
            }
        }

        public static void RepositionToTabAndGrid()
        {
            List<ResearchProjectDef> allUnpatchedResearch = DefDatabase<ResearchProjectDef>.AllDefs.Where(rpd => rpd.tab.modContentPack != SigmaResearchMod.mod.Content).ToList();
            if (allUnpatchedResearch.NullOrEmpty()) { return; }
            foreach (ResearchProjectDef project in allUnpatchedResearch)
            {
                if (project.tab == ResearchTabDefOf.Anomaly) { continue; }
                MoveResearchTab(project);
                Vector2 newCoords = GetNextFreeSlot(project);
                MoveGridCoords(project, newCoords);
            }
        }

        public static void MoveGridCoords(ResearchProjectDef project, Vector2 coords)
        {
            switch (project.techLevel)
            {
                case TechLevel.Animal:
                    if (animalProjectsMapping.ContainsKey(coords)) { LogUtil.LogError($"Attempted to assign coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); return; }
                    animalProjectsMapping.Add(coords, project.defName);
                    break;
                case TechLevel.Neolithic:
                    if (neolithicProjectsMapping.ContainsKey(coords)) { LogUtil.LogError($"Attempted to assign coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); return; }
                    neolithicProjectsMapping.Add(coords, project.defName);
                    break;
                case TechLevel.Medieval:
                    if (medievalProjectsMapping.ContainsKey(coords)) { LogUtil.LogError($"Attempted to assign coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); return; }
                    medievalProjectsMapping.Add(coords, project.defName);
                    break;
                case TechLevel.Industrial:
                    if (industrialProjectsMapping.ContainsKey(coords)) { LogUtil.LogError($"Attempted to assign coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); return; }
                    industrialProjectsMapping.Add(coords, project.defName);
                    break;
                case TechLevel.Spacer:
                    if (spacerProjectsMapping.ContainsKey(coords)) { LogUtil.LogError($"Attempted to assign coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); return; }
                    spacerProjectsMapping.Add(coords, project.defName);
                    break;
                case TechLevel.Ultra:
                    if (ultraProjectsMapping.ContainsKey(coords)) { LogUtil.LogError($"Attempted to assign coords {coords} to {project.defName} ({project.LabelCap}) but coords already mapped to {animalProjectsMapping[coords]}."); return; }
                    ultraProjectsMapping.Add(coords, project.defName);
                    break;
                default:
                    break;
            }
            project.researchViewX = coords.x;
            project.researchViewY = coords.y;
            project.x = project.researchViewX;
            project.y = project.researchViewY;
        }

        public static Vector2 GetNextFreeSlot(ResearchProjectDef project)
        {
            for (int x = 0; x < 80; x++)
            {
                for (float y = 0f; y <= 6.5f; y += 0.5f)
                {
                    Vector2 tempVec2 = new Vector2(x, y);
                    switch (project.techLevel)
                    {
                        case TechLevel.Animal:
                            if(!animalProjectsMapping.ContainsKey(tempVec2))
                            {
                                return tempVec2;
                            }
                            break;
                        case TechLevel.Neolithic:
                            if (!neolithicProjectsMapping.ContainsKey(tempVec2))
                            {
                                return tempVec2;
                            }
                            break;
                        case TechLevel.Medieval:
                            if (!medievalProjectsMapping.ContainsKey(tempVec2))
                            {
                                return tempVec2;
                            }
                            break;
                        case TechLevel.Industrial:
                            if (!industrialProjectsMapping.ContainsKey(tempVec2))
                            {
                                return tempVec2;
                            }
                            break;
                        case TechLevel.Spacer:
                            if (!spacerProjectsMapping.ContainsKey(tempVec2))
                            {
                                return tempVec2;
                            }
                            break;
                        case TechLevel.Ultra:
                            if (!ultraProjectsMapping.ContainsKey(tempVec2))
                            {
                                return tempVec2;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            LogUtil.LogError($"Could not find empty grid coords for tech level {project.techLevel.ToStringHuman()} for project {project.defName} ({project.LabelCap})");
            return new Vector2(0, 0);
        }

        public static void MoveResearchTab(ResearchProjectDef project)
        {
            switch (project.techLevel)
            {
                case TechLevel.Animal:
                    project.tab = SigmaResearchDefOf.Sigma_Animal;
                    break;
                case TechLevel.Neolithic:
                    project.tab = SigmaResearchDefOf.Sigma_Neolithic;
                    break;
                case TechLevel.Medieval:
                    project.tab = SigmaResearchDefOf.Sigma_Medieval;
                    break;
                case TechLevel.Industrial:
                    project.tab = SigmaResearchDefOf.Sigma_Industrial;
                    break;
                case TechLevel.Spacer:
                    project.tab = SigmaResearchDefOf.Sigma_Spacer;
                    break;
                case TechLevel.Ultra:
                    project.tab = SigmaResearchDefOf.Sigma_Ultratech;
                    break;
                default:
                    if(project.tab != ResearchTabDefOf.Anomaly) 
                    { 
                        LogUtil.LogError($"Research has no assigned tech level and cannot be properly organised into tabs: {project.defName} ({project.LabelCap})"); 
                    }
                    break;
            }
        }

        public static void RemoveEmptyTabs()
        {
            List<ResearchTabDef> tabsForRemoval = DefDatabase<ResearchTabDef>.AllDefs.Where(def => !DefDatabase<ResearchProjectDef>.AllDefs.Any(rpd => rpd.tab == def)).ToList();
            if (tabsForRemoval.NullOrEmpty()) { return; }
            for (int i = 0; i < tabsForRemoval.Count; i++)
            {
                DefDatabase<ResearchTabDef>.Remove(tabsForRemoval[i]);
            }
        }
    }
}
