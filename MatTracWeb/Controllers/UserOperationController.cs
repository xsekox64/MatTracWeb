using MatTracWeb.Models;
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
    public class UserOperationController : Controller
    {
        private const string url = "http://localhost:59067/api/Users/";

        private UsersReadDto SessionUserInfo
        {
            get { return (UsersReadDto)Session["UsersReadDto.SessionUserInfo"]; }
            set { Session["UsersReadDto.SessionUserInfo"] = value; }
        }
        // GET: UserOperation
        public ActionResult Index()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View(SessionUserInfo);
        }
        public ActionResult UserProcess()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View(SessionUserInfo);
        }
        public JsonResult UserListTemp()
        {
            try
            {
                UserResponceDTO usersResponce = new UserResponceDTO();
                List<UsersReadDto> usersReadDto = new List<UsersReadDto>();
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(url).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    usersReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UsersReadDto>>(respJson);
                    if (usersReadDto.Count > 0)
                    {
                        //SessionUserInfo = usersReadDto[0];
                        usersResponce.UserResponceDtoList = usersReadDto;
                        usersResponce.StatusCode = 200;
                        usersResponce.Message = "İşlem Başarılı";
                    }
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    usersResponce.UserResponceDtoList = null;
                    usersResponce.StatusCode = 404;
                    usersResponce.Message = "İşlem Başarısız";

                }
                return Json(usersReadDto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> UserUpdateSave(UsersUpdateDto usersUpdateDto)
        {
            try
            {
               
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(usersUpdateDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PutAsync(url, content);
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
        public async Task<ActionResult> UserDelete(string userId)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlend = url + userId;                
                HttpResponseMessage response = null;
                response = await client.GetAsync(urlend);
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

        public ActionResult Company()
        {
            return View();
        }
        public JsonResult CompanyList()
        {
            try
            {
                CompanyResponceDTO companyResponce = new CompanyResponceDTO();
                List<CompanyReadDto> companyReadDto = new List<CompanyReadDto>();
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;
                string urledn = url + "GetCompanyAllBy";
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urledn).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    companyReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CompanyReadDto>>(respJson);
                    if (companyReadDto.Count > 0)
                    {
                        companyResponce.CompanyResponceDtoList = companyReadDto;
                        companyResponce.StatusCode = 200;
                        companyResponce.Message = "İşlem Başarılı";
                    }
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    companyResponce.CompanyResponceDtoList = null;
                    companyResponce.StatusCode = 404;
                    companyResponce.Message = "İşlem Başarısız";

                }
                return Json(companyReadDto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> CompanyUpdateSave(CompanyUpdateDto companyUpdateDto)
        {
            try
            {
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(companyUpdateDto);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlend = url + "UpdateCompany";
                response = await client.PutAsync(urlend, content);
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

        public async Task<ActionResult> UserSaveControl(UsersCreateControl userControl)
        {
            try
            {
                Random rnd = new Random();
                int month = rnd.Next(1, 13);  
                int dice = rnd.Next(12, 27);
                int card = rnd.Next(52);
                string endRdn = month.ToString() + dice.ToString() + card.ToString();
                UsersCreateDto userCreate = new UsersCreateDto();
                userCreate.UserName = userControl.UserName.Trim(); 
                userCreate.UserSurname = userControl.UserSurname.Trim();
                userCreate.CompanyId = SessionUserInfo.CompanyId;
                userCreate.Email = userControl.Email.Trim();
                userCreate.UserAuthorization = Convert.ToInt32(userControl.UserAuthorization);
                userCreate.TcNo = userControl.TcNo.Trim(); ;
                userCreate.Phone = userControl.Phone.Trim(); ;
                userCreate.WebPlatform = userControl.WebPlatform == "1" ? true : false;
                userCreate.AndoridPlatform = userControl.AndoridPlatform == "1" ? true : false;
                userCreate.IOSPlatform = userControl.IOSPlatform == "1" ? true : false;
                userCreate.Password = endRdn;
                userCreate.PasswordChange = 1;                
               
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(userCreate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
                       
        }

        public ActionResult CompanySave()
        {
            return View();
        }

        public async Task<ActionResult> GetCityFront()
        {
            try
            {
                List<GetCityAllBy> getCityAllBy = new List<GetCityAllBy>();
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;
                string urledn = url + "GetCityAllBy";
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urledn).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    getCityAllBy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetCityAllBy>>(respJson);                    
                }               
                return Json(getCityAllBy, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> GetCountyFront(string parentId)
        {
            try
            {
                List<GetCounty> getCountyBy = new List<GetCounty>();
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;
                string urledn = url + "GetCountyAllBy?parentId=" + parentId;
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urledn).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    getCountyBy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetCounty>>(respJson);
                }
                return Json(getCountyBy, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> CompanySaveControl(CompanyCreateDto companyCreate)
        {
            try
            {  
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(companyCreate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlend = url + "CreateCompanys";
                response = await client.PostAsync(urlend, content);
                if (!response.IsSuccessStatusCode)
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
    }
}