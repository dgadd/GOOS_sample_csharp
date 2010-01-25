using System.Collections.Generic;

namespace AuctionSniper.Fakes.XMPPServer
{
    public class XMPPConnection
    {
        private static readonly List<XMPPConnection> _xmppConnections = new List<XMPPConnection>();

        private XMPPConnection()
        {
        }

        private XMPPConnection(string xmppHostname)
        {
            XmppHostname = xmppHostname;
            ChatManager = new ChatManager(this);
        }

        public ChatManager ChatManager { get; set; }
        public string XmppHostname { get; set; }
        public string UserName { get; set; }

        public string ServiceName
        {
            get { return "test service"; }
        }

        public static XMPPConnection CreateXMPPConnection(string xmppHostName)
        {
            foreach (XMPPConnection xmppConnection in _xmppConnections)
            {
                if (xmppConnection.XmppHostname == xmppHostName)
                {
                    return xmppConnection;
                }
            }

            var newConnection = new XMPPConnection(xmppHostName);
            _xmppConnections.Add(newConnection);
            return newConnection;
        }

        public void Connect()
        {
        }

        public void Login(string username, string auctionPassword, string auctionResource)
        {
            UserName = username;
        }
    }
}