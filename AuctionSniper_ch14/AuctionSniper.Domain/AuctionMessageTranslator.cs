using System;
using System.Collections.Generic;
using AuctionSniper.Fakes.XMPPServer;
using AuctionSniper.Shared;

namespace AuctionSniper.Domain
{
    public class AuctionMessageTranslator : IMessageListener
    {
        private readonly IAuctionEventListener _auctionEventListener;
        private readonly string _sniperId;

        public AuctionMessageTranslator(string sniperId, IAuctionEventListener auctionEventListener)
        {
            _sniperId = sniperId;
            _auctionEventListener = auctionEventListener;
        }

        #region IMessageListener Members

        public Message Message { get; set; }

        public event IMessageListenerEventHandler ProcessMessage;

        public void InvokeProcessMessage(MessageListenerEventArgs mle)
        {
            if (MessageHasPairedValues(mle))
            {
                AuctionEvent auctionEvent = AuctionEvent.From(mle.Message.Body);

                switch (auctionEvent.Type)
                {
                    case "CLOSE":
                        _auctionEventListener.AuctionClosed();
                        break;
                    case "PRICE":
                        _auctionEventListener.CurrentPrice(auctionEvent.CurrentPrice, auctionEvent.Increment,
                                                           auctionEvent.IsFrom(_sniperId));
                        break;
                    default:
                        string messageDetail = string.Format("Message type: {0} not handled, from message {1}",
                                                             auctionEvent.Type, mle.Message.Body);
                        throw new Exception(messageDetail);
                }
            }
            else
            {
                if (mle.Message.Body.Contains(SharedConstants.STATUS_JOINING))
                {
                    _auctionEventListener.JoiningAuction();
                }
            }
        }

        #endregion

        private bool MessageHasPairedValues(MessageListenerEventArgs mle)
        {
            return mle.Message.Body.Contains(";");
        }

        private static Dictionary<string, string> UnpackEventFrom(Message message)
        {
            var eventDetails = new Dictionary<string, string>();

            string[] messageElements = message.Body.Split(';');

            foreach (string messageElement in messageElements)
            {
                string[] eventPair = messageElement.Split(':');
                if (eventPair.Length == 2)
                {
                    eventDetails.Add(eventPair[0].Trim(), eventPair[1].Trim());
                }
            }

            return eventDetails;
        }
    }
}