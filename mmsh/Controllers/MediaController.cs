using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mmsh.Controllers
{
    public class MediaController : Controller
    {
        //
        // GET: /Media/
        public ActionResult Index()
        {
            ViewBag.Message = "Медиа";

            return View();
        }
        public ActionResult Audio()
        {
            ViewBag.Message = "Медиа > Аудио";

            return View();
        }

        public ActionResult Video()
        {
            ViewBag.Message = "Медиа > Видео";

            return View();
        }

        public ActionResult Pictures()
        {
            ViewBag.Message = "Медиа > Картинки";

            return View();
        }
	}
}