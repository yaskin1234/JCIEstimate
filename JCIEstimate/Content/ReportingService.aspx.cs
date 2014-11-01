using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;

namespace JCIEstimate.Content
{
    public partial class ReportingService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["projectUid"] != null)
                {
                    string report = Request.QueryString["report"];
                    if (!Request.IsLocal)
                    {
                        IReportServerCredentials myCred = new CustomReportCredentials("yaskin_reportserver", "7UtYru4Uv", "LOTUS");
                        ReportViewer1.ServerReport.ReportServerCredentials = myCred;
                        ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://lotus.arvixe.com/ReportServer_SQL/");
                        ReportViewer1.ServerReport.ReportPath = "/yaskin/JCIEstimate/" + report;
                    }
                    else
                    {
                        ReportViewer1.ServerReport.ReportPath = "/JCIEstimate/" + report;
                    }
                    ReportParameter rp = new ReportParameter("projectUid", Session["projectUid"].ToString());
                    ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { rp });
                    ReportViewer1.ShowExportControls = true;
                    ReportViewer1.ShowParameterPrompts = true;            
                    string contractorUid = Request.QueryString["contractorUid"];
                    if (contractorUid != null) 
                    {
                        ReportParameter cu = new ReportParameter("userUid", Session["userUid"].ToString());
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { cu });
                        ReportViewer1.ShowExportControls = true;
                        ReportViewer1.ShowParameterPrompts = true;            

                    }

                }                
            }
        }
    }

    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {

        // local variable for network credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {

                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {

            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }

    }

}