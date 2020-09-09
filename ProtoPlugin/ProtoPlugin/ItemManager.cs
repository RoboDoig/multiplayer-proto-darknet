using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using DarkRift.Server;

namespace ProtoPlugin
{
    public class ItemManager : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(0, 0, 1);

        public ItemManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += ClientConnected;

            List<NetworkItem> itemList = new List<NetworkItem>();
            itemList.Add(CreateItem("item.resource.wood", 2));
            CreateContainer(itemList, 0, 0, 0, 0);

            itemList = new List<NetworkItem>();
            itemList.Add(CreateItem("item.resource.gold", 2));
            CreateContainer(itemList, 5, 0, 5, 0);
        }

        void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += ItemMessageReceived;

            // Send all network container items to client
            foreach (NetworkItemContainer networkItemContainer in NetworkItemContainer.itemContainerDictionary.Values.Where(x => x.type == 0))
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

                        NetworkItemContainer.itemContainerDictionary[networkID].AddItem(CreateItem(itemName, amount));

                        using (Message newMessage = Message.Create(Tags.AddItemToContainerTag, NetworkItemContainer.itemContainerDictionary[networkID]))
                        {
                            foreach (IClient c in ClientManager.GetAllClients())
                                c.SendMessage(newMessage, SendMode.Reliable);
                        }
                    }
                }

                // Client wants to delete item from container
                if (message.Tag == Tags.DeleteItemFromContainerTag)
                {
                    using (DarkRiftReader reader = message.GetReader())
                    {
                        int networkID = reader.ReadInt32();
                        string itemName = reader.ReadString();
                        int amount = reader.ReadInt32();

                        NetworkItemContainer.itemContainerDictionary[networkID].DeleteItem(CreateItem(itemName, amount));

                        using (Message newMessage = Message.Create(Tags.AddItemToContainerTag, NetworkItemContainer.itemContainerDictionary[networkID]))
                        {
                            foreach (IClient c in ClientManager.GetAllClients())
                                c.SendMessage(newMessage, SendMode.Reliable);
                        }
                    }
                }

                // Client wants to do y
            }
        }

        NetworkItem CreateItem(string name, int amount)
        {
            NetworkItem newItem = new NetworkItem(name, amount);

            return newItem;
        }

        NetworkItemContainer CreateContainer(List<NetworkItem> contents, float posX, float posY, float posZ, ushort type)
        {
            NetworkItemContainer newContainer = new NetworkItemContainer(posX, posY, posZ, type);
            foreach(NetworkItem item in contents)
            {
                newContainer.AddItem(item);
            }

            return newContainer;
        }
    }
}
