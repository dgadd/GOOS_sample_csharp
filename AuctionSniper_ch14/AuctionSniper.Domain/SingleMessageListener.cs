using System;
using AuctionSniper.Fakes.XMPPServer;
using AuctionSniper.Shared;

namespace AuctionSniper.Domain
{
    public class SingleMessageListener : IMessageListener
    {
        #region IMessageListener Members

        public event IMessageListenerEventHandler ProcessMessage;

        public Message Message { get; set; }


        public void InvokeProcessMessage(MessageListenerEventArgs mle)
        {
            IMessageListenerEventHandler handler = ProcessMessage;
            if (handler != null) handler(this, mle);
        }

        #endregion

        public void ReceivesAMessage(MatcherCustom matcher)
        {
            if (Message == null)
            {
                throw new Exception("Message cannot be null");
            }
            matcher.AssertThat(Message.Body);
        }
    }
}