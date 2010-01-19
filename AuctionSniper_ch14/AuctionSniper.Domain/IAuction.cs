namespace AuctionSniper.Domain
{
    public interface IAuction
    {
        void Bid(int bidAmount);
        void Join();
    }
}