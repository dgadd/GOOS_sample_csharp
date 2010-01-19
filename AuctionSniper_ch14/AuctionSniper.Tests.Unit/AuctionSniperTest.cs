using AuctionSniper.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace AuctionSniper.Tests.Unit
{
    [TestClass]
    public class AuctionSniperTest
    {
        private IAuction _mockAuction;
        private ISniperListener _mockSniperListener;

        private MockRepository _mocks;
        private Domain.AuctionSniper _sniper;

        [TestInitialize]
        public void TestSetup()
        {
            _mocks = new MockRepository();
            _mockAuction = _mocks.StrictMock<IAuction>();
            _mockSniperListener = _mocks.StrictMock<ISniperListener>();
            _sniper = new Domain.AuctionSniper(_mockAuction, _mockSniperListener);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mocks.ReplayAll();
            _mocks.VerifyAll();
        }

        [TestMethod]
        public void ReportsLostIfAuctionClosesImmediately()
        {
            _mockSniperListener.SniperLost();

            _mocks.ReplayAll();

            _sniper.AuctionClosed();

            _mocks.VerifyAll();
        }

        [TestMethod]
        public void ReportsLostIfAuctionClosesWhenBidding()
        {
            _mockAuction.Bid(0);
            LastCall.IgnoreArguments();
            _mockSniperListener.SniperBidding();
            Expect.Call(_mockSniperListener.SniperLost).Repeat.AtLeastOnce();

            _mocks.ReplayAll();

            _sniper.CurrentPrice(123, 45, Enums.PriceSource.FromOtherBidder);
            _sniper.AuctionClosed();

            _mocks.VerifyAll();
        }

        [TestMethod]
        public void ReportsWonIfAuctionClosesWhenWinning()
        {
            //_mockAuction.Bid(0);
            //LastCall.IgnoreArguments();
            _mockSniperListener.SniperWinning();
            Expect.Call(_mockSniperListener.SniperWon).Repeat.AtLeastOnce();

            _mocks.ReplayAll();

            _sniper.CurrentPrice(123, 45, Enums.PriceSource.FromSniper);
            _sniper.AuctionClosed();

            _mocks.VerifyAll();
        }

        [TestMethod]
        public void BidsHigherAndReportsBiddingWhenNewPriceArrives()
        {
            const int PRICE = 1000;
            const int INCREMENT = 25;
            _mockAuction.Bid(PRICE + INCREMENT);
            Expect.Call(_mockSniperListener.SniperBidding).Repeat.AtLeastOnce();

            _mocks.ReplayAll();

            _sniper.CurrentPrice(PRICE, INCREMENT, Enums.PriceSource.FromOtherBidder);

            _mocks.VerifyAll();
        }

        [TestMethod]
        public void ReportsIsWinningWhenCurrentPriceComesFromSniper()
        {
            const int PRICE = 123;
            const int INCREMENT = 45;
            Expect.Call(_mockSniperListener.SniperWinning).Repeat.AtLeastOnce();

            _mocks.ReplayAll();

            _sniper.CurrentPrice(PRICE, INCREMENT, Enums.PriceSource.FromSniper);

            _mocks.VerifyAll();
        }
    }
}