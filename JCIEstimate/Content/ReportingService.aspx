<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportingService.aspx.cs" Inherits="JCIEstimate.Content.ReportingService" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote" WaitMessageFont-Names="Verdana" 
            Height="100%" Width="100%"
            WaitMessageFont-Size="14pt">
            <ServerReport ReportPath="/JCIEstimate/ContractorTotals" />
        </rsweb:ReportViewer>
    </form>
</body>
</html>
