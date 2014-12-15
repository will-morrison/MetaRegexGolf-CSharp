// ***************************************************************
//  WebServiceClient   version:  1.0   Date: 01/10/2006
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright © 2006 - Winista All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;
using System.Collections.Generic;

using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
	/// <summary>
	/// Summary description for WebServiceClient.
	/// </summary>
	public sealed class WebServiceClient
	{
        private AWSECommerceService m_obAmazonService = new AWSECommerceService();
		private ItemSearch m_obItemSearch = new ItemSearch();
		private ItemLookup _oItemLookup = new ItemLookup();
		private ItemSearchRequest m_obItemSearchRequest = new ItemSearchRequest();
		private ItemSearchResponse m_obItemSearchResponse;
		private ItemLookupRequest _oItemLookupRequest = new ItemLookupRequest();
		private ItemLookupResponse _oItemLookupResponse;

		private string _apiVersion = "2009-01-06";
		private string _lookupIds = null;
		private String m_strAssociateTag = String.Empty;
		private String m_strAccessKeyId = String.Empty;
		private SearchIndexType m_SearchType = SearchIndexType.Books;
		private String m_strKeywords = String.Empty;
		private String m_strTitleSearch = String.Empty;
		private String m_strArtistSearch = String.Empty;
        private bool _getEditorialReviews = false;
        private bool _getCustomerReviews = false;
		private SearchType _searchType = SearchType.ItemSearch;
		private Int32 m_iPage = 1;
        private int _maxResults = 10;
	    private static int PAGE_SIZE = 10;

		/// <summary>
		/// 
		/// </summary>
		public WebServiceClient()
		{
		}

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public string APIVersion
		{
			get { return this._apiVersion; }
			set { this._apiVersion = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public String AssociateTag
		{
			get
			{
				return this.m_strAssociateTag;
			}
			set
			{
				this.m_strAssociateTag = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String AccessKeyId
		{
			get
			{
				return this.m_strAccessKeyId;
			}
			set
			{
				this.m_strAccessKeyId = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SearchIndexType SearchIndexType
		{
			get
			{
				return this.m_SearchType;
			}
			set
			{
				this.m_SearchType = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String Keywords
		{
			get
			{
				return this.m_strKeywords;
			}
			set
			{
				this.m_strKeywords = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LookupIds
		{
			get { return this._lookupIds; }
			set { this._lookupIds = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public String TitleSearch
		{
			get
			{
				return this.m_strTitleSearch;
			}
			set
			{
				this.m_strTitleSearch = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String ArtistSearch
		{
			get
			{
				return this.m_strArtistSearch;
			}
			set
			{
				this.m_strArtistSearch = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Int32 PageToFetch
		{
			get
			{
				return this.m_iPage;
			}
			set
			{
				if (value <=0 || value > 400)
				{
					throw new ArgumentOutOfRangeException("PageToFetch", "1-400", "Page value should be between 1 and 400");
				}
				this.m_iPage = value;
			}
		}

        /// <summary>
        /// Gets or sets maximum number of results to get.
        /// </summary>
	    public int MaxResults
	    {
            get { return _maxResults; }
            set{ _maxResults = value;}
	    }

        /// <summary>
        ///
        /// </summary>
        public bool GetEditorialReviews
        {
            get { return this._getEditorialReviews; }
            set { this._getEditorialReviews = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool GetCustomerReviews
        {
            get { return this._getCustomerReviews; }
            set { this._getCustomerReviews = value; }
        }

		/// <summary>
		/// 
		/// </summary>
		public SearchType SearchType
		{
			get { return this._searchType; }
			set { this._searchType = value; }
		}
        
		#endregion

		#region Internal Methods
        public WebServiceResult SendItemLookupRequestToAmazon()
        {
            WebServiceResult obResult = new WebServiceResult();
			_oItemLookup.SubscriptionId = this.AccessKeyId;
			_oItemLookup.AssociateTag = this.AssociateTag;
			// Set request parameters.
			_oItemLookupRequest.IdType = ItemLookupRequestIdType.ASIN;
			_oItemLookupRequest.ItemId = this.LookupIds.Split(",".ToCharArray());
			_oItemLookupRequest.IdTypeSpecified = true;
			_oItemLookupRequest.ResponseGroup = GetResponseGroups();
			_oItemLookup.Request = new ItemLookupRequest[1] { _oItemLookupRequest };

			//send the query
			try
			{
				_oItemLookupResponse = m_obAmazonService.ItemLookup(_oItemLookup);
			}
			catch (Exception e)
			{
				obResult.StatusMessage = e.Message;
				obResult.StatusCode = 500;
				return obResult;
			}

			// Check for errors in the response
			if (_oItemLookupResponse == null)
			{
				obResult.StatusMessage = "Server Error";
				obResult.StatusCode = 500;
				return obResult;
			}

			Items[] itemsResponse = _oItemLookupResponse.Items;

			if (itemsResponse[0].Request.Errors != null)
			{
				obResult.StatusMessage = itemsResponse[0].Request.Errors[0].Message;
				obResult.StatusCode = 500;
				return obResult;
			}

			obResult.StatusCode = 0;
			obResult.ResultItems = itemsResponse[0].Item;
			try
			{
				obResult.TotalPages = Convert.ToInt32(itemsResponse[0].TotalPages);
			}
			catch { }

			try
			{
				obResult.TotalResults = Convert.ToInt32(itemsResponse[0].TotalResults);
			}
			catch { }

			return obResult;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public WebServiceResult SendQueryToAmazon()
		{
			WebServiceResult obResult = new WebServiceResult();

			//build request to Amazon
			m_obItemSearch.SubscriptionId = this.AccessKeyId;
			m_obItemSearch.AssociateTag = this.AssociateTag;
			m_obItemSearchRequest.Keywords = this.Keywords;
			m_obItemSearchRequest.SearchIndex = this.SearchIndexType.ToString();
            m_obItemSearchRequest.ResponseGroup = GetResponseGroups();

		    int pageIdx = 1;
		    int totalResultsToGet = this.MaxResults;
            int pagesToGet = (int)Math.Ceiling(totalResultsToGet / (double)PAGE_SIZE);
            do
            {
                ItemSearchResponse resp = null;
                try
                {
                    m_obItemSearchRequest.ItemPage = pageIdx.ToString();
                    m_obItemSearch.Request = new ItemSearchRequest[1] { m_obItemSearchRequest };
                    resp = m_obAmazonService.ItemSearch(m_obItemSearch);
                    if (resp == null)
                    {
                        obResult.StatusMessage = "Server Error";
				        obResult.StatusCode = 500;
                        break;
                    }

                    if (IsError(resp, obResult))
                    {
                        break;
                    }

                    Items[] itemsResponse = resp.Items;

                    if (obResult.TotalResults == 0)
                    {
                        break;
                    }

                    if (pageIdx == 1 && obResult.TotalResults != 0)
                    {
                        try
                        {
                            obResult.TotalPages = Convert.ToInt32(itemsResponse[0].TotalPages);
                        }
                        catch { }

                        try
                        {
                            obResult.TotalResults = Convert.ToInt32(itemsResponse[0].TotalResults);
                        }
                        catch { }
                        totalResultsToGet = Math.Min(totalResultsToGet, obResult.TotalResults);
                        obResult.ResultItems = new Item[totalResultsToGet];
                    }
                    Array.Copy(itemsResponse[0].Item, 0, obResult.ResultItems, ((pageIdx - 1) * PAGE_SIZE), Math.Min(itemsResponse[0].Item.Length, totalResultsToGet));
                    totalResultsToGet -= itemsResponse[0].Item.Length;
                    pageIdx++;
                    pagesToGet--;
                }
                catch (Exception e)
                {
                    obResult.StatusMessage = e.Message;
                    obResult.StatusCode = 500;
                    break;
                }
            }
            while (pagesToGet != 0);
			

			return obResult;
		}

        private string[] GetResponseGroups()
        {
            List<string> grpList = new List<string>();
            grpList.Add("Small");
            grpList.Add("Images");
            grpList.Add("ItemAttributes");
            grpList.Add("OfferFull");
			grpList.Add("Offers");
            if (this.GetEditorialReviews)
            {
                //grpList.Add("EditorialReview");
            }
            if (this.GetCustomerReviews)
            {
                grpList.Add("Reviews");
            }

            return grpList.ToArray();
        }
		#endregion

        #region Helper Methods
        private bool IsError(ItemSearchResponse resp, WebServiceResult obResult)
        {
            bool isError = false;
            isError = (resp.Items[0].Request.Errors != null && resp.Items[0].Request.Errors.Length > 0);
            if (isError)
            {
                for (int i = 0; i < resp.Items[0].Request.Errors.Length; i++)
                {
                    obResult.AmazonErrorCodes.Add(resp.Items[0].Request.Errors[i].Code);
                    obResult.AmazonErrorMessages.Add(resp.Items[0].Request.Errors[i].Message);
                }
            }

            return isError;
        }
        #endregion
    }
}
