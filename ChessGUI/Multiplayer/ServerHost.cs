using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;

namespace ChessGUI
{
    internal class ServerHost
    {
        SimpleTcpServer server;
        string password;
        string ipAndPort;
        HostGame hostGame;
        bool connected;
        string clientInfo;
        public event EventHandler NewGameStarted;

        public ServerHost()
        {
            password = null;
            hostGame = null;
            connected = false;
            server = null;
            ipAndPort = null;
            clientInfo = null;

            

        }

        public void StartHosting(HostGame host)
        {
            hostGame = host;
            ipAndPort = hostGame.IpInputBox.Text + ":" + hostGame.PortInputBox.Text;
            server = new SimpleTcpServer(ipAndPort);
            //server.StringEncoder = Encoding.UTF8;
            server.Events.ClientConnected += Server_ClientConnected;
            server.Events.ClientDisconnected += Server_ClientDisconnected;
            server.Events.DataReceived += Server_DataReceived;

            server.Start();
            if (server.IsListening)
            {
                hostGame.HostGame_Button.IsEnabled = false;
                hostGame.StopHosting_Button.IsEnabled = true;
                hostGame.Back_Button.IsEnabled = false;

                hostGame.TextPanel.Text = "Hosting: Waiting for Enemy";
                password = hostGame.PasswordInputBox.Password;
            }



        }

        private void Server_ClientConnected(object sender, ConnectionEventArgs e)
        {
            hostGame.Dispatcher.Invoke(() => {
                hostGame.TextPanel.Text = "Enemy connected";
                hostGame.StartGame_Button.IsEnabled = true;
            });
            clientInfo = (e.IpPort).ToString();
            connected = true;
        }

        private void Server_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            hostGame.Dispatcher.Invoke(() => {
                hostGame.TextPanel.Text = "Enemy disconnected";
                hostGame.StartGame_Button.IsEnabled = false;
            });
            connected = false;

        }

        private void Server_DataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StopHosting()
        {
            if (server.IsListening)
            {
                server.Stop();
                hostGame.TextPanel.Text = "Host a game";
                hostGame.HostGame_Button.IsEnabled = true;
                hostGame.StopHosting_Button.IsEnabled = false;
                hostGame.Back_Button.IsEnabled = true;
                password = null;
            }
        }



        public bool IsConnected()
        {
            return connected;
        }

        public void StartGame()
        {
            server.Send(clientInfo,"GameStarted");
            if (NewGameStarted != null)
            {
                NewGameStarted(this, EventArgs.Empty);
            }
        }

    }
}
