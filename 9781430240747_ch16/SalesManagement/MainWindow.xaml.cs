using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Linq;

using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Lync.Model;
using Microsoft.Lync.Controls;
using Microsoft.Lync.Model.Extensibility;
using Microsoft.Lync.Model.Conversation;

namespace SalesManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContext _context = new DataContext();
        private ExchangeService _service = GetBinding();
        private ConversationWindow _conversationWindow;

        private List<SalesAgent> _agents;
        private int _leadCount = 0;
        private int _collectCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //load agents
            _agents = _context.GetAgents();

            //bind agents to List Box with presense
            BindAgents();

            //Connect to Exchange and check for items in Inbox
            _service = GetBinding();

            _collectCount = _service.FindItems(WellKnownFolderName.Inbox, new ItemView(10)).TotalCount;

            UpdateCollectCount();
            UpdateLeadCount();
        }

        protected void BindAgents()
        {
            lstAgents.ItemsSource = _agents;
            txtAvailableAgents.Text = (from a in _agents where a.onlineStatus == "Available" select a).Count().ToString();
        }

        static ExchangeService GetBinding()
        {
            // Create the binding.
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

            // Define credentials.
            service.Credentials = new WebCredentials("websales@apress365.com", "@press365");

            // Use the AutodiscoverUrl method to locate the service endpoint.
            try
            {
                //service.Url = new Uri("https://ch1prd0402.outlook.com/EWS/Exchange.asmx");
                service.AutodiscoverUrl("websales@apress365.com", RedirectionUrlValidationCallback);
            }
            catch (AutodiscoverRemoteException ex)
            {
                MessageBox.Show("Autodiscover error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return service;
        }

        static bool RedirectionUrlValidationCallback(String redirectionUrl)
        {
            // Perform validation.
            // Validation is developer dependent to ensure a safe redirect.
            return true;
        }

        protected void UpdateCollectCount()
        {
            txtLeadsCollected.Text = _collectCount.ToString();
        }

        protected void UpdateLeadCount()
        {
            txtLeadsAssigned.Text = _leadCount.ToString();
        }

        private void btnCurrentItems_Click(object sender, RoutedEventArgs e)
        {
            ProcessMailItems();
        }

        private void btnNewItems_Click(object sender, RoutedEventArgs e)
        {
            SetStreamingNotifications();
        }

        protected void ProcessMailItems()
        {
            //Get mailbox items
            ItemView itemView = new ItemView(10);
            itemView.PropertySet = PropertySet.IdOnly;

            FindItemsResults<Item> mailItems = _service.FindItems(WellKnownFolderName.Inbox, itemView);
            PropertySet emailProperties = new PropertySet(
                                                EmailMessageSchema.Sender,
                                                EmailMessageSchema.DateTimeReceived,
                                                EmailMessageSchema.Body,
                                                EmailMessageSchema.Subject);
            emailProperties.RequestedBodyType = BodyType.Text;
            if (mailItems.TotalCount > 0)
                _service.LoadPropertiesForItems(mailItems.Items, emailProperties);

            List<ItemId> emailItemIDs = new List<ItemId>();

            //process each mailbox item
            foreach (Item mailItem in mailItems)
            {
                if (mailItem is EmailMessage)
                {
                    //find or create customer in sharepoint database
                    EmailMessage email = (EmailMessage)mailItem;
                    Customer customer = _context.GetCustomerByEmail(email.Sender.Address);
                    if (customer == null)
                    {
                        Customer newCustomer = new Customer()
                        {
                            name = email.Sender.Name,
                            email = email.Sender.Address,
                            status = "Prospect"
                        };

                        customer = _context.CreateNewCustomer(newCustomer);
                    }

                    //create contact record from email
                    Contact newContact = new Contact()
                    {
                        customerID = customer.customerID,
                        contactDate = email.DateTimeReceived.ToShortDateString(),
                        contactType = "Email",
                        contactNotes = "Subject: " + email.Subject + "\n\n" + email.Body.Text
                    };

                    Contact savedContact = _context.CreatContact(newContact);

                    //assign agent
                    SalesAgent assignedAgent = (from A in _agents
                                                where A.onlineStatus == "Available"
                                                orderby A.lastMessage descending
                                                select A).FirstOrDefault();

                    if (assignedAgent != null)
                    {
                        //start lync conversation with contactid
                        BeginConversation(assignedAgent, savedContact);

                        //update lead count
                        _leadCount++;

                        //display updated lead count
                        Dispatcher.BeginInvoke(new Action(UpdateLeadCount));
                    }

                    //add item to list of items to get moved to Archive folder
                    emailItemIDs.Add(email.Id);
                }
            }

            if (emailItemIDs.Count > 0)
            {
                //move emails to archive folder
                //create search filter to find specific folder
                SearchFilter filter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "Archive");

                //use Exchange Web Service to search for folder
                Folder folder = _service.FindFolders(WellKnownFolderName.Inbox, filter, new FolderView(1)).Folders[0];

                //move email items to found folder
                _service.MoveItems(emailItemIDs, folder.Id);
            }
        }

        protected void BeginConversation(SalesAgent agent, Contact contact)
        {
            // Conversation participant list.
            List<string> participantList = new List<string>();
            participantList.Add(agent.Uri);

            Dictionary<AutomationModalitySettings, object> conversationContextData = new Dictionary<AutomationModalitySettings, object>();

            // initial IM message
            conversationContextData.Add(AutomationModalitySettings.FirstInstantMessage, "Apress Remodeling Application Context");

            // send initial IM immediately
            conversationContextData.Add(AutomationModalitySettings.SendFirstInstantMessageImmediately, true);

            // set application ID
            conversationContextData.Add(AutomationModalitySettings.ApplicationId, "{A07EE104-A0C2-4E84-ABB3-BBC370A37636}");

            string appData = "ContactID=" + contact.contactID + "|CustomerID=" + contact.customerID;

            // set application data
            conversationContextData.Add(AutomationModalitySettings.ApplicationData, appData);

            Automation auto = LyncClient.GetAutomation();

            // start the conversation.
            IAsyncResult beginconversation = auto.BeginStartConversation(
            AutomationModalities.InstantMessage
            , participantList
            , conversationContextData
            , null
            , null);
        }

       

        // notify the automation object and conversationWindow
        // that the conversation started.
        private void BeginConversationCallBack(IAsyncResult ar)
        {
            Automation _automation = ar.AsyncState as Automation;
            _conversationWindow = _automation.EndStartConversation(ar);
        }

        protected void SetStreamingNotifications()
        {
            // Subscribe to streaming notifications on the Inbox folder, and listen 
            // for "NewMail", "Created", and "Deleted" events. 
            StreamingSubscription streamingsubscription = _service.SubscribeToStreamingNotifications(
                new FolderId[] { WellKnownFolderName.Inbox },
                EventType.NewMail);

            StreamingSubscriptionConnection connection = new StreamingSubscriptionConnection(_service, 5);

            connection.AddSubscription(streamingsubscription);
            // Delegate event handlers. 
            connection.OnNotificationEvent += OnNewItemEvent;

            connection.Open();
        }

        protected void OnNewItemEvent(object sender, NotificationEventArgs args)
        {
            StreamingSubscription subscription = args.Subscription;

            // Loop through all item-related events. 
            foreach (NotificationEvent notification in args.Events)
            {
                object[] emptyArgs = new object[0];

                switch (notification.EventType)
                {
                    case EventType.NewMail:
                        _collectCount++;
                        Dispatcher.BeginInvoke(new Action(UpdateCollectCount));
                        ProcessMailItems();
                        break;
                }
            }
        }
    }
}
