// ***************************************************************
//  WebServiceResult   version:  1.0   Date: 01/10/2006
//  -------------------------------------------------------------
//  
//  -------------------------------------------------------------
//  Copyright © 2006 - Winista All Rights Reserved
// ***************************************************************
// 
// ***************************************************************
using System;
using System.Collections.Specialized;
using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
	/// <summary>
	/// Summary description for WebServiceResult.
	/// </summary>
	/// 
	[SerializableAttribute]
	public sealed class WebServiceResult
	{
		#region Private Members
		private Int32 m_iStatusCode = 0;
		private Int32 m_iTotalPages = -1;
		private Int32 m_iTotalResults = -1;
		private String m_strStatusMessage = "OK";
		private Item[] m_collItems;
        private StringCollection _amazonErrorCodes = new StringCollection();
        private StringCollection _amazonErrorMessages = new StringCollection();
		#endregion

		public WebServiceResult()
		{
		}

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public Int32 StatusCode
		{
			get
			{
				return this.m_iStatusCode;
			}
			set
			{
				this.m_iStatusCode = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String StatusMessage
		{
			get
			{
				return this.m_strStatusMessage;
			}
			set
			{
				this.m_strStatusMessage = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Item[] ResultItems
		{
			get
			{
				return this.m_collItems;
			}
			set
			{
				this.m_collItems = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Int32 TotalPages
		{
			get
			{
				return this.m_iTotalPages;
			}
			set
			{
				this.m_iTotalPages = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Int32 TotalResults
		{
			get
			{
				return this.m_iTotalResults;
			}
			set
			{
				this.m_iTotalResults = value;
			}
		}

	    public StringCollection AmazonErrorCodes
	    {
            get { return this._amazonErrorCodes; }
	    }

	    public StringCollection AmazonErrorMessages
	    {
            get { return this._amazonErrorMessages; }
	    }
		#endregion
	}
}
