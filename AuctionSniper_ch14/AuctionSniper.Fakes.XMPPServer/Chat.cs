using System;
using System.Collections.Generic;

namespace AuctionSniper.Fakes.XMPPServer
{
    public class Chat
    {
        private readonly List<IMessageListener> _messageListeners = new List<IMessageListener>();

        private Chat()
        {
        }

        public Chat(string auctionId, string participant)
        {
            AuctionId = auctionId;
            Participant = participant;
        }

        public string AuctionId { get; set; }
        public string Participant { get; set; }

        public Message Message { get; set; }

        public List<IMessageListener> MessageListeners
        {
            get { return _messageListeners; }
        }

        public event EventHandler MessageSent;

        protected void OnMessageSent(EventArgs e)
        {
            MessageSent(this, e);
        }

        public void AddIMessageListener(IMessageListener messageListener)
        {
            MessageListeners.Add(messageListener);
        }

        public void SendMessage(string messageBody)
        {
            Message = new Message(this);
            Message.Body = messageBody;

            foreach (IMessageListener messageListener in MessageListeners)
            {
                var mle = new MessageListenerEventArgs(Message);
                messageListener.InvokeProcessMessage(mle);
            }
        }
    }
}