using System;
using System.Collections.Generic;
using System.Text;
using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
    public abstract class AmazonSearch
	{
    	internal static int PAGE_SIZE = 10;

		#region Class Members
		private String m_strAssociateTag = String.Empty;
		private String m_strAccessKeyId = String.Empty;

		protected AWSECommerceService _ws = new AWSECommerceService();
		protected ItemSearch _itemSearch = new ItemSearch();
		protected ItemLookup _itemLookup = new ItemLookup();
		protected ItemSearchRequest _itemSearchRequest = new ItemSearchRequest();
		protected ItemLookupRequest _itemLookupRequest = new ItemLookupRequest();
		#endregion

		#region Properties
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
		#endregion

		internal virtual void SetupRequest(SearchRequestParameters requestParameters)
		{
			_itemSearch.SubscriptionId = this.AccessKeyId;
			_itemSearch.AssociateTag = this.AssociateTag;

			if (requestParameters.SearchType == SearchType.ItemLookupByAsin)
			{
				_itemLookupRequest.IdType = ItemLookupRequestIdType.ASIN;
				_itemLookupRequest.ItemId = requestParameters.ASINs.Split(",".ToCharArray());
				_itemLookupRequest.IdTypeSpecified = true;
			}
		}

		internal virtual WebServiceResult PerformSearch(SearchRequestParameters requestParameters)
		{
			WebServiceResult obResult = new WebServiceResult();
			int pageIdx = 1;
			int totalResultsToGet = requestParameters.MaxResults;
			int pagesToGet = (int)Math.Ceiling(totalResultsToGet / (double)AmazonSearch.PAGE_SIZE);
			do
			{
				ItemSearchResponse resp = null;
				try
				{
					_itemSearchRequest.ItemPage = pageIdx.ToString();
					_itemSearch.Request = new ItemSearchRequest[1] { _itemSearchRequest };
					resp = _ws.ItemSearch(_itemSearch);
					if (resp == null)
					{
						obResult.StatusMessage = "Server Error";
						obResult.StatusCode = 500;
						break;
					}

					if (AmazonSearch.IsError(resp, obResult))
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

		internal virtual string[] GetResponseGroups(SearchRequestParameters requestParameters)
		{
			List<string> grpList = new List<string>();
			grpList.Add("Small");
			grpList.Add("Images");
			grpList.Add("ItemAttributes");
			grpList.Add("OfferFull");
			grpList.Add("Offers");

			if (requestParameters.GetCustomerReviews)
			{
				grpList.Add("Reviews");
			}

			return grpList.ToArray();
		}

		#region Helper Methods
		internal static bool IsError(ItemSearchResponse resp, WebServiceResult obResult)
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
