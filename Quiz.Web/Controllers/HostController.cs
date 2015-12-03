using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Web.Controllers
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    public class HostController : Controller
    {
        // GET: Host
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reset() {

            RemoveAllTeams();

            CreateNewGame();

            DashController.refresh = true;

            return View();
        }

        private void CreateNewGame() {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-16", null),
                new XElement("Game", new XAttribute("name", "Christmas quiz"),
                new XElement("Step", new XText(""))
                ));

            StringWriter sw = new StringWriter();
            XmlWriter xWrite = XmlWriter.Create(sw);
            xDoc.Save(xWrite);
            xWrite.Close();

            // Save to Disk
            var mappedPath = Server.MapPath("~/Game/");
            xDoc.Save(mappedPath + "Game.xml");
        }

        private void RemoveAllTeams() {
            var teamsFolder = Server.MapPath("~/teams/");
            string[] filePaths = Directory.GetFiles(teamsFolder);
            foreach (string filePath in filePaths)
                System.IO.File.Delete(filePath);
        }
    }
}