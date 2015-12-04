namespace Quiz.Web.Controllers {
    using System.IO;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    public class XmlHelper {

        public XmlHelper(HttpServerUtilityBase httpServer) {
            _httpServer = httpServer;
        }

        public string GetState()
        {
            var gameFolder = _httpServer.MapPath("~/Game/");
            XElement xelement = XElement.Load(gameFolder + "/Game.xml");

            XElement stateElement = xelement.Element("state");
            return stateElement.Value;
        }

        public void ChangeState(string state) {

            var gameFolder = _httpServer.MapPath("~/Game/");
            XElement xelement = XElement.Load(gameFolder + "/Game.xml");
            xelement.Element("state").Value = state;

            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-16", null),
                xelement);

            StringWriter sw = new StringWriter();
            XmlWriter xWrite = XmlWriter.Create(sw);
            xDoc.Save(xWrite);
            xWrite.Close();

            // Save to Disk
            var mappedPath = _httpServer.MapPath("~/Game/");
            xDoc.Save(mappedPath + "Game.xml");
        }

        private readonly HttpServerUtilityBase _httpServer;

    }
}