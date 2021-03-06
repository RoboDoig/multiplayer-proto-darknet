﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client.Unity;
using DarkRift.Client;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField]
    UnityClient client;

    [SerializeField]
    GameObject defaultContainer;

    public Dictionary<int, ItemContainer> itemContainerDict = new Dictionary<int, ItemContainer>();

    void Awake() {
        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            // Server wants to spawn new container
            if (message.Tag == Tags.SpawnItemContainerTag) {
                ItemContainerMessage itemContainerMessage = message.Deserialize<ItemContainerMessage>();

                GameObject obj = Instantiate(defaultContainer, itemContainerMessage.position, Quaternion.identity) as GameObject;
                ItemContainer itemContainer = obj.GetComponent<ItemContainer>();
                itemContainer.networkID = itemContainerMessage.networkID;
                itemContainer.itemSpawnManager = this;
                itemContainerDict.Add(itemContainer.networkID, itemContainer);

                foreach (ItemMessage item in itemContainerMessage.itemList) {
                    itemContainer.contents.Add(new Item(ItemManager.allItems[item.name], item.amount));
                }
            }

            // Server wants to add item to container
            if (message.Tag == Tags.AddItemToContainerTag) {
                ItemContainerMessage itemContainerMessage = message.Deserialize<ItemContainerMessage>();

                ItemContainer itemContainer = itemContainerDict[itemContainerMessage.networkID];
                itemContainer.contents.Clear();

                foreach (ItemMessage item in itemContainerMessage.itemList) {
                    itemContainer.contents.Add(new Item(ItemManager.allItems[item.name], item.amount));
                }
            }

            // Server wants to delete item from container
            if (message.Tag == Tags.DeleteItemFromContainerTag) {
                ItemContainerMessage itemContainerMessage = message.Deserialize<ItemContainerMessage>();

                ItemContainer itemContainer = itemContainerDict[itemContainerMessage.networkID];
                itemContainer.contents.Clear();

                foreach (ItemMessage item in itemContainerMessage.itemList) {
                    itemContainer.contents.Add(new Item(ItemManager.allItems[item.name], item.amount));
                }
            }

            // Server wants to transfer items between containers
            if (message.Tag == Tags.TransferItemBetweenContainersTag) {
                ItemContainerMessage itemContainerMessage = message.Deserialize<ItemContainerMessage>();

                ItemContainer itemContainer = itemContainerDict[itemContainerMessage.networkID];
                itemContainer.contents.Clear();

                foreach (ItemMessage item in itemContainerMessage.itemList) {
                    itemContainer.contents.Add(new Item(ItemManager.allItems[item.name], item.amount));
                }

                // TODO - this is horrible, should be dealt with by an event?
                itemContainer.GetComponentInChildren<InventoryPanel>().UpdateItems();
            }
        }
    }

    class ItemMessage {
        public int networkID {get; private set;}
        public string name {get; private set;}
        public int amount {get; private set;}

        public ItemMessage(string _name, int _amount) {
            name = _name;
            amount = _amount;
        }
    }

    class ItemContainerMessage : IDarkRiftSerializable
    {
        public int networkID {get; private set;}
        public Vector3 position {get; private set;}
        public List<ItemMessage> itemList {get; private set;}
        public ushort type {get; private set;}

        public void Deserialize(DeserializeEvent e)
        {
            itemList = new List<ItemMessage>();
            networkID = e.Reader.ReadInt32();
            position = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
            type = e.Reader.ReadUInt16();

            while (e.Reader.Position < e.Reader.Length) {
                string name = e.Reader.ReadString();
                int amount = e.Reader.ReadInt32();

                itemList.Add (new ItemMessage(name, amount));
            }
        }

        public void Serialize(SerializeEvent e)
        {
            throw new System.NotImplementedException();
        }
    }

    public void RequestAddItem(Item item, ItemContainer itemContainer) {
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(itemContainer.networkID);
            writer.Write(item.itemDefinition.nameID);
            writer.Write(item.amount);

            using (Message message = Message.Create(Tags.AddItemToContainerTag, writer))
                client.SendMessage(message, SendMode.Reliable);
        }
    }

    public void RequestDeleteItem(Item item, ItemContainer itemContainer) {
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(itemContainer.networkID);
            writer.Write(item.itemDefinition.nameID);
            writer.Write(item.amount);

            using (Message message = Message.Create(Tags.DeleteItemFromContainerTag, writer))
                client.SendMessage(message, SendMode.Reliable);
        }
    }

    public void RequestTransferItem(Item item, ItemContainer fromContainer, ItemContainer toContainer) {
        Debug.Log("Request Sent");
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(fromContainer.networkID);
            writer.Write(toContainer.networkID);
            writer.Write(item.itemDefinition.nameID);
            writer.Write(item.amount);

            using (Message message = Message.Create(Tags.TransferItemBetweenContainersTag, writer))
                client.SendMessage(message, SendMode.Reliable);
        }
    }
}
