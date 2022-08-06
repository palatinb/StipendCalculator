using System;
using System.IO;
using System.Xml;

namespace StudyStipendCalcAPI.Helpers
{
    public class AddDbConnStr
    {
        public static void AddUpdateConnString(string name)
        {
            bool isNew = false;
            string path = Path.GetFullPath("~/appsettings.json");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
        }
    }
}
