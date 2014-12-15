namespace ByteBlocks.AmazonAPI
{
	/// <summary>
	/// 
	/// </summary>
	public enum SearchType
	{
		ItemSearch = 0,
		ItemLookupByAsin = 1
	}

	/// <summary>
	/// 
	/// </summary>
	public enum ReviewSortType
	{
		/// <summary>
		/// Most helpful reviews first
		/// </summary>
		HelpfulVotesFirst = 0,
		/// <summary>
		/// Most helpful review last
		/// </summary>
		HelpfulVotesLast = 1,
		/// <summary>
		/// Best overall rating first
		/// </summary>
		OverallRatingFirst = 2,
		/// <summary>
		/// Best overall rating last
		/// </summary>
		OverallRaingLast = 3,
		/// <summary>
		/// Most recent reviews first
		/// </summary>
		SubmissionDateFirst = 4,
		/// <summary>
		/// Most recent reviews last
		/// </summary>
		SubmissionDateLast = 5
	}
}