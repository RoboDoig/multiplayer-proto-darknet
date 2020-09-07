using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;

namespace ProtoPlugin
{
    class NetworkItem : IDarkRiftSerializable
    {
        private static int nextNetworkID = 0;

        public int networkID { get; private set; }
        string name;
        int amount;
        float posX;
        float posY;
        float posZ;

        public NetworkItem(string _name, int _amount, float _posX, float _posY, float _posZ)
        {
            networkID = nextNetworkID;
            name = _name;
            amount = _amount;
            posX = _posX;
            posY = _posY;
            posZ = _posZ;

            nextNetworkID++;
        }

        public void Deserialize(DeserializeEvent e)
        {
            networkID = e.Reader.ReadInt32();
            name = e.Reader.ReadString();
            amount = e.Reader.ReadInt32();
            posX = e.Reader.ReadSingle();
            posY = e.Reader.ReadSingle();
            posZ = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(networkID);
            e.Writer.Write(name);
            e.Writer.Write(amount);
            e.Writer.Write(posX);
            e.Writer.Write(posY);
            e.Writer.Write(posZ);
        }
    }
}
