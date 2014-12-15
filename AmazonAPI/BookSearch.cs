using System;
using System.Collections.Generic;
using System.Text;
using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
    public class BookSearch : AmazonSearch
    {
    	private BookSearchParameters _searchParameters;
		public List<Book> Search(SearchRequestParameters requestParameters)
		{
			
			_searchParameters = requestParameters as BookSearchParameters;
			
			if (null == _searchParameters)
			{
				throw new ArgumentException("Invalid request parameter object");
			}
			SetupRequest(requestParameters);

			WebServiceResult result = PerformSearch(requestParameters);

			if (result.StatusCode == 0)
			{
				// Convert the list of Amazon items to typed results.
				List<Book> books = new List<Book>();
				for(int i = 0; i < result.ResultItems.Length; i++)
				{
					books.Add(new Book(result.ResultItems[i]));
				}
				return books;
			}
			return null;
		}

		internal override void SetupRequest(SearchRequestParameters requestParameters)
		{
			base.SetupRequest(requestParameters);

			if (requestParameters.SearchType == SearchType.ItemSearch)
			{
				if (_searchParameters.TitleSearch)
				{
					_itemSearchRequest.Title = _searchParameters.Keywords;
				}
				else
				{
					_itemSearchRequest.Keywords = _searchParameters.Keywords;
				}
			}

			_itemSearchRequest.SearchIndex = SearchIndexType.Books.ToString();
			_itemSearchRequest.ResponseGroup = GetResponseGroups(requestParameters);
		}
	}
}
