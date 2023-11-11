using MatTracWeb.Models;
using MatTracWeb.Models.Calenders;
using MatTracWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MatTracWeb.Controllers
{
    public class HomeController : Controller
    {
        private const string url = "http://localhost:59067/api/Calendar";
        private UsersReadDto SessionUserInfo
        {
            get { return (UsersReadDto)Session["UsersReadDto.SessionUserInfo"]; }
            set { Session["UsersReadDto.SessionUserInfo"] = value; }
        }
        private List<CalendarListRead> SessionCalendarInfo
        {
            get { return (List<CalendarListRead>)Session["CalendarListRead.SessionCalendarInfo"]; }
            set { Session["CalendarListRead.SessionCalendarInfo"] = value; }
        }
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View(SessionUserInfo);
        }

        public ActionResult Calander()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            List<CalendarListRead> calendarList = new List<CalendarListRead>();
            try
            {
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;
                string urledn = url + "/" + SessionUserInfo.CompanyId;
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urledn).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    calendarList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CalendarListRead>>(respJson);
                    SessionCalendarInfo = calendarList;
                }
                return View(new { Values = calendarList });
            }
            catch (Exception)
            {
                return View(new { Values = calendarList });
            }
        }

        public async Task<ActionResult> YeniEtkinlikEkle(YeniEtkinlikViewModel yeniEtkinlikEkle)
        {
            try
            {
                int intType = yeniEtkinlikEkle.interviewType == "Randevu" ? 1
                             : yeniEtkinlikEkle.interviewType == "Toplantı" ? 2
                             : yeniEtkinlikEkle.interviewType == "Görüşme" ? 3 : 0;
                CalendarListCreate calenderList = new CalendarListCreate();
                calenderList.allDay = null;
                calenderList.CompanyId = SessionUserInfo.CompanyId;
                calenderList.decsr = yeniEtkinlikEkle.descr.Trim();
                calenderList.endFin = yeniEtkinlikEkle.datesecond.Trim();
                calenderList.hour = yeniEtkinlikEkle.timeHour.Trim();
                calenderList.InterviewType = intType;
                calenderList.start = yeniEtkinlikEkle.datefirst.Trim();
                calenderList.title = yeniEtkinlikEkle.title.Trim();
                calenderList.url = null;
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(calenderList);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult TakvimAciklamaGetir(string id)
        {
            try
            {
                int calendarId = Convert.ToInt32(id);
                var responce = SessionCalendarInfo.Where(x => x.CalendarId == calendarId).FirstOrDefault();                
                return Json(responce, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> TakvimEtkinlikSil(string idsil)
        {
            try
            {
                if (idsil != "")
                {
                    HttpClient client = new HttpClient();
                    string urlend = url + "?id=" + idsil;
                    HttpResponseMessage response = null;
                    response = await client.DeleteAsync(urlend);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }                    
                    return View();
                }
                else { return null; }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult> TakvimiGuncelle(CalendarListUpdate calendarUpdate)
        {
            try
            {
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(calendarUpdate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlend = url + "/UpdateCalendarBy";
                response = await client.PutAsync(urlend, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View();
                }                
                else
                {
                    return null;
                }                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}