using AuctionSniper.Presenters;
using AuctionSniper.Shared;
using AuctionSniper.Views;

namespace AuctionSniper.Tests.Acceptance
{
    public class ApplicationRunner
    {
        public const string SNIPER_ID = "sniper";
        public const string SNIPER_XMPP_ID = "sniper@macbookpc/Auction";
        public readonly string SNIPER_PASSWORD = "sniper";

        public void StartBiddingIn(FakeAuctionServer auction, IPickerMainView mockPickerMainView)
        {
            var main = new MainPresenter(mockPickerMainView);
            main.Main(SharedConstants.XMPP_HOSTNAME, SNIPER_XMPP_ID, SNIPER_PASSWORD, auction.ItemId);
        }

        public void ShowsSniperHasWonAuction()
        {
            // The mocks.VerifyAll() method occuring in the calling call will take care of verifying this.
        }

        public void ShowsSniperHasLostAuction()
        {
            // The mocks.VerifyAll() method occuring in the calling call will take care of verifying this.
        }

        public void HasShownSniperIsBidding()
        {
            // The mocks.VerifyAll() method occuring in the calling call will take care of verifying this.
        }

        public void HasShownSniperIsWinning()
        {
            // The mocks.VerifyAll() method occuring in the calling call will take care of verifying this.
        }
    }
}