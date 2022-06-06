using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath, string HTTPVersion)
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])

            responseString = GetStatusLine(code, HTTPVersion);
            if (string.IsNullOrEmpty(redirectoinPath))
            {
                responseString += "\n" + "Content_Type:" + contentType +
                                  "\n" + "Content_Length:" + content.Length +
                                  "\n" + "Date:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +
                                  "\n" + "\n" + content;
            }
            else {
                responseString += "\n" + "Content_Type:" + contentType +
                                  "\n" + "Content_Length:" + content.Length +
                                  "\n" + "Date" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +
                                  "\n" + "Location:" + redirectoinPath +
                                  "\n" + "\n" + content;
            }
            // TODO: Create the request string

        }

        private string GetStatusLine(StatusCode code, string HTTPVersion)
        {
            // TODO: Create the response status line and return it
            string statusLine = "";
            if (code == StatusCode.OK)
            {
                statusLine = HTTPVersion + " " + code + " " + "Ok";
            }
            else if (code == StatusCode.BadRequest)
            {
                statusLine = HTTPVersion + " " + code + " " + "Bad Request";
            }
            else if (code == StatusCode.NotFound)
            {
                statusLine = HTTPVersion + " " + code + " " + "Not Found";
            }
            else if (code == StatusCode.Redirect)
            {
                statusLine = HTTPVersion + " " + code + " " + "Moved Permanently";
            }
            else {
                statusLine = HTTPVersion + " " + code + " " + "Internal Server Error";
            }
            return statusLine;
        }
    }
}
