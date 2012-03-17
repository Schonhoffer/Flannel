<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Flannel._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Appropriate Temporary Flannel!
    </h2>
    <form method="post" action="/api/submit" enctype="multipart/form-data">
		<input type="file" name="filename"/>
		<input type="submit"/>
    </form>
	
	<form method="post" action="/api/voteup" >
		<input type="hidden" name="SubscriptionID" value="8f4e0f4b-12a1-4cfe-b95c-0dabaf263b0b"/>
		<input type="submit"/>
    </form>
</asp:Content>
