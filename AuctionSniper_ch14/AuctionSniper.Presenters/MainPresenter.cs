using System;
using AuctionSniper.Domain;
using AuctionSniper.Fakes.XMPPServer;
using AuctionSniper.Shared;
using AuctionSniper.Views;

namespace AuctionSniper.Presenters
{
    public class MainPresenter
    {
        private const string AUCTION_RESOURCE = "Auction";
        public const String ITEM_ID_AS_LOGIN = "auction-{0}";
        public const String AUCTION_ID_FORMAT = "auction-{0}@{1}/Auction";
        private readonly IPickerMainView _pickerMainView;
        private string _itemId;
        private string _sniperId;
        private string _sniperPassword;
        private string _xmppHostName;

        public MainPresenter(IPickerMainView pickerMainView)
        {
            _pickerMainView = pickerMainView;
        }

        public IPickerMainView PickerMainView
        {
            get { return _pickerMainView; }
        }

        public void Main(string xmppHostName, string sniperId, string sniperPassword, string itemId)
        {
            _xmppHostName = xmppHostName;
            _sniperId = sniperId;
            _sniperPassword = sniperPassword;
            _itemId = itemId;

            PickerMainView.WindowTitle = SharedConstants.MAIN_WINDOW_TITLE;

            XMPPConnection connection = ConnectTo(_xmppHostName, _sniperId, _sniperPassword);
            JoinAuction(connection, itemId);
        }

        private void JoinAuction(XMPPConnection connection, string itemId)
        {
            string formattedAuctionId = AuctionId(itemId, connection);
            Chat chat = connection.ChatManager.CreateChat(formattedAuctionId, null);

            const int BID_AMOUNT = 35;
            IAuction auction = new XMPPAuction(chat);
            chat.AddIMessageListener(
                new AuctionMessageTranslator(
                    connection.UserName,
                    new Domain.AuctionSniper(auction, new SniperStateDisplayer(this))));
            auction.Join();
        }

        private static XMPPConnection ConnectTo(String hostname, String username, String password)
        {
            XMPPConnection connection = XMPPConnection.CreateXMPPConnection(hostname);
            connection.Connect();
            connection.Login(username, password, AUCTION_RESOURCE);

            return connection;
        }

        private static string AuctionId(String itemId, XMPPConnection connection)
        {
            return string.Format(AUCTION_ID_FORMAT, itemId, connection.ServiceName);
        }

        public void AuctionClosed()
        {
            PickerMainView.SniperStatus = SharedConstants.STATUS_CLOSED;
        }

        public void CurrentPrice(int currentPrice, int increment)
        {
        }
    }
}