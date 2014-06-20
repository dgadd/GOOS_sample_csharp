using AuctionSniper.Domain;
using NUnit.Framework;
using Rhino.Mocks;

namespace AuctionSniper.Tests.Unit
{
    [TestFixture]
    public class AuctionSniperTest
    {
        private IAuction _mockAuction;
        private ISniperListener _mockSniperListener;

        private Domain.AuctionSniper _sniper;

        [SetUp]
        public void TestSetup()
        {
            _mockAuction = MockRepository.GenerateStrictMock<IAuction>();
            _mockSniperListener = MockRepository.GenerateStrictMock<ISniperListener>();
            _sniper = new Domain.AuctionSniper(_mockAuction, _mockSniperListener);
        }

        [Test]
        public void ReportsLostIfAuctionClosesImmediately()
        {
            _mockSniperListener.Expect(x => x.SniperLost());

            _sniper.AuctionClosed();
        }

        [Test]
        public void ReportsLostIfAuctionClosesWhenBidding()
        {
            _mockAuction.Expect(x => x.Bid(0)).IgnoreArguments();
            _mockSniperListener.Expect(x => x.SniperBidding());
            _mockSniperListener.Expect(x => x.SniperLost()).Repeat.AtLeastOnce();

            _sniper.CurrentPrice(123, 45, Enums.PriceSource.FromOtherBidder);
            _sniper.AuctionClosed();

            _mockSniperListener.VerifyAllExpectations();
        }

        [Test]
        public void ReportsWonIfAuctionClosesWhenWinning()
        {
            //_mockAuction.Expect(x => x.Bid(0)).IgnoreArguments();
            _mockSniperListener.Expect(x => x.SniperWinning());
            _mockSniperListener.Expect(x => x.SniperWon()).Repeat.AtLeastOnce();

            _sniper.CurrentPrice(123, 45, Enums.PriceSource.FromSniper);
            _sniper.AuctionClosed();

            _mockSniperListener.VerifyAllExpectations();
        }

        [Test]
        public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
        {
            const int PRICE = 1000;
            const int INCREMENT = 25;

            _mockAuction.Expect(x => x.Bid(PRICE + INCREMENT));
            _mockSniperListener.Expect(x => x.SniperBidding()).Repeat.AtLeastOnce();

            _sniper.CurrentPrice(PRICE, INCREMENT, Enums.PriceSource.FromOtherBidder);

            _mockSniperListener.VerifyAllExpectations();
        }

        [Test]
        public void ReportsIsWinningWhenCurrentPriceComesFromSniper()
        {
            const int PRICE = 123;
            const int INCREMENT = 45;

            _mockSniperListener.Expect(x => x.SniperWinning()).Repeat.AtLeastOnce();

            _sniper.CurrentPrice(PRICE, INCREMENT, Enums.PriceSource.FromSniper);

            _mockSniperListener.VerifyAllExpectations();
        }
    }
}