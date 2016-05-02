<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InstaFollow.LicenseGen.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Your License</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family: Arial, Helvetica, sans-serif;font-size: 14px;text-align:center; width: 400px;margin: auto;text-align:center">
        <br/><br/><br/>
        <h2>Your BETA license key:</h2>
        <asp:TextBox runat="server" ID="tbLicense" Width="450"></asp:TextBox><br/><br/>
        <asp:Label runat="server" Text="Please paste this into your application."></asp:Label>
    </div>
    </form>
</body>
</html>
