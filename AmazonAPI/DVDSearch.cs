using System;
using System.Collections.Generic;
using System.Text;

namespace ByteBlocks.AmazonAPI
{
    public class DVDSearch : AmazonSearch
    {
    	private DVDSearchParameters _searchParameters;
		public List<DVD> Search(SearchRequestParameters requestParameters)
		{
			_searchParameters = requestParameters as DVDSearchParameters;

			if (null == _searchParameters)
			{
				throw new ArgumentException("Invalid request parameter object");
			}
			SetupRequest(requestParameters);

			WebServiceResult result = PerformSearch(requestParameters);

			if (result.StatusCode == 0)
			{
				// Convert the list of Amazon items to typed results.
				List<DVD> dvds = new List<DVD>();
				for (int i = 0; i < result.ResultItems.Length; i++)
				{
					dvds.Add(new DVD(result.ResultItems[i]));
				}
				return dvds;
			}
			return null;
		}

		internal override void SetupRequest(SearchRequestParameters requestParameters)
		{
			base.SetupRequest(requestParameters);

			if (requestParameters.SearchType == SearchType.ItemSearch)
			{
				if (!string.IsNullOrEmpty(_searchParameters.Actor))
				{
					_itemSearchRequest.Actor = _searchParameters.Actor;
				}
				if (!string.IsNullOrEmpty(_searchParameters.Director))
				{
					_itemSearchRequest.Director = _searchParameters.Director;
				}
				if (!string.IsNullOrEmpty(_searchParameters.Title))
				{
					_itemSearchRequest.Actor = _searchParameters.Title;
				}
			}

			_itemSearchRequest.SearchIndex = SearchIndexType.DVD.ToString();
			_itemSearchRequest.ResponseGroup = GetResponseGroups(requestParameters);
		}
	}
}
