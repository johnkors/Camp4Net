using System;
using System.IO;
using System.Net;
using System.Text;
using Camp4Net.Message;

namespace Camp4Net
{
    internal class Mailman
    {
        private ICredentials _credentials;
        private string _url;

        internal Mailman(string apiKey, string url)
        {
            _url = url;
            _credentials = new NetworkCredential(apiKey, "X");
        }

        internal string PostSound(MessageSound soundType)
        {
            var campfireMessage = new SoundMessage(soundType);
            return Post(campfireMessage);
        }

        internal string PostText(string message)
        {
            var campfireMessage = new TextMessage(message);
            return Post(campfireMessage);
        }

        internal string PostPaste(string message)
        {
            var campfireMessage = new PasteMessage(message);
            return Post(campfireMessage);
        }

        private string Post(CampfireMessage campfireMessage)
        {
            var data = Encoding.UTF8.GetBytes(campfireMessage.ToString());
            WebRequest request = WebRequest.Create(_url + "speak.json");
            request.Method = "POST";
            request.Credentials = _credentials;
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(data, 0, data.Length);
            }
            try
            {
                var response = request.GetResponse();
                string responseString = string.Empty;
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var sr = new StreamReader(stream);
                        responseString = sr.ReadToEnd();
                    }
                }
                return responseString;
            }
            catch (Exception e)
            {
                return "Campfire.MailmanError: " + e.Message;
            }
        }
    }
}