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

        public NetworkItem(string _name, int _amount)
        {
            networkID = nextNetworkID;
            name = _name;
            amount = _amount;

            nextNetworkID++;
        }

        public void Deserialize(DeserializeEvent e)
        {
            networkID = e.Reader.ReadInt32();
            name = e.Reader.ReadString();
            amount = e.Reader.ReadInt32();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(networkID);
            e.Writer.Write(name);
            e.Writer.Write(amount);
        }
    }
}
