using System;
using System.Collections.Generic;
using System.Text;

namespace ByteBlocks.AmazonAPI
{
    public class DigitalMusicSearch : AmazonSearch
    {
        private DigitalMusicSearchParameters _searchParameters;
        public List<DigitalMusic> Search(SearchRequestParameters requestParameters)
        {
            _searchParameters = requestParameters as DigitalMusicSearchParameters;

            if (null == _searchParameters)
            {
                throw new ArgumentException("Invalid request parameter object");
            }
            SetupRequest(requestParameters);

            WebServiceResult result = PerformSearch(requestParameters);

            if (result.StatusCode == 0)
            {
                // Convert the list of Amazon items to typed results.
                List<DigitalMusic> items = new List<DigitalMusic>();
                for (int i = 0; i < result.ResultItems.Length; i++)
                {
                    items.Add(new DigitalMusic(result.ResultItems[i]));
                }
                return items;
            }
            return null;
        }

        internal override void SetupRequest(SearchRequestParameters requestParameters)
        {
            base.SetupRequest(requestParameters);

            if (requestParameters.SearchType == SearchType.ItemSearch)
            {
            	_itemSearchRequest.Keywords = _searchParameters.Keywords;
            }

            _itemSearchRequest.SearchIndex = SearchIndexType.DigitalMusic.ToString();
            _itemSearchRequest.ResponseGroup = GetResponseGroups(requestParameters);
        }
    }
}
