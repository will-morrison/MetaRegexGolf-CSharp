using System;
using System.Collections.Generic;
using System.Text;

namespace ByteBlocks.AmazonAPI
{
	public class DVDSearchParameters : SearchRequestParameters
	{
		#region Class Members

		private string _title;
		private string _director;
		private string _actor;
		#endregion

		#region Properties
		public string Title
		{
			get { return _title; }
			set{ _title = value;}
		}

		public string Director
		{
			get{ return _director;}
			set{ _director = value;}
		}

		public string Actor
		{
			get{ return _actor;}
			set{ _actor = value;}
		}
		#endregion
	}
}
