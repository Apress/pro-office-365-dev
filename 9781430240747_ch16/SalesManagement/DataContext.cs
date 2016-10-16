using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

using MSDN.Samples.ClaimsAuth;

namespace SalesManagement
{
    //Data Context for methods getting and storing data
    public class DataContext
    {
        private ClientContext _clientContext = ClaimClientContext.GetAuthenticatedContext("https://apress365e.sharepoint.com/teams/dev/Tracking");

        /// <summary>
        /// Returns list of SalesAgents from Sharepoint
        /// </summary>
        /// <returns></returns>
        public List<SalesAgent> GetAgents()
        {
            List<SalesAgent> agents = new List<SalesAgent>();

            try
            {
                foreach (ListItem listItem in GetListItems("Agent", ""))
                {
                    SalesAgent newAgent = new SalesAgent()
                    {
                        displayName = listItem["AgentName"].ToString(),
                        email = listItem["SipAddress"].ToString(),
                        login = listItem["Login"].ToString(),
                        lastMessage = (listItem["LastMessage"] ?? string.Empty).ToString(),
                        onlineStatus = listItem["AgentStatus"].ToString()
                    };

                    agents.Add(newAgent);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return agents;
        }

        /// <summary>
        /// Returns Customer from Sharepoint based on Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Customer GetCustomerByEmail(string email)
        {
            string query = "<View><Query><Where><Contains><FieldRef Name='EmailAddress'/><Value Type='Text'>" + email + "</Value></Contains></Where></Query></View>";
            ListItemCollection listItems = GetListItems("Customer", query);

            //return first found
            if (listItems.Count > 0)
            {
                ListItem foundItem = listItems[0];
                Customer newCustomer = new Customer()
                {
                    customerID = foundItem["ID"].ToString(),
                    name = foundItem["CustomerName"].ToString(),
                    email = foundItem["EmailAddress"].ToString(),
                    status = foundItem["CustomerStatus"].ToString()
                };

                return newCustomer;
            }

            return null;
        }

        public Customer CreateNewCustomer(Customer newCustomer)
        {
            List list = _clientContext.Web.Lists.GetByTitle("Customer");

            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();

            ListItem listItem = list.AddItem(itemCreateInfo);
            listItem["CustomerName"] = newCustomer.name;
            listItem["EmailAddress"] = newCustomer.email;
            listItem["CustomerStatus"] = newCustomer.status;

            listItem.Update();

            _clientContext.ExecuteQuery();

            newCustomer.customerID = listItem.Id.ToString();

            return newCustomer;
        }

        private ListItemCollection GetListItems(string listName, string query)
        {
            CamlQuery q = new CamlQuery();
            q.ViewXml = query; //no query needed

            ListItemCollection listItems = _clientContext.Web.Lists.GetByTitle(listName).GetItems(q);
            _clientContext.Load(listItems);
            _clientContext.ExecuteQuery();

            return listItems;
        }

        public Contact CreatContact(Contact newContact)
        {
            List list = _clientContext.Web.Lists.GetByTitle("Contact");


            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem listItem = list.AddItem(itemCreateInfo);
            listItem["Customer"] = newContact.customerID;
            listItem["ContactDate"] = newContact.contactDate;
            listItem["ContactType"] = newContact.contactType;
            listItem["ContactNotes"] = newContact.contactNotes;


            listItem.Update();
            _clientContext.Load(listItem);
            _clientContext.ExecuteQuery();

            newContact.contactID = listItem.Id.ToString();

            return newContact;
        }
    }



    public class Customer
    {
        public string customerID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }

    public class Contact
    {
        public string contactID { get; set; }
        public string customerID { get; set; }
        public string contactDate { get; set; }
        public string contactType { get; set; }
        public string contactNotes { get; set; }
    }

    //Data Types
    public class SalesAgent
    {
        public string login { get; set; }
        public string lastMessage { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string onlineStatus { get; set; }
        public string Uri
        {
            get
            {
                return email;
            }
        }

        public string displayName
        {
            get;

            set;
        }
    }
}
