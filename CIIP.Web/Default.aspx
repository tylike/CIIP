<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
    <style type="text/css">
        .dxm-item.accountItem.dxm-subMenu .dx-vam
        {
            padding-left: 10px;
        }
        .dxm-item.accountItem.dxm-subMenu .dxm-image.dx-vam
        {
            border-radius: 32px;
            -moz-border-radius: 32px;
            -webkit-border-radius: 32px;
            padding-right: 0px !important;
            padding-left: 0px !important;
            max-height: 32px;
            max-width: 32px;
        }
    </style>
</head>
<body class="VerticalTemplate">
    <form id="form2" runat="server">
    <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
    <div runat="server" id="Content" />
    </form>
</body>
</html>
