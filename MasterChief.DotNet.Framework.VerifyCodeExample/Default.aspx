<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MasterChief.DotNet.Framework.VerifyCodeExample.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <img alt="看不清，换一张" src="CreateVerifyCode.aspx?style=type1" onclick="this.src='CreateVerifyCode.aspx?style=type1&ver='+Math.random()" /><br />

            <img alt="看不清，换一张" src="CreateVerifyCode.aspx?style=type10" onclick="this.src='CreateVerifyCode.aspx?style=type10?ver='+Math.random()" /><br />
            <%--托管管道模式设置经典--%>
        </div>
    </form>
</body>
</html>