using System;
using System.Collections.Generic;

namespace AuctionSniper.Domain
{
    public class AuctionEvent
    {
        private readonly Dictionary<string, string> _fieldPairs = new Dictionary<string, string>();

        private AuctionEvent()
        {
        }

        public string Type
        {
            get { return Get("Event"); }
        }

        public int CurrentPrice
        {
            get { return GetInt("CurrentPrice"); }
        }

        public int Increment
        {
            get { return GetInt("Increment"); }
        }

        // creation method
        public static AuctionEvent From(string messageBody)
        {
            var auctionEvent = new AuctionEvent();
            foreach (string field in GenerateFieldPairs(messageBody))
            {
                auctionEvent.AddField(field);
            }
            return auctionEvent;
        }


        private int GetInt(string fieldName)
        {
            return Convert.ToInt32(Get(fieldName));
        }

        private string Get(string fieldName)
        {
            string result;
            _fieldPairs.TryGetValue(fieldName, out result);
            return result;
        }

        private void AddField(string field)
        {
            if (field.Contains(":"))
            {
                string[] pair = field.Split(':');
                _fieldPairs.Add(pair[0].Trim(), pair[1].Trim());
            }
        }

        private static IEnumerable<string> GenerateFieldPairs(string messageBody)
        {
            return messageBody.Split(';');
        }

        public Enums.PriceSource IsFrom(string sniperId)
        {
            string bidderName = Get("Bidder");
            return sniperId.Equals(bidderName)
                       ? Enums.PriceSource.FromSniper
                       : Enums.PriceSource.FromOtherBidder;
        }
    }
}