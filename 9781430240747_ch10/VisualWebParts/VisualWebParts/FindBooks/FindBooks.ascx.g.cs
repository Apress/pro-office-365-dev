﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4214
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VisualWebParts.FindBooks {
    using System.Web;
    using System.Text.RegularExpressions;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.Security;
    using Microsoft.SharePoint.Utilities;
    using System.Web.UI;
    using System;
    using Microsoft.SharePoint.WebControls;
    using System.Web.UI.WebControls;
    using System.Collections.Specialized;
    using Microsoft.SharePoint;
    using System.Collections;
    using System.Web.Profile;
    using System.Text;
    using System.Web.Caching;
    using System.Configuration;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.SessionState;
    using System.Web.UI.HtmlControls;
    
    
    public partial class FindBooks {
        
        protected global::System.Web.UI.WebControls.DropDownList ddlAuthor;
        
        protected global::System.Web.UI.WebControls.Button btnSearch;
        
        protected global::System.Web.UI.WebControls.GridView gvResults;
        
        public static implicit operator global::System.Web.UI.TemplateControl(FindBooks target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlAuthor() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlAuthor = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlAuthor";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnSearch() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnSearch = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnSearch";
            @__ctrl.Text = "Get Books";
            @__ctrl.Click -= new System.EventHandler(this.btnSearch_ClickLINQ);
            @__ctrl.Click += new System.EventHandler(this.btnSearch_ClickLINQ);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.GridView @__BuildControlgvResults() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.gvResults = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "gvResults";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::VisualWebParts.FindBooks.FindBooks @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n<p>\r\n    Filter Books By Author:\r\n    "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl1;
            @__ctrl1 = this.@__BuildControlddlAuthor();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    "));
            global::System.Web.UI.WebControls.Button @__ctrl2;
            @__ctrl2 = this.@__BuildControlbtnSearch();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n</p>\r\n\r\n\r\n<p>\r\n    Results:\r\n    "));
            global::System.Web.UI.WebControls.GridView @__ctrl3;
            @__ctrl3 = this.@__BuildControlgvResults();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n</p>"));
        }
        
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
