using System;

namespace AuctionSniper.Domain
{
    public class AuctionSniper : IAuctionEventListener
    {
        private readonly IAuction _auction;
        private readonly ISniperListener _sniperListener;
        private Boolean _isWinning;

        public AuctionSniper(IAuction auction, ISniperListener sniperListener)
        {
            _auction = auction;
            _sniperListener = sniperListener;
        }

        #region IAuctionEventListener Members

        public void AuctionClosed()
        {
            if (_isWinning)
            {
                _sniperListener.SniperWon();
            }
            else
            {
                _sniperListener.SniperLost();
            }
        }

        public void CurrentPrice(int currentPrice, int increment, Enums.PriceSource priceSource)
        {
            _isWinning = (priceSource == Enums.PriceSource.FromSniper);

            if (_isWinning)
            {
                _sniperListener.SniperWinning();
            }
            else
            {
                _auction.Bid(currentPrice + increment);
                _sniperListener.SniperBidding();
            }
        }

        public void JoiningAuction()
        {
            _sniperListener.SniperJoining();
        }

        #endregion
    }
}