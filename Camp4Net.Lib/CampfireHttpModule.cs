using System;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using Camp4Net.Message;
using log4net;

namespace Camp4Net
{
     
    public class CampfireHttpModule : IHttpModule
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

            if (HasAllConfigSettings())
            {
                _url = string.Format("https://{0}.campfirenow.com/room/{1}/", _site, _room);
                _log.InfoFormat("URL:{0}",_url);
                _mailService = new MailService(_apiKey, _url);
                application.Error += OnApplicationError;
            }
        }

        public void Dispose()
        {
            
        }

        private bool HasAllConfigSettings()
        {
            var hasAllConfigSettings = !string.IsNullOrEmpty(_apiKey) &&
                                       !string.IsNullOrEmpty(_site) &&
                                       !string.IsNullOrEmpty(_room);
                                       
            if (hasAllConfigSettings)
            {
                _log.Info("All AppSettings present");
                return true;
            }
            else
            {
                _log.ErrorFormat("Lacks AppSettings or invalid AppSetting values!");
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