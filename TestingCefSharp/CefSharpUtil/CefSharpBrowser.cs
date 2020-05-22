using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace TestingCefSharp.CefSharpUtil
{
    public class CefSharpBrowser : IBrowser
    {
        private readonly ChromiumWebBrowser _chromiumWebBrowser;

        public CefSharpBrowser(ChromiumWebBrowser chromiumWebBrowser)
        {
            _chromiumWebBrowser = chromiumWebBrowser;
        }

        public string EvaluateScript(
            string script)
        {
            var javascriptResponse = _chromiumWebBrowser.EvaluateScriptAsync(script).Result;

            return javascriptResponse.Result.ToString();
        }

        public void ExecuteAfterLoad(string afterLoadScript)
        {
            _chromiumWebBrowser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    args.Frame.ExecuteJavaScriptAsync(afterLoadScript);
                }
            };
        }

        public void SetMenu(IContextMenuHandler menu)
        {
            _chromiumWebBrowser.MenuHandler = menu;
        }

        public void ShowDevTools()
        {
            if (!_chromiumWebBrowser.IsBrowserInitialized)
            {
                _chromiumWebBrowser.IsBrowserInitializedChanged += ChromeBrowser_IsBrowserInitializedChanged;
            }
            else
            {
                _chromiumWebBrowser.ShowDevTools();
            }
        }

        private void ChromeBrowser_IsBrowserInitializedChanged(
            object sender,
            EventArgs e)
        {
            _chromiumWebBrowser.ShowDevTools();
        }

        public Control Control => _chromiumWebBrowser;
    }
}
