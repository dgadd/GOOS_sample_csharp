using System;

namespace AuctionSniper.Fakes.XMPPServer
{
    public class MessageListenerEventArgs : EventArgs
    {
        public MessageListenerEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; set; }
    }
}