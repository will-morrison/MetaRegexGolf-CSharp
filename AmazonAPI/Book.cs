using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using ByteBlocks.AmazonAPI.Amazon;

namespace ByteBlocks.AmazonAPI
{
    public class Book : SearchItem
    {
        #region Class Members

    	private string _title;
        private string _isbn;
        private string[] _authors;
        #endregion
        internal Book(Item item) : base(item)
        {
            if (null != item.ItemAttributes)
            {
            	_title = item.ItemAttributes.Title;
                _isbn = item.ItemAttributes.ISBN;
                _authors = new string[item.ItemAttributes.Author.Length];
                Array.Copy(item.ItemAttributes.Author, _authors, item.ItemAttributes.Author.Length);
            }
            
        }

        #region Properties
    	public string Title
    	{
			get { return _title; }
    	}
        public string ISBN
        {
            get { return _isbn; }
        }

        public string [] Authors
        {
            get { return _authors; }
        }
        #endregion
    }
}
