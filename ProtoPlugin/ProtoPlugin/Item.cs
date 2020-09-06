using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoPlugin
{
    public class Item
    {
        public int networkID;
        private static int nextNetworkID = 0;
        public string name;
        public string description;
        public float weight;

        public Item()
        {
            name = "item";
            description = "Default base item";
            networkID = nextNetworkID;
            nextNetworkID++;
        }
    }
}
