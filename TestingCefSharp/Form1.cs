using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using Newtonsoft.Json;

namespace TestingCefSharp
{
    //https://luminousmen.com/post/big-data-file-formats
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
        }

        
        public ChromiumWebBrowser browser;
        List<List<int[]>> structure = new List<List<int[]>>();
        List<string> ConsoleMessages = new List<string>();
        public void InitBrowser()
        {
            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: @"HTML\ECharts",
                    hostName: "cefsharp",
                    defaultPage: "ECharts.html"
                )
            });

            //StartButton.Click += new EventHandler(StartButton_Click);
            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser("localfolder://cefsharp/");

            //CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            browser.JavascriptObjectRepository.Register("boundAsync", new BoundObject(), isAsync: true, options: BindingOptions.DefaultBinder);

            //browser.RegisterJsObject("boundAsync", new BoundObject());
            //browser.JavascriptObjectRepository.Register("boundAsync", new BoundObject(), true);

            //browser.JavascriptObjectRepository.Register("jsonarray", "{ahora se esta poniendo feo }", isAsync: true, options: BindingOptions.DefaultBinder);

            browser.JavascriptObjectRepository.ObjectBoundInJavascript += (sender, e) =>
            {
                var name = e.ObjectName;

                //MessageBox.Show($"Object {e.ObjectName} was bound successfully.");
            };
            this.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
            browser.ConsoleMessage += Browser_ConsoleMessage;
            //InitDense("");
           // Dense("");
        }

        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        delegate void Del(string str);

        private async void StartProcess(string type)
        {
            var time = DateTime.Now;
            var id = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < structure.Count; i++)
            {
                var parameter = JsonConvert.SerializeObject(structure[i].ToArray());
                await ExecuteAfterLoad($"Progresive" + type + $"Load({parameter});");
            }
            var finish = DateTime.Now;
            var result = finish - time;
        }


        private async void InitDense(string type)
        {

            await ExecuteAfterLoad($"SetChart('dense" + type + "');");
        }


        private async void ProgressiveLoading()
        {
            Random rd = new Random();
            List<int[]> parame = new List<int[]>();
            for (int i = 0; i < 1000000; i++)
            {
                int[] current = new int[2];
                current[0] = rd.Next(0, 10000000);
                current[1] = rd.Next(0, 10000000);
                parame.Add(current);
            }

            var parameter = JsonConvert.SerializeObject(parame.ToArray());
            await ExecuteAfterLoad($"ProgresiveLoad({parameter});");
        }

        private async void DynamicDense(string type)
        {
            for (int i = 0; i < 5; i++)
            {
                Random rd = new Random();
                List<int[]> parame = new List<int[]>();
                for (int j = 0; j < 500000; j++)
                {
                    int[] current = new int[2];
                    current[0] = rd.Next(0, 10000000);
                    current[1] = rd.Next(0, 10000000);
                    // _browser.EvaluateScript($"UpdateChart('{x}','{y}');");
                    parame.Add(current);
                }

                var parameter = JsonConvert.SerializeObject(parame.ToArray());
                await ExecuteAfterLoad($"DynamicDense({parameter});");
                await ExecuteAfterLoad($"SetChart('dense');");
            }
        }

        public async Task EvaluateScript(
            string script)
        {
            await browser.EvaluateScriptAsync(script);

            //return javascriptResponse.Result.ToString();
        }

        void Dense(string type)
        {
            int steps = 1; //int.Parse(StepsTB.Text);
            int total = 1000000; //int.Parse(TotalTB.Text);
            int interval = total / steps;
            structure = new List<List<int[]>>();
            Random rd = new Random();
            for (int i = 0; i < steps; i++)
            {
                List<int[]> parame = new List<int[]>();
                for (int j = 0; j < interval; j++)
                {
                    int[] aux = new int[]
                    {
                        rd.Next(0, 10000000),
                        rd.Next(0, 10000000)
                    };

                    parame.Add(aux);
                }
                structure.Add(parame);
            }
            
            //var chartPainel = this.Controls.Find("Chart", true).FirstOrDefault(); //["Chart"];// .Add(browser);
            StartProcess("");
        }

        public async Task ExecuteAfterLoad(string afterLoadScript)
        {
            browser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                if (args.Frame.IsMain)
                {
                    args.Frame.ExecuteJavaScriptAsync(afterLoadScript);
                }
            };
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {

        }



    }




}
