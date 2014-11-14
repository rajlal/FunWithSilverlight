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
using System.Windows.Browser;

namespace HTMLBridge
{
    public partial class PageAccessDOM : UserControl
    {
        public PageAccessDOM()
        {
            InitializeComponent();
        }

        private void GetId()
        {
            CollapseAll();
            CanvasId.Visibility = Visibility.Visible;

            HtmlElement elementSL = HtmlPage.Document.GetElementById("ContentText");

            if (elementSL != null)
                txtID.Text = elementSL.GetProperty("OuterHtml").ToString();
            else
                txtID.Text = "Element with id=ContentText not found";
        }
        private string GetNodeChildren(ScriptObjectCollection elemColl, System.Text.StringBuilder returnStr, Int32 depth)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();

            foreach (HtmlElement elem in elemColl)
            {
                string elemName;

                elemName = elem.GetAttribute("id");
                if (elemName == null || elemName.Length == 0)
                {
                    elemName = "<No id>";
                }
                else
                {
                    elemName = "<" + elemName + ">";
                }

                str.Append(' ', depth * 4);
                str.Append(elemName + ": " + elem.TagName);
                returnStr.AppendLine(str.ToString());

                try
                {
                    GetNodeChildren(elem.Children, returnStr, depth + 1);
                }
                catch (Exception)
                { }

                str.Remove(0, str.Length);
            }

            return (returnStr.ToString());
        }
        private void GetTreeCollection()
        {
            if (HtmlPage.Document != null)
            {
                ScriptObjectCollection elemColl = null;
                HtmlDocument doc = HtmlPage.Document;
                if (doc != null)
                {
                    elemColl = doc.GetElementsByTagName("HTML");
                    String str = GetNodeChildren(elemColl, new System.Text.StringBuilder(), 0);
                    txtHTML.Text = str;
                }
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            GetId();
        }
        private void showID(object sender, MouseButtonEventArgs e)
        {
            GetId();
            
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void showTree(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasTree.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            GetTreeCollection();
        }
        private void showTag(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasTag.Visibility = Visibility.Visible;

            ScriptObjectCollection DivTags = HtmlPage.Document.GetElementsByTagName("DIV");

            txtTag.Text += "  " +DivTags.Count + " DIV tags found in the HTML page\n\n"; 
            int i=1;
            foreach (HtmlElement div in DivTags)
            {

                string elemID = div.GetAttribute("id");
                txtTag.Text += "   " + i + "." + " Id=" + elemID + "\n";
                 i++;
            }
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CollapseAll()
        {
            CanvasId.Visibility = Visibility.Collapsed;
            CanvasTag.Visibility = Visibility.Collapsed;
            CanvasTree.Visibility = Visibility.Collapsed;
            txtID.Text = "";
            txtTag.Text = "";
            txtHTML.Text = "";
        }

    }
}
