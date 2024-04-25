using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;
using ChessCore;

namespace ChessGUI
{
    public class ServerClient
    {
        public event EventHandler NewGameStarted;
        SimpleTcpClient client;
        JoinGame joinGame;
        string ipAndPort;
        string moveString;
        

        public ServerClient()
        {
            joinGame = null;
            client = null;
            ipAndPort = null;
        }

        public void Connect(JoinGame join)
        {
            joinGame = join;
            joinGame.JoinGame_Button.IsEnabled = false;
            ipAndPort = joinGame.IpInputBox.Text + ":" + joinGame.PortInputBox.Text;
            client = new SimpleTcpClient(ipAndPort);
            client.Events.DataReceived += Client_DataReceived;
            
            try
            {
                client.Connect();
                joinGame.TextPanel.Text = "Waiting for host to start";

            }
            catch (Exception)
            {
                joinGame.TextPanel.Text = "Try Again";
                joinGame.JoinGame_Button.IsEnabled = true;
            }

        }

        private void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if(Encoding.UTF8.GetString(e.Data) == "GameStarted")
            {
                joinGame.Dispatcher.Invoke(() => {
                    joinGame.TextPanel.Text = "Game started";
                    if (NewGameStarted != null)
                    {
                        NewGameStarted(this, EventArgs.Empty);
                    }
                });
            }
            else
            {
                moveString = Encoding.UTF8.GetString(e.Data);
                Console.WriteLine(moveString);

            }


            
        }

        public void Disconnect()
        {
            if (client!=null)
            {
                joinGame = null;
                client.Disconnect();
            }
            
        }

    }
}
