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

        [Description("List of official plugins to be updated automatically")]
        public bool DotaHeroes { get; set; } = true;
        public bool DiscordIntegration { get; set; } = true;
        public bool CustomItems { get; set; } = true;
        public bool CustomRoles { get; set; } = true;
        public bool SCPCosmetics { get; set; } = true;
        public bool UncomplicatedCustomRoles { get; set; } = true;
        public bool FacilityManagement { get; set; } = true;
        public bool BetterSinkholes { get; set; } = true;
        public bool SerpentsHand { get; set; } = true;
        public bool BetterRestartingSystem { get; set; } = true;
        public bool AutoBroadcast { get; set; } = true;
        public bool ShootingInteractions { get; set; } = true;
        public bool UIURescueSquad { get; set; } = true;
        public bool ScriptedEvents { get; set; } = true;



    }
}
