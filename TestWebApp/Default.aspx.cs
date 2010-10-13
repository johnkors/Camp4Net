using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace TestWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            _log.Error("ERROR in web app!");
            throw new UnauthorizedAccessException("This should be posted to your campfire room!");
        }
    }
}