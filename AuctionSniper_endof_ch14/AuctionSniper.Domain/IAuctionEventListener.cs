namespace AuctionSniper.Domain
{
    public interface IAuctionEventListener
    {
        void AuctionClosed();
        void CurrentPrice(int currentPrice, int increment, Enums.PriceSource fromOtherBidder);
        void JoiningAuction();
    }
}