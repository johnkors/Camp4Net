using System;
using System.IO;
using System.Reflection;
using log4net;

namespace TestWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            _log.Debug("Page_Load");
            _log.Error("ERROR in web app!");
            throw new UnauthorizedAccessException("This should be posted to your campfire room!");
        }

        protected void click(object sender, EventArgs e)
        {
            File.WriteAllBytes(@"D:\Projects\Camp4Net\TestWebApp\logs\test.txt", fu.FileBytes);
        }
    }
}