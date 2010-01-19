using AuctionSniper.Domain;
using AuctionSniper.Fakes.XMPPServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace AuctionSniper.Tests.Unit
{
    [TestClass]
    public class AuctionMessageTranslatorTest
    {
        private const string SNIPER_ID = "test";
        public const Chat UNUSED_CHAT = null;
        private IAuctionEventListener _mockListener;

        private MockRepository _mocks;
        private AuctionMessageTranslator _translator;

        [TestInitialize]
        public void TestSetup()
        {
            _mocks = new MockRepository();
            _mockListener = _mocks.StrictMock<IAuctionEventListener>();
            _translator = new AuctionMessageTranslator(SNIPER_ID, _mockListener);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mocks.ReplayAll();
            _mocks.VerifyAll();
        }

        [TestMethod]
        public void NotifiesAuctionClosedWhenCloseMessageReceived()
        {
            _mockListener.AuctionClosed();

            _mocks.ReplayAll();

            var message = new Message(UNUSED_CHAT) {Body = "SOLVersion: 1.1; Event: CLOSE;"};
            var mlea = new MessageListenerEventArgs(message);
            _translator.InvokeProcessMessage(mlea);
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromSniper()
        {
            _mockListener.CurrentPrice(234, 5, Enums.PriceSource.FromSniper);

            _mocks.ReplayAll();

            var message = new Message(UNUSED_CHAT)
                              {
                                  Body =
                                      string.Format(
                                      "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 234; Increment: 5; Bidder: {0};",
                                      SNIPER_ID)
                              };
            var mlea = new MessageListenerEventArgs(message);
            _translator.InvokeProcessMessage(mlea);
        }

        [TestMethod]
        public void NotifiesBidDetailsWhenCurrentPriceMessageReceivedFromOtherBidder()
        {
            _mockListener.CurrentPrice(192, 7, Enums.PriceSource.FromOtherBidder);

            _mocks.ReplayAll();

            var message = new Message(UNUSED_CHAT)
                              {
                                  Body =
                                      "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7; Bidder: Someone else;"
                              };
            var mlea = new MessageListenerEventArgs(message);
            _translator.InvokeProcessMessage(mlea);
        }
    }
}