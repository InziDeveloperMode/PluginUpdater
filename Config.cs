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

       /* [Description("Do you also want to download pre-release plugins?")]
        public bool Download_pre_release { get; set; } = false;*/
    }
}
