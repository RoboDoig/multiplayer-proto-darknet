using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoPlugin
{
    static class Tags
    {
        public static readonly ushort SpawnPlayerTag = 1001;
        public static readonly ushort DespawnPlayerTag = 1002;
        public static readonly ushort MovePlayerTag = 1003;
        public static readonly ushort SpawnPlayerInventoryTag = 1004;

        public static readonly ushort SpawnItemContainerTag = 6001;
        public static readonly ushort AddItemToContainerTag = 6002;
        public static readonly ushort DeleteItemFromContainerTag = 6003;
        public static readonly ushort TransferItemBetweenContainersTag = 6004;
    }
}
