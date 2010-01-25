using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AuctionSniper.UI
{
    public class Main : Form, IPickerMainView
    {
        private readonly Label _lblStatus;

        public Main()
        {
            _lblStatus = new Label();
            this.Controls.Add(_lblStatus);
        }

        public string SniperStatus
        {
            get
            {
                return _lblStatus.Text;
            }
            set
            {
                _lblStatus.Text = value;
            }
        }

        public string WindowTitle
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        //public string ShowDialog(int matches)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
