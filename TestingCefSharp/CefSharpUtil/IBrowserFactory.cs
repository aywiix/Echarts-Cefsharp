using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingCefSharp.CefSharpUtil
{
    public interface IBrowserFactory
    {
        void InitializeGlobal();
        IBrowser CreateBrowserInstance(
            string address = null);
    }
}
