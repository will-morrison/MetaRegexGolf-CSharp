using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Categorize.Words;

namespace Categorize.IO
{
    static class FileHelper
    {
        public static string GetStringFromFile(string filePath)
        {
            //create a stream reader
            StreamReader bookStream;
            //string of entire book
            string fullBook = string.Empty;
            //stream to file
            bookStream = File.OpenText(filePath);
            //read stream to string
            fullBook = bookStream.ReadToEnd();
            //close stream
            bookStream.Close();
            return fullBook;
        }

        public static void CreateReport(string outputFilePath, WordCollection collection)
        {
            //create stream writer
            StreamWriter streamWrite;
            //create output file
            streamWrite = File.AppendText(outputFilePath);
            //for each word+num in frequrncyHash
            foreach (string word in collection.Words.Keys)
            {
                //Write  to file
                streamWrite.WriteLine(string.Format("{0}: {1}", word, collection.Words[word]));
            }
            streamWrite.Close();
        }
    }
}
