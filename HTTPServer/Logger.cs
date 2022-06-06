using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            DateTime localDateNow = DateTime.Now;
            sr.WriteLine("DateTime is " + localDateNow.ToString());
            sr.WriteLine("Message is" + ex.Message);
            sr.Close();
        }
    }
}
