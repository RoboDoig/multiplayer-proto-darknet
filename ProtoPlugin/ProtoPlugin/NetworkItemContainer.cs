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
        private static int nextNetworkID = 0;
        public int networkID { get; private set; }
        List<NetworkItem> contents;
        float posX;
        float posY;
        float posZ;

        public NetworkItemContainer(float _posX, float _posY, float _posZ)
        {
            networkID = nextNetworkID;
            posX = _posX;
            posY = _posY;
            posZ = _posZ;
            contents = new List<NetworkItem>();

            nextNetworkID++;
        }

        public void AddItem(NetworkItem item)
        {
            contents.Add(item);
        }

        public void Deserialize(DeserializeEvent e)
        {
            networkID = e.Reader.ReadInt32();
            posX = e.Reader.ReadSingle();
            posY = e.Reader.ReadSingle();
            posZ = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(networkID);
            e.Writer.Write(posX);
            e.Writer.Write(posY);
            e.Writer.Write(posZ);

            foreach(NetworkItem item in contents)
            {
                e.Writer.Write(item);
            }
        }
    }
}
