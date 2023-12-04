using System;
using Exiled.API.Features;
using System.IO;
using System.Net;
using System.IO.Compression;
using Config = PluginUpdater.Config;
using EventsHandler = PluginUpdater.EventsHandler;
using System.Runtime.CompilerServices;
using Exiled.API.Enums;
using static System.Net.WebRequestMethods;

namespace AutoUpdatePlugin
{
    public class Main : Plugin<Config>
    {
        public static Main Instance;
        public override string Name => "PluginUpdater";
        public override string Author => "semplicementeinzi";
        public override Version RequiredExiledVersion => new(8, 4, 3);
        public override Version Version => new Version(1, 0, 1);

        public override PluginPriority Priority => PluginPriority.Lowest;

        public class PluginInfo
        {
            public string Name { get; set; }
            public string GitHubRepoUrl { get; set; }
        }

        public readonly PluginInfo[] pluginsToUpdate =
        {
            new PluginInfo { Name = "PluginUpdater", GitHubRepoUrl = "https://github.com/InziDeveloperMode/PluginUpdater" },
            new PluginInfo { Name = "DotaHeroes", GitHubRepoUrl = "https://github.com/SpGerg/DotaHeroes" },
            new PluginInfo { Name = "DiscordIntegration", GitHubRepoUrl = "https://github.com/Exiled-Team/DiscordIntegration" },
            new PluginInfo { Name = "CustomItems", GitHubRepoUrl = "https://github.com/Exiled-Team/CustomItems" },
            new PluginInfo { Name = "CustomRoles", GitHubRepoUrl = "https://github.com/joker-119/CustomRoles" },
            new PluginInfo { Name = "SCPCosmetics", GitHubRepoUrl = "https://github.com/creepycats/SCPCosmetics" },
            new PluginInfo { Name = "FacilityManagement", GitHubRepoUrl = "https://github.com/louis1706/FacilityManagement" },
            new PluginInfo { Name = "UncomplicatedCustomRoles", GitHubRepoUrl = "https://github.com/FoxWorn3365/UncomplicatedCustomRoles" },
            new PluginInfo { Name = "BetterSinkholes", GitHubRepoUrl = "https://github.com/Gamers-Workshop/BetterSinkholes" },
            new PluginInfo { Name = "SerpentsHand", GitHubRepoUrl = "https://github.com/NikkiGardiner1/SerpentsHand" },
            new PluginInfo { Name = "BetterRestartingSystem", GitHubRepoUrl = "https://github.com/An4r3w/BetterRestartingSystem" },
            new PluginInfo { Name = "AutoBroadcast", GitHubRepoUrl = "https://github.com/Misfiy/AutoBroadcast" },
            new PluginInfo { Name = "ShootingInteractions", GitHubRepoUrl = "https://github.com/SiphoxR/ShootingInteractions" },
            new PluginInfo { Name = "UIURescueSquad", GitHubRepoUrl = "https://github.com/NikkiGardiner1/UIURescueSquad"},
        };

        public override void OnEnabled()
        {
            Instance = this;
            EventsHandler.UpdatePlugins();
            base.OnEnabled();

            
        }
        public override void OnDisabled()
        {
            Instance = null;
            base.OnEnabled();

            
        }


    }
}

