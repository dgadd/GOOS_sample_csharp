using System;
using NUnit.Framework;


namespace AuctionSniper.Tests.Acceptance
{
    [TestFixture]
    public class AuctionSniperEndToEndTest
    {
        private ApplicationRunner _application;
        private FakeAuctionServer _auction;

        [SetUp]
        public void Setup()
        {
            _auction = new FakeAuctionServer("item-54321");
            _application = new ApplicationRunner();
        }

        [Test]
        public void SniperJoinsAuctionUntilAuctionCloses()
        {
            _auction.StartSellingItem(); // Step 1
            _application.StartBiddingIn(_auction); // Step 2
            _auction.HasReceivedJoinRequestFromSniper(ApplicationRunner.SNIPER_XMPP_ID); // Step 3
            _auction.AnnounceClosed(); // Step 4
            _application.ShowsSniperHasLostAuction(); // Step 5
        }

        [Test]
        public void SniperMakesAHigherBidButLoses()
        {
            _auction.StartSellingItem();
            _application.StartBiddingIn(_auction);
            _auction.HasReceivedJoinRequestFromSniper(ApplicationRunner.SNIPER_XMPP_ID);
            _auction.ReportPrice(1000, 98, "other bidder");
            _application.HasShownSniperIsBidding();
            _auction.HasReceivedBid(1098, ApplicationRunner.SNIPER_XMPP_ID);
            _auction.AnnounceClosed();
            _application.ShowsSniperHasLostAuction();
        }

        [Test]
        public void SniperWinsAnAuctionByBiddingHigher()
        {
            _auction.StartSellingItem();
            _application.StartBiddingIn(_auction);
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