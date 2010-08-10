using System;
using System.Configuration;
using System.Web;
using Camp4Net.Message;

namespace Camp4Net
{
    public class CampfireHttpModule : IHttpModule
    {
        private string _apiKey;
        private string _room;
        private string _site;
        private string _url;
        private Mailman _mailman;
        private HttpApplication _application;

        public void Init(HttpApplication application)
        {
            _application = application;
            _apiKey = ConfigurationManager.AppSettings["Campfire.UserApiKey"];
            _site = ConfigurationManager.AppSettings["Campfire.Site"];
            _room = ConfigurationManager.AppSettings["Campfire.Room"];
            _url = string.Format("http://{0}.campfirenow.com/room/{1}/", _site, _room);
            _mailman = new Mailman(_apiKey, _url);
            if (HasValidConfigSettings())
            {
                application.Error += OnApplicationError;
            }
        }

        private bool HasValidConfigSettings()
        {
            var isNotNull = _apiKey != null && _site != null && _room != null;
            if (isNotNull)
            {
                Uri uri;
                return Uri.TryCreate(_url, UriKind.Absolute, out uri);
            }
            else
            {
                return false;
            }
        }

        private void OnApplicationError(object sender, EventArgs e)
        {
            string usr = _application.Context.User != null ? _application.Context.User.Identity.Name : "Unknown";
            _mailman.PostSound(MessageSound.trombone);
            _mailman.PostText(string.Format("Bruker {0} opplevde feil i kontrollstasjonen!", usr));
            _mailman.PostText(string.Format("Feil: {0}", _application.Server.GetLastError().Message));
            _mailman.PostText(string.Format("URL: {0}", _application.Request.Url));
            _mailman.PostPaste(_application.Server.GetLastError().ToString());
        }

        public void Dispose()
        {

        }
    }
}