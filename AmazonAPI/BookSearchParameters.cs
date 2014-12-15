using System;
using System.Collections.Generic;
using System.Text;

namespace ByteBlocks.AmazonAPI
{
	public class BookSearchParameters : SearchRequestParameters
	{
		#region Class Members

		private bool _titleSearch;
		#endregion

		#region Properties
		public bool TitleSearch
		{
			get { return _titleSearch; }
			set{ _titleSearch = value;}
		}

		#endregion
	}
}
