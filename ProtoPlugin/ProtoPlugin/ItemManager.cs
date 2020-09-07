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

        Dictionary<int, NetworkItem> itemDictionary = new Dictionary<int, NetworkItem>();

        public ItemManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += ClientConnected;
            //ClientManager.ClientDisconnected += ClientDisconnected;

            CreateItem("item.resource.wood", 1, 0, 0, 0);
            CreateItem("item.resource.gold", 2, -5, 0, 5);
        }

        void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += ItemMessageReceived;

            // Send all network items to client
            foreach (NetworkItem networkItem in itemDictionary.Values)
            {
                using (Message newMessage = Message.Create(Tags.SpawnItemTag, networkItem))
                {
                    foreach (IClient client in ClientManager.GetAllClients().Where(x => x == e.Client))
                        client.SendMessage(newMessage, SendMode.Reliable);
                }
            }
        }

        void ItemMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // Logic to deal with client spawning new object

            // Logic to deal with client modifying an object
        }

        void CreateItem(string name, int amount, float posX, float posY, float posZ)
        {
            NetworkItem newItem = new NetworkItem(name, amount, posX, posY, posZ);
            itemDictionary.Add(newItem.networkID, newItem);
        }
    }
}
