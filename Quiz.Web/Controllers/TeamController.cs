
namespace Quiz.Web.Controllers {
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Linq;
    using Models;

    public class TeamController
        : Controller {

        [HttpGet]
        public ActionResult Join() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Team team) {

            var guid = Guid.NewGuid();
            //save image
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-16", null),
                new XElement("Team", new XAttribute("name", team.Name), new XAttribute("image", guid)));

            StringWriter sw = new StringWriter();
            XmlWriter xWrite = XmlWriter.Create(sw);
            xDoc.Save(xWrite);
            xWrite.Close();

            // Save to Disk
            var mappedPath = Server.MapPath("~/Teams/");
            xDoc.Save(mappedPath + guid + ".xml");

            DashController.refresh = true;

            return Redirect("/test");
        }
    }
}