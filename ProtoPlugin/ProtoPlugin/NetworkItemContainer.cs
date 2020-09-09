using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;

namespace ProtoPlugin
{
    class NetworkItemContainer : IDarkRiftSerializable
    {
        public static Dictionary<int, NetworkItemContainer> itemContainerDictionary = new Dictionary<int, NetworkItemContainer>();
        private static int nextNetworkID = 0;
        public int networkID { get; private set; }
        public List<NetworkItem> contents { get; private set; }
        float posX;
        float posY;
        float posZ;
        public ushort type; // 0 = free item, 1 = player inventory, 2 = structure inventory

        public NetworkItemContainer(float _posX, float _posY, float _posZ, ushort _type)
        {
            networkID = nextNetworkID;
            posX = _posX;
            posY = _posY;
            posZ = _posZ;
            type = _type;
            contents = new List<NetworkItem>();

            nextNetworkID++;
            itemContainerDictionary.Add(networkID, this);
        }

        // Check if container has amount of an item
        public bool CheckHasItem(NetworkItem item)
        {
            foreach (NetworkItem contentsItem in contents)
            {
                if (contentsItem.name == item.name)
                {
                    if (contentsItem.amount >= item.amount)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public void AddItem(NetworkItem item)
        {
            foreach (NetworkItem contentsItem in contents)
            {
                if (contentsItem.name == item.name)
                {
                    contentsItem.amount += item.amount;
                    return;
                }
            }

            contents.Add(item);
        }

        public void DeleteItem(NetworkItem item)
        {
            foreach (NetworkItem contentsItem in contents)
            {
                if (contentsItem.name == item.name)
                {
                    if (contentsItem.amount - item.amount >= 0)
                    {
                        contentsItem.amount -= item.amount;
                        return;
                    }
                }
            }
        }

        public void Deserialize(DeserializeEvent e)
        {
            networkID = e.Reader.ReadInt32();
            posX = e.Reader.ReadSingle();
            posY = e.Reader.ReadSingle();
            posZ = e.Reader.ReadSingle();
            type = e.Reader.ReadUInt16();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(networkID);
            e.Writer.Write(posX);
            e.Writer.Write(posY);
            e.Writer.Write(posZ);
            e.Writer.Write(type);

            foreach(NetworkItem item in contents)
            {
                e.Writer.Write(item);
            }
        }
    }
}
