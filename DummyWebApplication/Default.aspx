<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DummyWebApplication.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal runat="server" ID="ltrResponse"></asp:Literal>
            <asp:Button runat="server" ID="btnRunException" Text="Generate an exception!" />
        </div>
    </form>
</body>
</html>
