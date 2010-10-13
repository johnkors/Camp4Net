using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Camp4Net.Message;
using log4net;

namespace Camp4Net
{
    public class MailService : IMailService
    {
        private ICredentials _credentials;
        private string _url;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MailService(string apiKey, string url)
        {
            _url = url;
            _credentials = new NetworkCredential(apiKey, "X");
        }

        public virtual string PostSound(MessageSound soundType)
        {
            var campfireMessage = new SoundMessage(soundType);
            return Post(campfireMessage);
        }

        public virtual string PostText(string message)
        {
            var campfireMessage = new TextMessage(message);
            return Post(campfireMessage);
        }

        public virtual string PostPaste(string message)
        {
            var campfireMessage = new PasteMessage(message);
            return Post(campfireMessage);
        }

        private string Post(CampfireMessage campfireMessage)
        {
            _log.InfoFormat("Posting message {0}", campfireMessage);
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
                _log.ErrorFormat("Exception {0}", e);
                return "Camp4Net.MailmanError: " + e.Message;
            }
        }
    }
}