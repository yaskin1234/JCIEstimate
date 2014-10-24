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
            if (!IsPostBack)
            {
                string report = Request.QueryString["report"];
                ReportViewer1.ServerReport.ReportPath = "/JCIEstimate/" + report;                
                ReportParameter rp = new ReportParameter("projectUid", Session["projectUid"].ToString());
                ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { rp });
                ReportViewer1.ShowExportControls = true;
                ReportViewer1.ShowParameterPrompts = true;            
            }
            
           
        }
    }
}