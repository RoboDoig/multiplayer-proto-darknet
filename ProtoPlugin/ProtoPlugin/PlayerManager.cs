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
        }

        void ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {

        }
    }
}
