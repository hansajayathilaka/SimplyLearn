using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplyLearn
{
    public class WebBrowser
    {
        public BrowserNames BrowserName { get; set; }
        public int MajorVersion { get; set; }

        public WebBrowser(string browserNameBeforeTranslating, int majorVersion)
        {
            BrowserName = TranslateStringToBrowserName(browserNameBeforeTranslating);
            MajorVersion = majorVersion;
        }

        //public BrowserNames TranslateStringToBrowserName(string browserName)
        //{
        //    //to reduce if conditions and increase code readability
        //    BrowserName = browserName switch
        //    {
        //        "IE" =>BrowserNames.InternetExplorer,
        //        "ME" =>BrowserNames.Edge,
        //        "MF" =>BrowserNames.Firefox,
        //        "GC" =>BrowserNames.Chrome,
        //        "OP" =>BrowserNames.Opera,
        //        "SA" =>BrowserNames.Safari,
        //        "DO" =>BrowserNames.Dolphin,
        //        "KQ" =>BrowserNames.Konqueror,
        //        "LY" =>BrowserNames.Lynx,
        //        _=>BrowserNames.Unknown,
        //    };
        //    return BrowserName;
        //}

        private BrowserNames TranslateStringToBrowserName(string browserName)
        {
            //changed to switch case making code readability better
            switch (browserName)
            {
                case "IE":
                    return BrowserNames.InternetExplorer;
                    break;

                case "ME":
                    return BrowserNames.Edge;
                    break;

                case "MF":
                    return BrowserNames.Firefox;
                    break;

                case "GC":
                    return BrowserNames.Chrome;
                    break;

                case "OP":
                    return BrowserNames.Opera;
                    break;

                case "SA":
                    return BrowserNames.Safari;
                    break;

                case "DO":
                    return BrowserNames.Dolphin;
                    break;

                case "KQ":
                    return BrowserNames.Konqueror;
                    break;

                case "LY":
                    return BrowserNames.Lynx;
                    break;

                default:
                    return BrowserNames.Unknown;
                    break;
            }

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
}
