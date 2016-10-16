using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Microsoft.SharePoint;
using Microsoft.SharePoint.UserCode;
using System.Globalization;
using System.Threading;

namespace CustomWorkflowActions
{
    public class ProcessNewBookAction
    {
        public Hashtable ProcessNewBook(SPUserCodeWorkflowContext context)
        {
            Hashtable response = new Hashtable();

            try
            {
                using (SPSite site = new SPSite(context.CurrentWebUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        
                        SPList bookList = web.Lists[context.ListId];
                        SPListItem currentBook = bookList.GetItemById(context.ItemId);
                        //proper case title and author
                        CultureInfo culture = CultureInfo.CurrentCulture;
                        TextInfo textInfo = culture.TextInfo;

                        currentBook["Title"] = textInfo.ToTitleCase(currentBook["Title"].ToString().ToLower());

                        string authorBefore = currentBook["BookAuthor"].ToString().ToLower();
                        string authorAfter = textInfo.ToTitleCase(authorBefore);

                        currentBook["BookAuthor"] = textInfo.ToTitleCase(currentBook["BookAuthor"].ToString().ToLower());

                        currentBook.Update();
                        
                        response["result"] = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                response["result"] = "error: " + ex.Message;
            }

            return response;
        }
    }
}
