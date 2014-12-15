using System;
using System.Web.UI;
using System.ComponentModel;

namespace Winista.Web.UI.Controls.AmazonWeb
{
	/// <summary>
	/// 
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ReviewAttributes
	{
		#region Class Members
		private ReviewSortType _sortType;
		private short _maxReviews = 5;
		private int _maxReviewsTextLength = 100;
		#endregion

		#region Public Properties
		/// <summary>
		/// 
		/// </summary>
		public ReviewSortType SortType
		{
			get { return this._sortType; }
			set { this._sortType = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public short MaxReviews
		{
			get { return this._maxReviews; }
			set { this._maxReviews = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int MaxReviewLength
		{
			get { return this._maxReviewsTextLength; }
			set { this._maxReviewsTextLength = value; }
		}
		#endregion
	}
}
