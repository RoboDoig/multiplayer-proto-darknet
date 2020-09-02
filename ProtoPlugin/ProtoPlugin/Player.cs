using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoPlugin
{
    class Player
    {
        public ushort ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Player(ushort id, float x, float y)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
        }
    }
}
