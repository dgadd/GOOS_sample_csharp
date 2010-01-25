using AuctionSniper.Domain;
using AuctionSniper.UI;

namespace AuctionSniper.Tests.Acceptance
{
    public class ApplicationRunner
    {
        public const string SNIPER_ID = "sniper";
        public const string SNIPER_XMPP_ID = "sniper@macbookpc/Auction";
        public readonly string SNIPER_PASSWORD = "sniper";
        private AuctionSniperDriver _driver;

        public void StartBiddingIn(FakeAuctionServer auction)
        {
            _driver = new AuctionSniperDriver(new Main(), 1000);
            var main = new MainPresenter(_driver.Main);
            main.Main(SharedConstants.XMPP_HOSTNAME, SNIPER_XMPP_ID, SNIPER_PASSWORD, auction.ItemId);
            _driver.LaunchApplicationInItsOwnThread();
            _driver.ShowsSniperStatus(SharedConstants.STATUS_JOINING);
        }

        public void ShowsSniperHasWonAuction()
        {
            if (_driver != null) _driver.ShowsSniperStatus(SharedConstants.STATUS_WON);
        }

        public void ShowsSniperHasLostAuction()
        {
            if (_driver != null) _driver.ShowsSniperStatus(SharedConstants.STATUS_LOST);
        }

        public void HasShownSniperIsBidding()
        {
            if (_driver != null) _driver.ShowsSniperStatus(SharedConstants.STATUS_BIDDING);
        }

        public void HasShownSniperIsWinning()
        {
            if (_driver != null) _driver.ShowsSniperStatus(SharedConstants.STATUS_WINNING);
        }
    }
}