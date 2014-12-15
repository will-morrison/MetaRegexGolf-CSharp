using System;
using System.Collections.Generic;
using System.Text;

namespace ByteBlocks.AmazonAPI
{
	public abstract class SearchRequestParameters
	{
		#region Class Members

		private string _keywords;
		private int _maxResults = 10;
		private bool _getCustomerReviews = false;
		private SearchType _searchType = SearchType.ItemSearch;
		private string _asins;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public string Keywords
		{
			get { return _keywords; }
			set{ _keywords = value;}
		}

		/// <summary>
		/// Gets or sets list of asins to search.
		/// </summary>
		public string ASINs
		{
			get { return _asins; }
			set{ _asins = value;}
		}
		/// <summary>
		/// Gets or sets maximum number of results to get.
		/// </summary>
		public int MaxResults
		{
			get { return _maxResults; }
			set { _maxResults = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool GetCustomerReviews
		{
			get { return this._getCustomerReviews; }
			set { this._getCustomerReviews = value; }
		}

		public SearchType SearchType
		{
			get { return _searchType; }
			set{ _searchType = value;}
		}
		#endregion
	}
}
