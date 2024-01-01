using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginUpdater
{
    public class Config : IConfig
    {
        [Description("Do you want to enable the plugin?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Do you want to enable the debug mode?")]
        public bool Debug { get; set; } = false;

        [Description("PluginUpdater folder path")]
        public string FolderPath { get; set; } = $"{Exiled.API.Features.Paths.Configs}/PluginUpdater";

        [Description("List of official plugins to be updated automatically")]
        public bool DotaHeroes { get; set; } = false;
        public bool DiscordIntegration { get; set; } = false;
        public bool CustomItems { get; set; } = false;
        public bool CustomRoles { get; set; } = false;
        public bool SCPCosmetics { get; set; } = false;
        public bool UncomplicatedCustomRoles { get; set; } = false;
        public bool FacilityManagement { get; set; } = false;
        public bool BetterSinkholes { get; set; } = false;
        public bool SerpentsHand { get; set; } = false;
        public bool BetterRestartingSystem { get; set; } = false;
        public bool AutoBroadcast { get; set; } = false;
        public bool ShootingInteractions { get; set; } = false;
        public bool UIURescueSquad { get; set; } = false;
        public bool ScriptedEvents { get; set; } = false;



    }
}
