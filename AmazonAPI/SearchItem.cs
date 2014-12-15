using System;
using System.Collections.Generic;
using System.Text;
using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
    public abstract class SearchItem
    {
        #region Class Members
        private string _asin;
        private string _title;
        private string _smallImage;
        private string _mediumImage;
        private string _largeImage;
        private string _salesRank;
        private string _detailsUrl;
        private string _listPrice;
        private string _listPriceAmount;
		private decimal _averageRating;
        #endregion
        internal SearchItem(Item item)
        {
            _asin = item.ASIN;
            _salesRank = item.SalesRank;
            _smallImage = item.SmallImage.URL;
            _mediumImage = item.MediumImage.URL;
            _largeImage = item.LargeImage.URL;
            _detailsUrl = item.DetailPageURL;

            if (null != item.ItemAttributes)
            {
                ItemAttributes attribs = item.ItemAttributes;
                _title = attribs.Title;
				if (null != attribs.ListPrice)
				{
					_listPrice = attribs.ListPrice.FormattedPrice;
					_listPriceAmount = attribs.ListPrice.Amount;
				}
            }
			if (null != item.CustomerReviews)
			{
				_averageRating = item.CustomerReviews.AverageRating;
			}
        }

        #region Properties
        public string ASIN
        {
            get { return _asin; }
        }
        /// <summary>
        /// Gets sales rank.
        /// </summary>
        public string SalesRank
        {
            get { return _salesRank; }
        }

        /// <summary>
        /// Gets url for small image
        /// </summary>
        public string SmallImage
        {
            get { return _smallImage; }
        }

        /// <summary>
        /// Gets url for medium image
        /// </summary>
        public string MediumImage
        {
            get { return _mediumImage; }
        }

        /// <summary>
        /// Gets url for large image
        /// </summary>
        public string LargeImage
        {
            get { return _largeImage; }
        }

        /// <summary>
        /// Gets URL for details of this item.
        /// </summary>
        public string DetailsUrl
        {
            get { return _detailsUrl; }
        }

        public string ListPrice
        {
            get { return _listPrice; }
        }

    	public decimal AverageRating
    	{
			get { return _averageRating; }
    	}
        #endregion
    }
}
