using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace JCIEstimate.Content
{
    public partial class ReportingService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter rp = new ReportParameter("ecmUid", Session["ecmUid"].ToString());
            ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { rp });
        }
    }
}