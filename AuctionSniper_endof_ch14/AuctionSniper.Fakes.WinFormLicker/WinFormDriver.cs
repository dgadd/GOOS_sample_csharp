using System;
using System.Threading;
using System.Windows.Forms;
using AuctionSniper.UI;

namespace AuctionSniper.Fakes.WinFormLicker
{
    public class WinFormDriver
    {
        protected readonly Main _main;
        protected Thread _thread;
        protected int _sleepMilliseconds;


        public WinFormDriver(Main main, int sleepMilliseconds)
        {
            _main = main;
            _sleepMilliseconds = sleepMilliseconds;
        }

        public Main Main
        {
            get { return _main; }
        }

        public void LaunchApplicationInItsOwnThread()
        {
            _thread = new Thread(new ParameterizedThreadStart(Launch));
            _thread.Start(this.Main);
            this.Main.Show();
            this.Main.BringToFront();
            this.Main.Refresh();
        }

        public void QuitApplication()
        {
            Thread.Sleep(_sleepMilliseconds);
            this.Main.Close();
            Application.Exit();
        }

        private static void Launch(object input)
        {
            var form = (Form)input;
            Application.Run(form);
        }
    }
}