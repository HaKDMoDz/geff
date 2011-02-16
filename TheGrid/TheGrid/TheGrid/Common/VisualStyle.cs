using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.XPath;

namespace TheGrid.Common
{
    public static class VisualStyle
    {
        public static String CurrentVisualStyleName = String.Empty;
        public static Color BackForeColor = Color.White;
        public static Color BackForeColorChecked = Color.White;
        public static Color BackForeColorMouseOver = Color.White;
        public static Color BackColorModalScreen = Color.White;
        public static Color ForeColor = Color.White;
        public static Color BackColorDark = Color.White;
        public static Color BackColorLight = Color.White;
        public static Color BackColorLight2 = Color.White;
        public static Color Transparent = Color.Transparent;


        public static bool OpenVisualStyle(string visualStyleName)
        {
            //===========================================================================================//
            //NOTE : Utilisation du site http://colorschemedesigner.com/ pour la génération des styles   //
            //===========================================================================================//

            string pathVisualStyleFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"VisualStyle\" + visualStyleName + ".xml");

            if (!File.Exists(pathVisualStyleFile))
            {
                return false;
            }

            CurrentVisualStyleName = visualStyleName;

            XPathDocument doc = new XPathDocument(pathVisualStyleFile);
            XPathNavigator xPath = doc.CreateNavigator();

            BackForeColor = ReadColor(xPath, "palette/colorset[@id='primary']/color[@id='primary-2']");
            BackForeColorChecked = ReadColor(xPath, "palette/colorset[@id='complement']/color[@id='complement-4']");
            BackForeColorMouseOver = ReadColor(xPath, "palette/colorset[@id='primary']/color[@id='primary-4']");
            BackColorModalScreen = new Color(0.1f, 0.1f, 0.1f, 0.85f);
            ForeColor = ReadColor(xPath, "palette/colorset[@id='complement']/color[@id='complement-2']");
            BackColorDark = ReadColor(xPath, "palette/colorset[@id='primary']/color[@id='primary-3']");
            BackColorLight = ReadColor(xPath, "palette/colorset[@id='primary']/color[@id='primary-1']");
            BackColorLight2 = ReadColor(xPath, "palette/colorset[@id='primary']/color[@id='primary-1']");

            return true;
        }

        private static Color ReadColor(XPathNavigator xPath, string request)
        {
            Color color = Color.White;

            XPathNavigator navigator = xPath.SelectSingleNode(request);

            byte r = byte.Parse(navigator.GetAttribute("r", ""));
            byte g = byte.Parse(navigator.GetAttribute("g", ""));
            byte b = byte.Parse(navigator.GetAttribute("b", ""));

            color = new Color(r, g, b, 235);

            return color;
        }
    }
}
