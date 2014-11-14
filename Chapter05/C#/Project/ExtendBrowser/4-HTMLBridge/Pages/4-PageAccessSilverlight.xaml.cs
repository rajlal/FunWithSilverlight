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
    public partial class PageAccessSilverlight : UserControl
    {
        public PageAccessSilverlight()
        {
            InitializeComponent();
            // Create and register a scriptable object.
            HtmlPage.RegisterScriptableObject("ScriptableMember", new ScriptableMember());
            HtmlPage.RegisterScriptableObject("ScriptableClass", new ScriptableClass());
        }
        public class ScriptableMember
        {
            [ScriptableMember]
            public bool IsPalindrome(string s)
            {
                int length = s.Length;
                char[] chrArray = s.ToCharArray();
                if (length == 0)
                    return false;
                if (length == 1)
                    return true;

                int start = 0;
                int end = length - 1;

                while (end > start)
                {
                    if (chrArray[start] == chrArray[end])
                    {
                        start++;
                        end--;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [ScriptableType]
        public class ScriptableClass
        {
            public int GetLength(string s)
            { return s.Length; }
        }
        private void Palindrome_Click(object sender, RoutedEventArgs e)
        {
            ScriptableMember localclass = new ScriptableMember();
            txtValue.Text = localclass.IsPalindrome(txtValue.Text).ToString().ToLower();
        }
        private void GetLength_Click(object sender, RoutedEventArgs e)
        {
            ScriptableClass localclass = new ScriptableClass();
            txtValue.Text = localclass.GetLength(txtValue.Text).ToString().ToLower();
        }
    }
  
}
