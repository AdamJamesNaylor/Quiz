namespace Quiz.Web.Controllers {
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    public class XmlHelper {

        public XmlHelper(HttpServerUtilityBase httpServer) {
            _httpServer = httpServer;
        }

        public bool GameExists() {
            var gameFolder = _httpServer.MapPath("~/Game/");
            return File.Exists(gameFolder + "/Game.xml");
        }

        public string GetState()
        {
            var gameFolder = _httpServer.MapPath("~/Game/");
            XElement xelement = XElement.Load(gameFolder + "/Game.xml");

            XElement stateElement = xelement.Element("State");
            return stateElement.Value;
        }

        public void ChangeState(string state) {

            var gameFolder = _httpServer.MapPath("~/Game/");
            XElement xelement = XElement.Load(gameFolder + "/Game.xml");
            xelement.Element("State").Value = state;

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

        public void CreateNewGame() {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-16", null),
                new XElement("Game", new XAttribute("name", "Christmas quiz"),
                new XElement("State", new XText(""))
                ));

            StringWriter sw = new StringWriter();
            XmlWriter xWrite = XmlWriter.Create(sw);
            xDoc.Save(xWrite);
            xWrite.Close();

            // Save to Disk
            var mappedPath = _httpServer.MapPath("~/Game/");
            xDoc.Save(mappedPath + "Game.xml");
        }

        private readonly HttpServerUtilityBase _httpServer;

        public int Next(string currentState) {

            var gameFolder = _httpServer.MapPath("~/Game/");
            XElement xelement = XElement.Load(gameFolder + "/Questions.xml");

            var questions = xelement.Value.Split(',').ToList();
            int result = 0;
            if (!int.TryParse(currentState, out result))
                return Convert.ToInt32(questions.First());

            return Convert.ToInt32(questions.First(q => q == currentState));
        }
    }


}