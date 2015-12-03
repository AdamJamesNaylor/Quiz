using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Web.Controllers
{
    using System.IO;
    using System.Runtime.Caching;
    using System.Xml.Linq;
    using Models;

    public class DashController : Controller {
        public static bool refresh = false;

        // GET: Dash
        public ActionResult Index()
        {
            //http://www.schillmania.com/projects/fireworks/
            var state = GetState();
            return ProcessState(state);

        }

        public string GetState() {
            var gameFolder = Server.MapPath("~/Game/");
            XElement xelement = XElement.Load(gameFolder + "/Game.xml");

            XElement stateElement = xelement.Element("state");
            return stateElement.Value;
        }

        private ActionResult ProcessState(string state) {
            switch (state) {
                case "":
                    var model = new TeamList();
                    model.Teams = GetTeams();
                    return View("AwaitingView", model);
            }
            return View();
        }

        private IEnumerable<Team> GetTeams() {
            var result = new List<Team>();

            var teamsFolder = Server.MapPath("~/Teams/");
            var dir = new DirectoryInfo(teamsFolder);
            foreach (var file in dir.GetFiles("*.xml").OrderBy(f => f.CreationTimeUtc)) {
                XElement xelement = XElement.Load(file.FullName);

                var name = xelement.Attribute("name");
                result.Add(new Team {
                    Name = name.Value
                });
            }

            return result;
        }

        public ActionResult Refresh() {
            if (!refresh)
                return Json(false);

            refresh = false;
            return Json(true);
        }
    }
}