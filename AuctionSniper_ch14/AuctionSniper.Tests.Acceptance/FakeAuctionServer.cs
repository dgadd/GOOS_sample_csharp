using System;
using AuctionSniper.Domain;
using AuctionSniper.Fakes.XMPPServer;
using AuctionSniper.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuctionSniper.Tests.Acceptance
{
    public class FakeAuctionServer
    {
        public const String ITEM_ID_AS_LOGIN = "auction-{0}";
        public const String AUCTION_RESOURCE = "Auction";
        private const String AUCTION_PASSWORD = "auction";

        private readonly XMPPConnection _connection;
        private readonly String _itemId;

        private readonly SingleMessageListener _singleMessageListener = new SingleMessageListener();
        private Chat _currentChat;

        public FakeAuctionServer(String itemId)
        {
            _itemId = itemId;
            _connection = XMPPConnection.CreateXMPPConnection(SharedConstants.XMPP_HOSTNAME);
        }

        public string ItemId
        {
            get { return _itemId; }
        }

        public void StartSellingItem()
        {
            _connection.Connect();
            _connection.Login(string.Format(ITEM_ID_AS_LOGIN, _itemId), AUCTION_PASSWORD, AUCTION_RESOURCE);
            //_connection.ChatManager.ChatCreated += new EventHandler(FakeAuctionServer_ChatCreated);
            _connection.ChatManager.ChatCreated += ChatManager_ChatCreated;
        }

        private void ChatManager_ChatCreated(object sender, EventArgs e)
        {
            _currentChat = ((ChatManager) sender).CurrentChat;
            _currentChat.AddIMessageListener(_singleMessageListener);
            _singleMessageListener.ProcessMessage += MessageListenerProcessMessage;
        }

        private static void MessageListenerProcessMessage(object sender, MessageListenerEventArgs mle)
        {
            var messageListener = (IMessageListener) sender;
            messageListener.Message = mle.Message;
        }

        internal void HasReceivedJoinRequestFromSniper(string sniperId)
        {
            ReceivesAMessageMatching(sniperId,
                                     new MatcherCustom(MatcherCustom.MatchVia.EqualTo,
                                                       SharedConstants.STATUS_JOINING));
        }

        internal void AnnounceClosed()
        {
            _currentChat.SendMessage("SOLVersion: 1.1; Event: CLOSE;");
        }

        public void ReportPrice(int price, int increment, string otherBidder)
        {
            string message =
                string.Format("SOLVersion: 1.1; Event: PRICE; CurrentPrice: {0}; Increment: {1}; Bidder: {2};",
                              price,
                              increment,
                              otherBidder);
            _currentChat.SendMessage(message);
        }

        public void HasReceivedBid(int expectedBid, string sniperId)
        {
            ReceivesAMessageMatching(sniperId,
                                     new MatcherCustom(MatcherCustom.MatchVia.EqualTo,
                                                       string.Format("amount{0}", SharedConstants.BID_COMMAND_FORMAT)));
        }

        private void ReceivesAMessageMatching(string sniperId, MatcherCustom matcher)
        {
            _singleMessageListener.ReceivesAMessage(matcher);
            Assert.AreEqual(_singleMessageListener.Message.Chat.Participant, sniperId);
        }
    }
}