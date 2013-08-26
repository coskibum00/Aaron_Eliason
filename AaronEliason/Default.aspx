<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="false" EnableViewState="false"
    CodeBehind="Default.aspx.cs" Inherits="AaronEliason.UI.Default" %>
<%@ Register Src="~/Panels/NumericSpiral.ascx" TagPrefix="ae" TagName="NumericSpiral" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" type="text/css" href="Styles/AaronEliason.css" /> 
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ae:NumericSpiral ID="NumericSpiral" runat="server" />
</asp:Content>
