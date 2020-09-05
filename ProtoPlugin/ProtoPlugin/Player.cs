using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;

namespace ProtoPlugin
{
    class Player: IDarkRiftSerializable
    {
        public ushort ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float rotX { get; set; }
        public float rotY { get; set; }
        public float rotZ { get; set; }
        public float rotW { get; set; }

        public Player(ushort id, float x, float y)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
        }

        public void Deserialize(DeserializeEvent e)
        {
            ID = e.Reader.ReadUInt16();
            X = e.Reader.ReadSingle();
            Y = e.Reader.ReadSingle();
            rotX = e.Reader.ReadSingle();
            rotY = e.Reader.ReadSingle();
            rotZ = e.Reader.ReadSingle();
            rotW = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(ID);
            e.Writer.Write(X);
            e.Writer.Write(Y);
            e.Writer.Write(rotX);
            e.Writer.Write(rotY);
            e.Writer.Write(rotZ);
            e.Writer.Write(rotW);
        }
    }
}
