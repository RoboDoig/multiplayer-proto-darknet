using System.Collections;
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

    Dictionary<int, Item> networkItems = new Dictionary<int, Item>();

    void Awake() {
        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            if (message.Tag == Tags.SpawnItemTag) {

                // Spawn Item
                ItemMessage itemMessage = message.Deserialize<ItemMessage>();
                GameObject obj = Instantiate(defaultContainer, itemMessage.position, Quaternion.identity) as GameObject;
                Item newItem = new Item(ItemManager.allItems[itemMessage.name], itemMessage.amount);
                obj.GetComponent<ItemContainer>().contents.Add(newItem);
                networkItems.Add(itemMessage.networkID, newItem);

                //
            }
        }
    }

    class ItemMessage : IDarkRiftSerializable
    {
        public int networkID {get; private set;}
        public string name {get; private set;}
        public int amount {get; private set;}
        public Vector3 position {get; private set;}

        public void Deserialize(DeserializeEvent e)
        {
            networkID = e.Reader.ReadInt32();
            name = e.Reader.ReadString();
            amount = e.Reader.ReadInt32();
            position = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
        }

        public void Serialize(SerializeEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}
