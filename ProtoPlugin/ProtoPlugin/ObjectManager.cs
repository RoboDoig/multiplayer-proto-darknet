using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift;
using DarkRift.Server;

namespace ProtoPlugin
{
    class ObjectManager : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(0, 0, 1);

        public ObjectManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += ClientConnected;
        }

        void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            // Send all existing objects to the client

            // When this client sends a message, we should fire the message received method
            e.Client.MessageReceived += MessageReceived;
        }

        void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage() as Message)
            {
                if (message.Tag == Tags.PlaceObjectTag)
                {
                    using (DarkRiftReader reader = message.GetReader())
                    {
                        // Position of object to be placed
                        float posX = reader.ReadSingle();
                        float posY = reader.ReadSingle();
                        float posZ = reader.ReadSingle();

                        // Check logic

                        // Add object to server object reference list

                        // Send confirmation back to client
                        using (DarkRiftWriter writer = DarkRiftWriter.Create())
                        {
                            writer.Write(posX);
                            writer.Write(posY);
                            writer.Write(posZ);

                            message.Serialize(writer);
                        }

                        foreach (IClient c in ClientManager.GetAllClients())
                            c.SendMessage(message, e.SendMode);
                    }
                }
            }
        }
    }
}
