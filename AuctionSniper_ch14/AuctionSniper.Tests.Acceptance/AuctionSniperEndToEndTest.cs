using System;
using AuctionSniper.Shared;
using AuctionSniper.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace AuctionSniper.Tests.Acceptance
{
    [TestClass]
    public class AuctionSniperEndToEndTest
    {
        private ApplicationRunner _application;
        private FakeAuctionServer _auction;
        private IPickerMainView _mockPickerMainView;
        private MockRepository _mocks;

        [TestInitialize]
        public void TestSetup()
        {
            _auction = new FakeAuctionServer("item-54321");
            _application = new ApplicationRunner();
            _mocks = new MockRepository();
            _mockPickerMainView = _mocks.StrictMock<IPickerMainView>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mocks.ReplayAll();
            _mocks.VerifyAll();
        }

        [TestMethod]
        public void SniperJoinsAuctionUntilAuctionCloses()
        {
            _mockPickerMainView.WindowTitle = SharedConstants.MAIN_WINDOW_TITLE;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_JOINING;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_LOST;

            _mocks.ReplayAll();
            _auction.StartSellingItem(); // Step 1
            _application.StartBiddingIn(_auction, _mockPickerMainView); // Step 2
            _auction.HasReceivedJoinRequestFromSniper(ApplicationRunner.SNIPER_XMPP_ID); // Step 3
            _auction.AnnounceClosed(); // Step 4
            _application.ShowsSniperHasLostAuction(); // Step 5
        }

        [TestMethod]
        public void SniperMakesAHigherBidButLoses()
        {
            _mockPickerMainView.WindowTitle = SharedConstants.MAIN_WINDOW_TITLE;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_JOINING;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_BIDDING;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_LOST;

            _mocks.ReplayAll();
            _auction.StartSellingItem();
            _application.StartBiddingIn(_auction, _mockPickerMainView);
            _auction.HasReceivedJoinRequestFromSniper(ApplicationRunner.SNIPER_XMPP_ID);
            _auction.ReportPrice(1000, 98, "other bidder");
            _application.HasShownSniperIsBidding();
            _auction.HasReceivedBid(1098, ApplicationRunner.SNIPER_XMPP_ID);
            _auction.AnnounceClosed();
            _application.ShowsSniperHasLostAuction();
        }

        [TestMethod]
        public void SniperWinsAnAuctionByBiddingHigher()
        {
            _mockPickerMainView.WindowTitle = SharedConstants.MAIN_WINDOW_TITLE;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_JOINING;
            Expect.Call(_mockPickerMainView.SniperStatus = SharedConstants.STATUS_BIDDING).Repeat.AtLeastOnce();
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_WINNING;
            _mockPickerMainView.SniperStatus = SharedConstants.STATUS_WON;

            _mocks.ReplayAll();
            _auction.StartSellingItem();
            _application.StartBiddingIn(_auction, _mockPickerMainView);
            _auction.HasReceivedJoinRequestFromSniper(ApplicationRunner.SNIPER_XMPP_ID);
            _auction.ReportPrice(1000, 98, "other bidder");
            _application.HasShownSniperIsBidding();
            _auction.HasReceivedBid(1098, ApplicationRunner.SNIPER_XMPP_ID);
            _auction.ReportPrice(1098, 97, ApplicationRunner.SNIPER_XMPP_ID);
            _application.HasShownSniperIsWinning();
            _auction.AnnounceClosed();
            _application.ShowsSniperHasWonAuction();
        }
    }
}