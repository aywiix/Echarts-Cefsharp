using CefSharp;
using System.Windows.Forms;

namespace TestingCefSharp.CefSharpUtil
{
    public interface IBrowser
    {
        string EvaluateScript(
            string script);
        void ExecuteAfterLoad(string afterLoadScript);
        void ShowDevTools();
        Control Control { get; }
        void SetMenu(IContextMenuHandler menu);
    }
}
