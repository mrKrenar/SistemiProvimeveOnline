<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExamPage.aspx.cs" Inherits="SistemiProvimeveOnline.Account.ExamPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:Label runat="server" ID="detyra">Titulli detyres</asp:Label>
    <asp:TextBox ID="implementBox" runat="server" TextMode="MultiLine" Wrap="true" AutoCompleteType="None" style="width: 100%; height:300px"></asp:TextBox>
    <br />
    <asp:Button ID="Compile" runat="server" Text="Compile" AutoPostBack = "false" OnClick="Compile_Click"/>
    <asp:Button ID="Submit" runat="server" Text="Submit" AutoPostBack = "false" OnClick="Submit_Click" />
    <br />
    <asp:Label ID="errorMsg" runat="server" style="color:brown" Visible="false"></asp:Label>
    <asp:Label ID="compSuccessMsg" runat="server" style="color:olivedrab" Visible="False" Text="Kodi u kompajllua me sukses!"></asp:Label>
</asp:Content>
