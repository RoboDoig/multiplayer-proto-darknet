using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using DarkRift.Server;

namespace ProtoPlugin
{
    public class PlayerManager : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(0, 0, 1);

        // Hard coded, needs to reflect game world map somehow
        const float MAP_WIDTH = 20;

        Dictionary<IClient, Player> players = new Dictionary<IClient, Player>();

        public PlayerManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += ClientConnected;
            ClientManager.ClientDisconnected += ClientDisconnected;
        }

        void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            // When client connects, send its ID back to it
            using (DarkRiftWriter newWriter = DarkRiftWriter.Create())
            {
                newWriter.Write(e.Client.ID);

                using (Message newMessage = Message.Create(Tags.TestMessageTag, newWriter))
                {
                    foreach (IClient client in ClientManager.GetAllClients().Where(x => x == e.Client))
                        client.SendMessage(newMessage, SendMode.Reliable);
                }

            }

            // When client connects, generate new player data
            Random r = new Random();
            Player newPlayer = new Player(
                e.Client.ID,
                (float)r.NextDouble() * MAP_WIDTH - MAP_WIDTH / 2,
                (float)r.NextDouble() * MAP_WIDTH - MAP_WIDTH / 2
            );

            // Write player data and tell other connected clients about this client player
            using (DarkRiftWriter newPlayerWriter = DarkRiftWriter.Create())
            {
                newPlayerWriter.Write(newPlayer.ID);
                newPlayerWriter.Write(newPlayer.X);
                newPlayerWriter.Write(newPlayer.Y);

                using (Message newPlayerMessage = Message.Create(Tags.SpawnPlayerTag, newPlayerWriter))
                {
                    foreach (IClient client in ClientManager.GetAllClients().Where(x => x != e.Client))
                        client.SendMessage(newPlayerMessage, SendMode.Reliable);
                }
            }

            // Add new player to class players reference
            players.Add(e.Client, newPlayer);

            // Tell the client player about all connected players
            using (DarkRiftWriter playerWriter = DarkRiftWriter.Create())
            {
                foreach(Player player in players.Values)
                {
                    playerWriter.Write(player.ID);
                    playerWriter.Write(player.X);
                    playerWriter.Write(player.Y);
                }

                using (Message playerMessage = Message.Create(Tags.SpawnPlayerTag, playerWriter))
                    e.Client.SendMessage(playerMessage, SendMode.Reliable);
            }
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {

        }
    }
}
