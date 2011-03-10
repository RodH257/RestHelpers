using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace RestHelpers
{

    /// <summary>
    /// Handles HTTP communication to server.
    /// </summary>
    public class RestfulCommunicator
    {
        //Using a subdomain on some spare webhosting
        private string _baseUrl;

        public RestfulCommunicator(string baseUrl)
        {
            _baseUrl = baseUrl;
        }


        /// <summary>
        /// Syntactic Sugar to call post request with default credentials
        /// </summary>
        public string NewPostRequest(string method, IDictionary<string, string> keyValues)
        {
            return NewPostRequest(method, keyValues, null);
        }


        /// <summary>
        /// Sends a post request to the sever
        /// </summary>
        /// <returns></returns>
        public string NewPostRequest(string method, IDictionary<string, string> keyValues, ICredentials credentials)
        {
            Uri address = new Uri(_baseUrl + method);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (credentials != null)
                request.Credentials = credentials;

            StringBuilder data = new StringBuilder();

            bool first = true;
            foreach (string key in keyValues.Keys)
            {
                if (!first)
                    data.Append("&");
                data.Append(key + "=" + HttpUtility.UrlEncode(keyValues[key]));
                first = false;
            }

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output  
                return reader.ReadToEnd();
            }
        }


        /// <summary>
        /// Syntactic sugar for new get request with default credentials
        /// </summary>
        public string NewGetRequest(string method, IDictionary<string, string> keyValues)
        {
            return NewGetRequest(method, keyValues, null);
        }

        /// <summary>
        /// Sends a get request to the server
        /// </summary>
        public string NewGetRequest(string method, IDictionary<string, string> keyValues, ICredentials credentials)
        {
            StringBuilder url = new StringBuilder();
            url.Append(_baseUrl);
            url.Append(method);
            url.Append("?");

            bool first = true;
            foreach (string key in keyValues.Keys)
            {
                if (!first)
                    url.Append("&");
                url.Append(key);
                url.Append("=");
                url.Append(keyValues[key]);
                first = false;
            }

            HttpWebRequest request = WebRequest.Create(url.ToString()) as HttpWebRequest;

            if (credentials != null)
                request.Credentials = credentials;

            // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output  
                return reader.ReadToEnd();
            }
        }
    }


}
