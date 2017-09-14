using System;
using System.Web.Mvc;
using InterviewTask.Models;
using InterviewTask.ServiceReference1;

namespace InterviewTask.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Landing Page
        /// </summary>
        /// <returns>The Data Capture Form</returns>
        public ActionResult Data()
        {
            return View();
        }

        /// <summary>
        /// To get the formatted output for given number
        /// </summary>
        /// <param name="model">DataModel</param>
        /// <returns>JsonResults</returns>
        [HttpPost]
        public JsonResult GetPriceInWords(DataModel model)
        {
            string _word = null;
            FormatClient _client = new FormatClient();

            try
            {
                //consuming the format number service
                _word = _client.FormatPrice(model.Price);
            }
            catch (Exception ex)
            {
                //log error in DB or log file
                //As of now we display errors on UI itself(if any)
                _word = ex.Message;
            }

            return Json(new { IsSuccess = true, data = _word }, JsonRequestBehavior.AllowGet);
        }
    }
}