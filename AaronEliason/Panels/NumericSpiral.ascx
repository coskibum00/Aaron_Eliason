<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" CodeBehind="NumericSpiral.ascx.cs" Inherits="AaronEliason.UI.NumericSpiral" %>

<asp:Label ID="Instructions" runat="server" CssClass="instructions" Text="Please enter a positive whole number to see it displayed in a spiral format" />
<br />
<br />
<asp:Label ID="IntegerInputLabel" runat="server" Text="Enter Integer"></asp:Label>
<asp:TextBox ID="IntegerInput" runat="server" Text=""></asp:TextBox>
<asp:Button ID="CreateOutput" runat="server" Text="Build Spiral" OnClick="onCreateOutput_Clicked" />


<div id="Output" runat="server" class="numeric-spiral">
    <div id="OutputResults" runat="server" class="result" visible="false"></div>
    <div id="ValidationErrors" runat="server" class="error" visible="false"></div>
</div>