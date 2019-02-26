using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace File_Search
{
    class Search
    {
        static Mutex mutex;

        public Search()
        {
            mutex = new Mutex();
        }
  
        public static IEnumerable<String> GetFiles(String path, String pattern)
        {

            return Directory.EnumerateFiles(path, pattern).Union(Directory.EnumerateDirectories(path).SelectMany(a =>
            {

                try
                {
                    return GetFiles(a, pattern);
                }
                catch (UnauthorizedAccessException)
                {
                    return Enumerable.Empty<String>();
                }
            }
            ));


        }

        public static void DisplayText(IEnumerable<String> Enum)
        {

            try
            {
                mutex.WaitOne();

                foreach (String s in Enum)
                {
                    MessageBox.Show(s);
                }

                mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

       }
    }
}
