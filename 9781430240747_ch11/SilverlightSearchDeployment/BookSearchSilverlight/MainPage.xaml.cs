using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.SharePoint.Client;

namespace BookSearchSilverlight
{
    public partial class MainPage : UserControl
    {
        private Web _web;
        private List _bookList;
        private ListItemCollection _items;
        private ClientContext _context = ClientContext.Current;

        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _web = _context.Web;
            _bookList = _web.Lists.GetByTitle("Books");
            string queryString = "<View><Query><Where><BeginsWith><FieldRef Name='Title'/><Value Type='Text'>" + txtSearch.Text + "</Value></BeginsWith></Where></Query></View>";
            CamlQuery query = new CamlQuery();
            query.ViewXml = queryString;
            _items = _bookList.GetItems(query);
            _context.Load(_items);
            _context.ExecuteQueryAsync(onQuerySuccess, onQueryFailure);
        }
            
        private void onQuerySuccess(object sender, ClientRequestSucceededEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(BindBookList));
        }

        private void onQueryFailure(object sender, ClientRequestFailedEventArgs e)
        {
            MessageBox.Show("Error:" + e.Message);
        }

        private void BindBookList()
        {
            List<string> books = new List<string>();

            foreach(ListItem item in _items)
            {
                books.Add(item["Title"].ToString());
            }

            lstSearchResults.ItemsSource = books;
        }
        
    }
}
