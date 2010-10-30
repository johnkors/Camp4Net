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
            Log(campfireMessage);
            byte[] campfireMessageBytes = ConvertToBytes(campfireMessage);
            WebRequest request = CreateNewWebRequest(campfireMessageBytes);
            WriteDataToRequest(campfireMessageBytes, request);
            try
            {
                return GetCampfireWebResponse(request);
            }
            catch (Exception e)
            {
                return HandleError(e);
            }
        }

        private void Log(CampfireMessage campfireMessage)
        {
           
            if (campfireMessage is PasteMessage)
            {
                StringReader strReader = new StringReader(campfireMessage.ToString());
                var firstLine = strReader.ReadLine();
                var logString = string.Format("Posting message {0}", firstLine);
                _log.Debug(logString);
            }
            else
            {
                var logString = string.Format("Posting message {0}", campfireMessage);
                _log.Info(logString);
            }
        }

        private byte[] ConvertToBytes(CampfireMessage campfireMessage)
        {
            return Encoding.UTF8.GetBytes(campfireMessage.ToString());
        }

        private string GetCampfireWebResponse(WebRequest request)
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

        private void WriteDataToRequest(byte[] data, WebRequest request)
        {
            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(data, 0, data.Length);
            }
        }

        private WebRequest CreateNewWebRequest(byte[] data)
        {
            WebRequest request = WebRequest.Create(_url + "speak.json");
            request.Method = "POST";
            request.Credentials = _credentials;
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            return request;
        }

        private string HandleError(Exception e)
        {
            _log.ErrorFormat("Exception {0}", e);
            return "Camp4Net.MailmanError: " + e.Message;
        }
    }
}