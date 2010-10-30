using System;
using System.Web;
using System.Web.Configuration;
using Camp4Net.Message;

namespace Camp4Net
{
    public class CampfireHttpModule : IHttpModule
    {
        protected string _apiKey;
        protected string _room;
        protected string _site;
        protected string _url;
        internal MailService _mailService;
        protected HttpApplication _application;

        public void Init(HttpApplication application)
        {
            _application = application;
            _apiKey = WebConfigurationManager.AppSettings["Campfire.UserApiKey"];
            _site = WebConfigurationManager.AppSettings["Campfire.Site"];
            _room = WebConfigurationManager.AppSettings["Campfire.Room"];
            _url = string.Format("http://{0}.campfirenow.com/room/{1}/", _site, _room);
            _mailService = new MailService(_apiKey, _url);
            if (HasValidConfigSettings())
            {
                application.Error += OnApplicationError;
            }
        }

        public void Dispose()
        {
            
        }

        private bool HasValidConfigSettings()
        {
            var hasAllConfigSettings = _apiKey != null && _site != null && _room != null;
            if (hasAllConfigSettings)
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
            OnApplicationErrorWrapper(new HttpContextWrapper(_application.Context), _mailService);
        }

        public void OnApplicationErrorWrapper(HttpContextBase httpContextWrapper, IMailService mailService)
        {
            string usr = httpContextWrapper.User != null ? httpContextWrapper.User.Identity.Name : "Unknown user";
            mailService.PostSound(MessageSound.trombone);
            mailService.PostText(string.Format("User {0} experienced error!", usr));
            mailService.PostText(string.Format("Exception: {0}", httpContextWrapper.Server.GetLastError().Message));
            mailService.PostText(string.Format("URL: {0}", httpContextWrapper.Request.Url));
            mailService.PostPaste(httpContextWrapper.Server.GetLastError().ToString());
        }

    }
}