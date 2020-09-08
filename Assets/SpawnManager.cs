using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    UnityClient client;

    [SerializeField]
    NetworkPlayerManager networkPlayerManager;

    [SerializeField]
    GameObject localPlayerPrefab;
    [SerializeField]
    GameObject networkPlayerPrefab;

    void Awake() {
        if (client == null) {
            Debug.Log("Client unassigned in SpawnManager");
            Application.Quit();
        }

        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            if (message.Tag == Tags.SpawnPlayerTag) {
                SpawnPlayer(sender, e);
            } else if (message.Tag == Tags.DespawnPlayerTag) {
                DespawnPlayer(sender, e);
            }
        }
    }

    void SpawnPlayer(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader()) {

            PlayerMessage playerMessage = reader.ReadSerializable<PlayerMessage>();

            Vector3 position = new Vector3(playerMessage.X, 1f, playerMessage.Y);
            GameObject obj;
            Inventory inv;
            // if the message refers to us as a local player, instantiate controllable local player
            if (playerMessage.ID == client.ID) {
                obj = Instantiate(localPlayerPrefab, position, Quaternion.identity) as GameObject;
                obj.GetComponent<Player>().Client = client;
                inv = obj.GetComponent<Inventory>();
                inv.networkID = playerMessage.networkInventoryID;                   
            // else instantiate non-controllable network player
            } else {
                obj = Instantiate(networkPlayerPrefab, position, Quaternion.identity) as GameObject;
                inv = obj.GetComponent<Inventory>();
                inv.networkID = playerMessage.networkInventoryID;
            }

            inv.contents.Add(playerMessage.networkInventoryContents[0]);
            networkPlayerManager.Add(playerMessage.ID, obj.GetComponent<NetworkPlayer>());       
        }
    }

    void DespawnPlayer(object sender, MessageReceivedEventArgs e) {
        using (Message message  = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader()) {
            networkPlayerManager.DestroyPlayer(reader.ReadUInt16());
        }
    }

    class PlayerMessage : IDarkRiftSerializable
    {
        public ushort ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float rotX { get; set; }
        public float rotY { get; set; }
        public float rotZ { get; set; }
        public float rotW { get; set; }
        public int networkInventoryID;
        public List<Item> networkInventoryContents = new List<Item>();

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadUInt16();
            X = e.Reader.ReadSingle();
            Y = e.Reader.ReadSingle();
            rotX = e.Reader.ReadSingle();
            rotY = e.Reader.ReadSingle();
            rotZ = e.Reader.ReadSingle();
            rotW = e.Reader.ReadSingle();
            networkInventoryID = e.Reader.ReadInt32();

            while (e.Reader.Position < e.Reader.Length) {
                Item newItem = new Item(ItemManager.allItems[e.Reader.ReadString()], e.Reader.ReadInt32());
                networkInventoryContents.Add(newItem);
            }
        }

        public void Serialize(SerializeEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}
