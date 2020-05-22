using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;

namespace TestingCefSharp.CefSharpUtil
{
    
    public class CefSharpFactory : IBrowserFactory
    {
        //Cada módulo que usar o CefSharp precisa registrar os próprios paths!!
        //melhor maneira de resolver?
        private static List<CefCustomScheme> PreRegisteredSchemes { get; set; } = new List<CefCustomScheme>()
        {
            new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: @"/HTML/",
                    hostName: "cefsharp",
                    defaultPage: "ECharts.html"
                )
            },
           
            //adicione um CefCustomScheme na coleção se for necessário
        };

        //General use:https://github.com/cefsharp/CefSharp/wiki/General-Usage
        //Settings: https://github.com/cefsharp/CefSharp/wiki/General-Usage#initialize-and-shutdown
        //Local page: https://stackoverflow.com/questions/52338368/loading-local-html-css-js-files-with-cefsharp-v65
        public void InitializeGlobal()
        {
            if (Cef.IsInitialized)
                return;

            var chromeSettings = new CefSettings();
            chromeSettings.CefCommandLineArgs.Add("enable-media-stream");
            chromeSettings.CefCommandLineArgs.Add("no-proxy-server");

            foreach (var scheme in PreRegisteredSchemes)
            {
                chromeSettings.RegisterScheme(scheme);
            }

            Cef.Initialize(chromeSettings, performDependencyCheck: true, browserProcessHandler: null);
        }

        public IBrowser CreateBrowserInstance(
            string address = null)
        {
            var chromiumWebBrowser = new ChromiumWebBrowser(address)
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                ActivateBrowserOnCreation = true
            };


            var browser = new CefSharpBrowser(chromiumWebBrowser);
            return browser;
        }
    }
}
