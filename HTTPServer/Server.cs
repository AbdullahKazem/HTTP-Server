using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Threading;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            IPEndPoint ipend = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipend);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket client = serverSocket.Accept();
                Console.WriteLine("new cleint accept : {0}" + client.RemoteEndPoint);
                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));
                newthread.Start(client);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket
            Socket cleintSocket = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            cleintSocket.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request

                    byte[] datarecieved = new byte[1024 * 1024];
                    int dataLength = cleintSocket.Receive(datarecieved);
                    string stringRequest = Encoding.ASCII.GetString(datarecieved, 0, dataLength);

                    // TODO: break the while loop if receivedLen==0

                    if (dataLength == 0)
                        break;

                    // TODO: Create a Request object using received request string

                    Request request = new Request(stringRequest);

                    // TODO: Call HandleRequest Method that returns the response

                    Response response = HandleRequest(request);

                    // TODO: Send Response back to client

                    byte[] dataSend = Encoding.ASCII.GetBytes(response.ResponseString);
                    cleintSocket.Send(dataSend);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            cleintSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            Response response;
            try
            {
                //TODO: check for bad request 

                bool badRequest = request.ParseRequest();
                if (badRequest == false)
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    response = new Response(StatusCode.BadRequest, "text/html", content, null, request.httpVersion);
                    return response;
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.

                string[] names = request.relativeURI.Split('/');
                string relativeUri = names[1];

                //TODO: check for redirect

                string redirctPage = GetRedirectionPagePathIFExist(relativeUri);
                if (redirctPage != string.Empty)
                {
                    string redirctionPath = Path.Combine(Configuration.RootPath, redirctPage);
                    content = LoadDefaultPage(redirctPage);
                    response = new Response(StatusCode.Redirect, "text/html", content, redirctionPath, request.httpVersion);
                    return response;
                }

                //TODO: check file exists

                content = LoadDefaultPage(relativeUri);
                if (content == string.Empty)
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    response = new Response(StatusCode.NotFound, "text/html", content, null, request.httpVersion);
                    return response;
                }
                else {
                    //TODO: read the physical file
                    // Create OK response
                    response = new Response(StatusCode.OK, "text/html", content, null, request.httpVersion);
                    return response;
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                response = new Response(StatusCode.InternalServerError, "text/html", content, null,request.httpVersion);
                return response;
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            if (!Configuration.RedirectionRules.ContainsKey(relativePath)){
                return string.Empty;
            }
            else {
                return Configuration.RedirectionRules[relativePath];
            }
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (!File.Exists(filePath)){
                return string.Empty;
            }
            else {
            // else read file and return its content
                return File.ReadAllText(filePath);
            }
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    string[] rules = line.Split(',');
                    Configuration.RedirectionRules.Add(rules[0], rules[1]);
                    //line = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
