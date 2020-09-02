using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoPlugin
{
    static class Tags
    {
        public static readonly ushort TestMessageTag = 0;
        public static readonly ushort SpawnPlayerTag = 1;
        public static readonly ushort DespawnPlayerTag = 2;
        public static readonly ushort MovePlayerTag = 3;
    }
}
