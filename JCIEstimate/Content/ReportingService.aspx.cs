﻿using System;
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
                //if (Session["projectUid"] != null)
                //{
                    string report = Request.QueryString["report"];
                    ReportViewer1.ServerReport.ReportPath = "/JCIEstimate/" + report;
                    bool isFound = false;
                    foreach (var item in ReportViewer1.ServerReport.GetParameters())
                    {
                        if (item.Name == "projectUid")
                        {
                            isFound = true;
                            break;
                        }
                    }

                    if (isFound)
                    {
                        ReportParameter rp = new ReportParameter("projectUid", Session["projectUid"].ToString());
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { rp });                    
                    }
                    
                    ReportViewer1.ShowParameterPrompts = true;
                    string userUid = Request.QueryString["userUid"];

                    if (report == "EquipmentExport")
                    {
                        string sortType = Session["sortType"].ToString();
                        string sortValue = Session["sortValue"].ToString();
                        ReportParameter sType = new ReportParameter("sortType", sortType);
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { sType });
                        ReportParameter sValue = new ReportParameter("sortValue", sortValue);
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { sValue });
                        //ReportParameter format = new ReportParameter("rs:Format", "excel");
                        //ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { format });
                        ReportViewer1.ShowExportControls = true;
                        ReportViewer1.ShowParameterPrompts = false;
                        ReportViewer1.ServerReport.Timeout = 60000;
                    }
                    //else if (report == "ScopeOfWorkECM")
                    //{
                    //    ReportParameter format = new ReportParameter("rs:Format", "word");
                    //    ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { format });
                    //}

                    if (userUid != null) 
                    {
                        ReportParameter cu = new ReportParameter("userUid", userUid);
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { cu });
                        ReportViewer1.ShowExportControls = true;
                        ReportViewer1.ShowParameterPrompts = true;
                    }
                    string contractorUid = Request.QueryString["contractorUid"];
                    if (contractorUid != null)
                    {
                        ReportParameter cu = new ReportParameter("contractorUid", contractorUid);
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { cu });
                        ReportViewer1.ShowExportControls = true;
                        ReportViewer1.ShowParameterPrompts = true;
                    }
                //}                                
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