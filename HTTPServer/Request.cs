using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        string method;
        public string relativeURI;
        Dictionary<string, string> headerLines = new Dictionary<string, string>();

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        public string httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   

            string[] stringSeparators = new string[] { "\r\n" };
             requestLines = requestString.Split(stringSeparators, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            if (requestLines.Length <= 2) {
                return false;
            }

            // Parse Request line

            if (ParseRequestLine() == false) {
                return false;
            }

            // Validate blank line exists

            if (ValidateBlankLine() == false) {
                return false;
            }

            // Load header lines into HeaderLines dictionary
            int i = 1;
            while (!string.IsNullOrEmpty(requestLines[i])) {
                string[] headers = requestLines[i].Split(':');
                headerLines.Add(headers[0], headers[1]);
                i++;
            }

            return true;

        }

        private bool ParseRequestLine()
        {
            string[] names = requestLines[0].Split(' ');
            if (names.Length == 3)
            {
                method = names[0];
                relativeURI = names[1];
                httpVersion = names[2];
                return true;
            }
            else {
                return false;
            }
            
            //throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            if (!string.IsNullOrEmpty(requestLines[requestLines.Length - 1]))
            {
                return false;
            }
            else {
                return true;
            }
            //throw new NotImplementedException();
        }

    }
}
