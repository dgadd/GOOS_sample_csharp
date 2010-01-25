using System;
using System.Collections.Generic;

namespace AuctionSniper.Fakes.XMPPServer
{
    public class ChatManager
    {
        private readonly List<Chat> _chats = new List<Chat>();
        public Guid ID;

        public ChatManager()
        {
            ID = Guid.NewGuid();
        }

        public ChatManager(XMPPConnection xmppConnection)
        {
            XMPPConnection = xmppConnection;
        }

        public XMPPConnection XMPPConnection { get; set; }

        public List<Chat> Chats
        {
            get { return _chats; }
        }

        public Chat CurrentChat { get; set; }

        public event EventHandler ChatCreated;

        private void OnChatCreated(EventArgs e)
        {
            if (ChatCreated != null)
                ChatCreated(this, e);
        }

        public void RequestNewChat(string auctionId, string participant, Message message)
        {
            CurrentChat = new Chat(auctionId, participant);
            CurrentChat.Message = message;
            Chats.Add(CurrentChat);
            OnChatCreated(EventArgs.Empty);
        }

        public Chat CreateChat(string auctionId, string participant, IMessageListener messageListener)
        {
            CurrentChat = new Chat(auctionId, participant);
            CurrentChat.AddIMessageListener(messageListener);
            Chats.Add(CurrentChat);
            OnChatCreated(EventArgs.Empty);
            return CurrentChat;
        }

        public Chat CreateChat(string auctionId, IMessageListener messageListener)
        {
            CurrentChat = new Chat(auctionId, XMPPConnection.UserName);
            if (messageListener != null)
            {
                CurrentChat.AddIMessageListener(messageListener);
            }
            Chats.Add(CurrentChat);
            OnChatCreated(EventArgs.Empty);
            return CurrentChat;
        }
    }
}