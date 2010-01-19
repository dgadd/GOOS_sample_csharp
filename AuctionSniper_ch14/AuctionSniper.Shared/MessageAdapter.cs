using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuctionSniper.Shared
{
    public static class MessageAdapter
    {
        public static string MessageConversion(string message)
        {
            // 2010Jan09  David Gadd
            // This STUPID, WEIRD override, while unneccessary and pointless, is a close equivalent
            // to what the GOOS book is instructing the Java code to do. It's WindowLicker derived
            // class does normal event handling, but then its Main method in ch. 11 arbitrarily overrides
            // that to set it to Lost. BIZZARE.
            if (message == SharedConstants.STATUS_CLOSED)
            {
                return SharedConstants.STATUS_LOST;
            }
            else if (message.StartsWith("SOLVersion: 1.1; Event: PRICE"))
            {
                return SharedConstants.STATUS_BIDDING;
            }

            return message;
        }
    }
}
