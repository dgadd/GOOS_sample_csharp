namespace AuctionSniper.Views
{
    public interface IPickerMainView
    {
        string WindowTitle { get; set; }

        string SniperStatus { get; set; }
        string ShowDialog(int matches);
    }
}