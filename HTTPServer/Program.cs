using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            
            //Start server
            // 1) Make server object on port 1000
            string redirectionMatrixPath = CreateRedirectionRulesFile();
            int port = 1000;
            Server server = new Server(port, redirectionMatrixPath);
            // 2) Start Server
            Console.WriteLine("Started...");
            server.StartServer();
        }

        static string CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            string path = Environment.CurrentDirectory + "\\redirectionRules.txt";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string line = "aboutus.html,aboutus2.html";
            sw.WriteLine(line);
            sw.Close();
            fs.Close();
            return path;
        }
         
    }
}
