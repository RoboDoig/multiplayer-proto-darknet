using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using DarkRift.Server;

namespace AgarPlugin
{
    public class AgarPlayerManager : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(0, 0, 1);

        public AgarPlayerManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
        }
    }
}
