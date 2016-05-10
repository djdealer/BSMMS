<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BSMMS.LicenseGen.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Your License</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div style="font-family: Arial, Helvetica, sans-serif;font-size: 14px;text-align:center; width: 600px;margin: auto;text-align:center">
            <br/><br/><br/>
            <asp:Label runat="server" Text="BETA version is <b>FREE</b> at the moment. But I would appreciate a donation."></asp:Label>
            <br/><br/>
            <input type="hidden" name="cmd" value="_s-xclick" />
            <input type="hidden" name="hosted_button_id" value="5PK2ZPQ5WWZJE" />
            <asp:ImageButton 
                ID="ImageButton1" 
                AlternateText="PayPal - The safer, easier way to pay online!" 
                runat="server" 
                ImageUrl="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" 
                PostBackUrl="https://www.paypal.com/cgi-bin/webscr" 
                OnClick="OnClickBtnDonate"/>
            <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1"/>
            <br/><br/><br/>
            <h2>Please provide your data:</h2>
            <label>First name:</label><br/>
            <asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox><br/><br/>
        
            <label>Last name:</label><br/>
            <asp:TextBox runat="server" ID="tbLastName"></asp:TextBox><br/><br/>
        
            <label>Paypal Email:</label><br/>
            <asp:TextBox runat="server" ID="tbEmail"></asp:TextBox><br/><br/>
        
            <asp:Button runat="server" ID="btnGetLicense" OnClick="OnClickBtnGetLicense" Text="Get Licence" />
            <h2>Your BETA license key:</h2>
            <asp:TextBox runat="server" ID="tbLicense" Width="450"></asp:TextBox><br/><br/>
            <asp:Label runat="server" Text="Please paste this into your application."></asp:Label>
            <br/><br/><br/>
            <asp:Label runat="server" Text="BETA version is <b>FREE</b> at the moment. But I would appreciate a donation."></asp:Label>
            <br/><br/>
            <input type="hidden" name="cmd" value="_s-xclick" />
            <input type="hidden" name="hosted_button_id" value="5PK2ZPQ5WWZJE" />
            <asp:ImageButton 
                ID="btnDonate" 
                AlternateText="PayPal - The safer, easier way to pay online!" 
                runat="server" 
                ImageUrl="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" 
                PostBackUrl="https://www.paypal.com/cgi-bin/webscr" 
                OnClick="OnClickBtnDonate"/>
            <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1"/>
        </div>
    </form>
</body>
</html>
