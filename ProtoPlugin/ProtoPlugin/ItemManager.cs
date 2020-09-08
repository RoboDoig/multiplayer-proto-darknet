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

        Dictionary<int, NetworkItemContainer> itemContainerDictionary = new Dictionary<int, NetworkItemContainer>();
        Dictionary<int, NetworkItem> itemDictionary = new Dictionary<int, NetworkItem>();

        public ItemManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += ClientConnected;

            List<NetworkItem> itemList = new List<NetworkItem>();
            itemList.Add(CreateItem("item.resource.wood", 2));
            CreateContainer(itemList, 0, 0, 0);

            itemList = new List<NetworkItem>();
            itemList.Add(CreateItem("item.resource.gold", 2));
            CreateContainer(itemList, 5, 0, 5);
        }

        void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += ItemMessageReceived;

            // Send all network items to client
            foreach (NetworkItemContainer networkItemContainer in itemContainerDictionary.Values)
            {
                using (Message newMessage = Message.Create(Tags.SpawnItemContainerTag, networkItemContainer))
                {
                    foreach (IClient client in ClientManager.GetAllClients().Where(x => x == e.Client))
                    {
                        client.SendMessage(newMessage, SendMode.Reliable);
                    }
                }
            }
        }

        void ItemMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage() as Message)
            {
                // Client wants to add item to container
                if (message.Tag == Tags.AddItemToContainerTag)
                {
                    using (DarkRiftReader reader = message.GetReader())
                    {
                        int networkID = reader.ReadInt32();
                        string itemName = reader.ReadString();
                        int amount = reader.ReadInt32();

                        itemContainerDictionary[networkID].AddItem(CreateItem(itemName, amount));

                        using (DarkRiftWriter writer = DarkRiftWriter.Create())
                        {
                            writer.Write(networkID);
                            writer.Write(itemName);
                            writer.Write(amount);

                            message.Serialize(writer);
                        }

                        foreach (IClient c in ClientManager.GetAllClients())
                            c.SendMessage(message, SendMode.Reliable);
                    }
                }

                // Client wants to do x

                // Client wants to do y
            }
        }

        NetworkItem CreateItem(string name, int amount)
        {
            NetworkItem newItem = new NetworkItem(name, amount);
            itemDictionary.Add(newItem.networkID, newItem);

            return newItem;
        }

        void CreateContainer(List<NetworkItem> contents, float posX, float posY, float posZ)
        {
            NetworkItemContainer newContainer = new NetworkItemContainer(posX, posY, posZ);
            foreach(NetworkItem item in contents)
            {
                newContainer.AddItem(item);
            }

            itemContainerDictionary.Add(newContainer.networkID, newContainer);
        }
    }
}
