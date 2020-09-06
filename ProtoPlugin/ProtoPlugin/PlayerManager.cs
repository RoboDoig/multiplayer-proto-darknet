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

            // When this client sends a message, we should fire the movement handler
            e.Client.MessageReceived += MovementMessageReceived;
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            players.Remove(e.Client);

            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
                writer.Write(e.Client.ID);

                using (Message message = Message.Create(Tags.DespawnPlayerTag, writer))
                {
                    foreach (IClient client in ClientManager.GetAllClients())
                        client.SendMessage(message, SendMode.Reliable);
                }
            }
        }

        void MovementMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage() as Message)
            {
                if (message.Tag == Tags.MovePlayerTag)
                {
                    using(DarkRiftReader reader = message.GetReader())
                    {
                        // what updated position did we receive?
                        float newX = reader.ReadSingle();
                        float newY = reader.ReadSingle();
                        float rotX = reader.ReadSingle();
                        float rotY = reader.ReadSingle();
                        float rotZ = reader.ReadSingle();
                        float rotW = reader.ReadSingle();

                        // update specified player with this information
                        Player player = players[e.Client];

                        player.X = newX;
                        player.Y = newY;
                        player.rotX = rotX;
                        player.rotY = rotY;
                        player.rotZ = rotZ;
                        player.rotW = rotW;

                        // send this player's updated position back to all clients except the client that sent the message
                        using (DarkRiftWriter writer = DarkRiftWriter.Create())
                        {
                            writer.Write(player.ID);
                            writer.Write(player.X);
                            writer.Write(player.Y);
                            writer.Write(player.rotX);
                            writer.Write(player.rotY);
                            writer.Write(player.rotZ);
                            writer.Write(player.rotW);

                            message.Serialize(writer);
                        }

                        foreach (IClient c in ClientManager.GetAllClients().Where(x => x != e.Client))
                            c.SendMessage(message, e.SendMode);
                    }
                }
            }
        }

    }
}
