namespace AuctionSniper.Domain
{
    public interface ISniperListener
    {
        void SniperLost();
        void SniperBidding();
        void SniperJoining();
        void SniperWinning();
        void SniperWon();
    }
}