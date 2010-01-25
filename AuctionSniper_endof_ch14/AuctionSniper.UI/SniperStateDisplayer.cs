using AuctionSniper.Domain;

namespace AuctionSniper.UI
{
    public class SniperStateDisplayer : ISniperListener
    {
        private readonly MainPresenter _mainPresenter;

        public SniperStateDisplayer(MainPresenter mainPresenter)
        {
            _mainPresenter = mainPresenter;
        }

        public void SniperLost()
        {
            _mainPresenter.PickerMainView.SniperStatus = SharedConstants.STATUS_LOST;
        }

        public void SniperBidding()
        {
            _mainPresenter.PickerMainView.SniperStatus = SharedConstants.STATUS_BIDDING;
        }

        public void SniperJoining()
        {
            _mainPresenter.PickerMainView.SniperStatus = SharedConstants.STATUS_JOINING;
        }

        public void SniperWinning()
        {
            _mainPresenter.PickerMainView.SniperStatus = SharedConstants.STATUS_WINNING;
        }

        public void SniperWon()
        {
            _mainPresenter.PickerMainView.SniperStatus = SharedConstants.STATUS_WON;
        }
    }
}