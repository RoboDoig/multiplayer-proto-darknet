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
        string name;
        int amount;

        public NetworkItem(string _name, int _amount)
        {
            name = _name;
            amount = _amount;
        }

        public void Deserialize(DeserializeEvent e)
        {
            name = e.Reader.ReadString();
            amount = e.Reader.ReadInt32();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(name);
            e.Writer.Write(amount);
        }
    }
}
