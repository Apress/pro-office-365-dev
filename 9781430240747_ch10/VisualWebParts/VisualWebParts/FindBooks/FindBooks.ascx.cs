using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint.Linq;
using System.Linq;
using booklist;
using System.Collections.Generic;

namespace VisualWebParts.FindBooks
{
    [ToolboxItem(false)]
    public partial class FindBooks : System.Web.UI.WebControls.WebParts.WebPart
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateAuthorListLINQ();
            }
        }

        protected void PopulateAuthorList()
        {
            SPList bookList = SPContext.Current.Web.Lists["Books"];

            SPQuery query = new SPQuery();
            query.ViewFieldsOnly = true;
            query.ViewFields = "<FieldRef Name='BookAuthor' />";
            query.RowLimit = 20;

            ddlAuthor.DataSource = bookList.GetItems(query).GetDataTable();
            ddlAuthor.DataTextField = "BookAuthor";
            ddlAuthor.DataValueField = "BookAuthor";
            ddlAuthor.DataBind();
        }

        protected void PopulateAuthorListLINQ()
        {
            BooklistDataContext context = new BooklistDataContext(SPContext.Current.Web.Url);
            
            

            var authorList = (from B in context.Books
                              select B.Author);

            foreach (string author in authorList)
            {
                ddlAuthor.Items.Add(author);
            }

            //ddlAuthor.DataSource = authorList;
            //ddlAuthor.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SPList bookList = SPContext.Current.Web.Lists["Books"];

            SPQuery query = new SPQuery();
            query.ViewFieldsOnly = true;
            query.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='BookAuthor' />";
            query.Query = "<Where><Eq><FieldRef Name='BookAuthor'/><Value Type='Text'>" + ddlAuthor.SelectedItem.Text + "</Value></Eq></Where>";
            query.RowLimit = 20;

            gvResults.DataSource = bookList.GetItems(query).GetDataTable();
            gvResults.DataBind();
        }

        protected void btnSearch_ClickLINQ(object sender, EventArgs e)
        {
            BooklistDataContext context = new BooklistDataContext(SPContext.Current.Web.Url);

            List<BookView> bookList = (from B in context.Books
                                       where B.Author == ddlAuthor.SelectedValue
                                       select new BookView{ Title = B.Title, Author = B.Author }).ToList();
            
            gvResults.DataSource = bookList;
            gvResults.DataBind();
        }
    }
}
