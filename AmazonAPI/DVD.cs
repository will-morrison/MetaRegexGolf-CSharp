using System;
using System.Collections.Generic;
using System.Text;
using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
	public class DVD : SearchItem
	{
		#region Class Members

		private string [] _actor;
		private string [] _director;
		private string _title;
		#endregion

		internal DVD(Item item) : base(item)
		{
			if (item.ItemAttributes != null)
			{
				ItemAttributes attrib = item.ItemAttributes;
				_actor = attrib.Actor;
				_director = attrib.Director;
				_title = attrib.Title;
			}
		}

		#region Properties
		public string [] Actor
		{
			get { return _actor; }
		}

		public string [] Director
		{
			get { return _director; }
		}

		public string Title
		{
			get { return _title; }
		}
		#endregion
	}
}
