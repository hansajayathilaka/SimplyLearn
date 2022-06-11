using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyLearnServer.Model
{
    public class WebBrowser
    {
        public BrowserNames BrowserName { get; set; }
        public int MajorVersion { get; set; }
    }

    public enum BrowserNames
    {
        Unknown,
        InternetExplorer,
        Edge,
        Firefox,
        Chrome,
        Opera,
        Safari,
        Dolphin,
        Konqueror,
        Lynx
    }
}
