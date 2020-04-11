<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterQuestions.aspx.cs" Inherits="SistemiProvimeveOnline.Account.RegisterQuestions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h2>Forma per regjistrim te pyetjeve</h2>
    <asp:Table runat="server">
        <asp:TableRow><asp:TableCell>Numri pyetjes * </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="idPyetjes" AutoCompleteType="None" ></asp:TextBox></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell>Emri fajllit * </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="emriFajllit" AutoCompleteType="None"></asp:TextBox></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell>Pyetja * </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="pyetja" AutoCompleteType="None"></asp:TextBox></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell>Parametrat </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="parametrat" AutoCompleteType="None"></asp:TextBox></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell>Rezultati pritur * </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="rezultati" AutoCompleteType="None"></asp:TextBox></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell>Pre Code * </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="preCode" TextMode="MultiLine" AutoCompleteType="None" Width="500" Height="250"></asp:TextBox></asp:TableCell></asp:TableRow>
        <asp:TableRow><asp:TableCell>Post Code * </asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="postCode" TextMode="MultiLine" AutoCompleteType="None" Width="500" Height="250"></asp:TextBox></asp:TableCell></asp:TableRow>
    </asp:Table>
    <br />
    <asp:Label runat="server" ForeColor="#ff3535" Text="Fushat e shenuara me * jane obligative" Visible="false" ID="errorMsg"></asp:Label>
    <asp:Label runat="server" ForeColor="#008414" Text="Regjistrimi u krye!" Visible="false" ID="successMsg"></asp:Label>
    <br />
    <asp:Button runat="server" Text="Regjistro" ID="register" OnClick="register_Click"/>
</asp:Content>
