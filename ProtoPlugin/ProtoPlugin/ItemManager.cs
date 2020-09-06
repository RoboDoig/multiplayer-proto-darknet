using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using DarkRift.Server;

namespace ProtoPlugin
{
    class ItemManager : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(0, 0, 1);

        Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

        public ItemManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            //ClientManager.ClientConnected += ClientConnected;
            //ClientManager.ClientDisconnected += ClientDisconnected;
        }
    }
}
