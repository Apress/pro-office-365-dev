using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Events.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("c1df9b55-8ca3-42d5-8d60-f27f0a2768ca")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (SPWeb web = (SPWeb)properties.Feature.Parent)
            {
                SPList bookList = web.Lists["Books"];

                SPListItem book1 = bookList.Items.Add();
                book1["Title"] = "The Invisble Man";
                book1["ISBN"] = "1450517935";
                book1["BookAuthor"] = "H.G. Wells";
                book1["Rating"] = "7";
                book1.Update();

                SPListItem book2 = bookList.Items.Add();
                book2["Title"] = "David Copperfield";
                book2["ISBN"] = "0679783415";
                book2["BookAuthor"] = "Charles Dickens";
                book2["Rating"] = "6";
                book2.Update();

                SPListItem book3 = bookList.Items.Add();
                book3["Title"] = "The Adventures of Sherlock Holmes";
                book3["ISBN"] = "978-0486474915";
                book3["BookAuthor"] = "Sir Arthur Conan Doyle";
                book3["Rating"] = "9";
                book3.Update();

                SPListItem book4 = bookList.Items.Add();
                book4["Title"] = "The Count of Monte Cristo";
                book4["ISBN"] = "978-1613820971";
                book4["BookAuthor"] = "Alexander Dumas";
                book4["Rating"] = "7";
                book4.Update();

                SPListItem book5 = bookList.Items.Add();
                book5["Title"] = "The Jungle Book";
                book5["ISBN"] = "978-0553211993";
                book5["BookAuthor"] = "Rudyard Kipling";
                book5["Rating"] = "6";
                book5.Update();

            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
