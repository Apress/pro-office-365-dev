using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Linq;
using Microsoft.Lync;
using Microsoft.Lync.Controls;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Extensibility;
using System.Windows.Interop;

namespace LyncApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private delegate void FocusWindow();
        private delegate void ResizeWindow(Size newSize);
        private WindowInteropHelper _windowHelper;
        private Int32 _handle;
        private ConversationWindow _conversationWindow;

        public Window1()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            //set interop values
            _windowHelper = new WindowInteropHelper(this);
            _handle = _windowHelper.Handle.ToInt32();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BuildContactList();
            LoadContacts();
            LoadCustomContacts();
        }

        private List<Contact> _contacts = new List<Contact>();

        protected void BuildContactList()
        {
            // Build collection of valid contacts -- using Contact class
            _contacts.Add(new Contact() 
            {name = "Mark Collins", sipAddress = "sip:markc@apress365.com"});
            _contacts.Add(new Contact() 
            {name = "Michael Mayberry", sipAddress = "sip:michaelm@apress365.com"});
            _contacts.Add(new Contact() 
            {name = "Corbin Collins", sipAddress = "sip:corbinc@apress365.com"});
            _contacts.Add(new Contact() 
            {name = "Paul Michaels", sipAddress = "sip:paulm@apress365.com"});
            _contacts.Add(new Contact() 
            {name = "Jonathan Hassel", sipAddress = "sip:jonathanh@apress365.com"});
            _contacts.Add(new Contact() 
            {name = "Martina Grom", sipAddress = "sip:martinag@apress365.com"});
        }

        protected void LoadContacts()
        {
            // Bind collection to combo box
            cboContacts.ItemsSource = _contacts;
            cboContacts.DisplayMemberPath = "name";
            cboContacts.SelectedValuePath = "sipAddress";
        }

        private void cboContacts_SelectionChanged
            (object sender, SelectionChangedEventArgs e)
        {
            // Set the sipAddress of the selected item as the 
            // source for the presence indicator
            lyncPresence.Source = cboContacts.SelectedValue;

            // Set the start instant message button
            lyncStartMessage.Source = cboContacts.SelectedValue;

            // Setup the contextual information
            ConversationContextualInfo info = new ConversationContextualInfo();
            info.ApplicationId = "{F03A7197-5E52-49FF-B28C-3ADA7636D02F}";
            info.ApplicationData = "\"Can you see this?\" ";
            info.Subject = "Hello " + ((Contact)cboContacts.SelectedItem).name +
            ", take a look at this program!";

            lyncStartMessage.ContextualInformation = info;
        }

        protected void LoadCustomContacts()
        {
            lyncCustomList.ItemsSource = (from C in _contacts select C.sipAddress);
        }

        private void btnConversationStart_Click(object sender, RoutedEventArgs e)
        {
            // Create an Automation object.
            Automation automation = LyncClient.GetAutomation();

            List<string> participants = new List<string>();
            participants.Add(cboContacts.SelectedValue.ToString());

            // Declare instance of Dictionary to pass the conversation context data.
            Dictionary<AutomationModalitySettings, object> automationSettings = new Dictionary<AutomationModalitySettings, object>();

            // Provide Conversation context: First IM Message.
            automationSettings.Add(AutomationModalitySettings.FirstInstantMessage, "Hello!");

            // Provide Conversation context: Send first IM message immediately after the conversation starts.
            automationSettings.Add(AutomationModalitySettings.SendFirstInstantMessageImmediately, true);

            // Start the conversation.
            IAsyncResult beginconversation = automation.BeginStartConversation(AutomationModalities.InstantMessage, participants
            , automationSettings
            , onConversationStart
            , automation);
        }

        private void onConversationStart(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                //get conversation window
                _conversationWindow = ((Automation)result.AsyncState).EndStartConversation(result);
                
                //wire up events
                _conversationWindow.NeedsSizeChange += _conversationWindow_NeedsSizeChange;
                _conversationWindow.NeedsAttention += _conversationWindow_NeedsAttention;
   
                //dock conversation window
                _conversationWindow.Dock(formHost.Handle);
            }
        }

        private void _conversationWindow_NeedsAttention(object sender, ConversationWindowNeedsAttentionEventArgs e)
        {
            FocusWindow focusWindow = new FocusWindow(GetWindowFocus);
            Dispatcher.Invoke(focusWindow, new object[] { });
        }

        private void _conversationWindow_NeedsSizeChange(object sender, ConversationWindowNeedsSizeChangeEventArgs e)
        {
            Size windowSize = new Size();
            windowSize.Height = e.RecommendedWindowHeight;
            windowSize.Width = e.RecommendedWindowWidth;
            ResizeWindow resize = new ResizeWindow(SetWindowSize);
            Dispatcher.Invoke(resize, new object[] { windowSize });
        }

        private void SetWindowSize(Size newSize)
        {
            formPanel.Size = new System.Drawing.Size(
            (int)newSize.Width, (int)newSize.Height);
        }

        private void GetWindowFocus()
        {
            Focus();
        }
    }
}