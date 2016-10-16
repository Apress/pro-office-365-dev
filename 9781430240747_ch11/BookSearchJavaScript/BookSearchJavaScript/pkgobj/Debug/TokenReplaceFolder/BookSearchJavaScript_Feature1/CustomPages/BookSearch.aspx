<%@ Page Language="C#" masterpagefile="~masterurl/default.master" %>

<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">

<SharePoint:ScriptLink ID="scriptLink1" runat="server" />

<script language="javascript" type="text/javascript">
    var items;
    
    function searchList()
    {
        //get client context
        var context = new SP.ClientContext.get_current();

        //get the current site
        var bookList = context.get_web().get_lists().getByTitle('Books');

        var queryString = "<View><Query><Where><BeginsWith><FieldRef Name='Title'/><Value Type='Text'>" + document.getElementById('txtSearchQuery').value + "</Value></BeginsWith></Where></Query></View>";
        var camlQuery = new SP.CamlQuery();
        camlQuery.set_viewXml(queryString);

        items = bookList.getItems(camlQuery);
        context.load(items);

        context.executeQueryAsync(
                Function.createDelegate(this, this.onSuccess),
                Function.createDelegate(this, this.onFailure));
    }

    function onSuccess(sender, args) {
        //show items
        var resultString = '';

        var itemsList = items.getEnumerator();
        while (itemsList.moveNext())
        {
            var item = itemsList.get_current();
            resultString += item.get_item('Title') + '<br />';
        }

        document.getElementById('result').innerHTML = resultString;
    }

    function onFailure(sender, args){
        alert('Error!');
    }

</script>

<div>
<label>Search:</label><input type="text" id="txtSearchQuery" />
<input type="button" id="btnSearch" value="Search" onclick="searchList()" />
<br />
<br />
<br />
Results:<br />
<div id="result"></div>
</div>
</asp:Content>
