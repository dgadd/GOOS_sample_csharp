using System;

namespace AuctionSniper.Domain
{
    public class MatcherCustom
    {
        #region MatchVia enum

        public enum MatchVia
        {
            EqualTo
        }

        #endregion

        private readonly string _baseString;
        private readonly MatchVia _matchVia;

        public MatcherCustom(MatchVia matchVia, string baseString)
        {
            _matchVia = matchVia;
            _baseString = baseString;
        }

        public void AssertThat(string stringToCompare)
        {
            switch (_matchVia)
            {
                case MatchVia.EqualTo:
                    if (_baseString.Equals(stringToCompare))
                    {
                        // assertion is satisfied
                    }
                    else
                    {
                        string message = string.Format("string1: {0} not equal to stringToCompare: {1}", _baseString,
                                                       stringToCompare);
                        throw new Exception(message);
                    }
                    break;
            }
        }
    }
}