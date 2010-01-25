using AuctionSniper.Fakes.XMPPServer;

namespace AuctionSniper.Domain
{
    public class XMPPAuction : IAuction
    {
        private readonly Chat _chat;

        public XMPPAuction(Chat chat)
        {
            _chat = chat;
        }

        #region IAuction Members

        public void Bid(int bidAmount)
        {
            _chat.SendMessage(string.Format("amount{0}", SharedConstants.BID_COMMAND_FORMAT));
        }

        public void Join()
        {
            _chat.SendMessage(SharedConstants.STATUS_JOINING);
        }

        #endregion
    }
}