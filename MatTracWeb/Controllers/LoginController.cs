using MatTracWeb.Models;
using MatTracWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MatTracWeb.Controllers
{
    public class LoginController : Controller
    {
        private const string url = "http://localhost:59067/api/Users/";

        private UsersReadDto SessionUserInfo
        {
            get { return (UsersReadDto)Session["UsersReadDto.SessionUserInfo"]; }
            set { Session["UsersReadDto.SessionUserInfo"] = value; }
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserControl(LoginUserControl loginUserControl)
        {
            if (ModelState.IsValid)
            {
                UserResponceDTO usersResponce = new UserResponceDTO();
                List<UsersReadDto> usersReadDto = new List<UsersReadDto>();
                string baseurl = url + "" + loginUserControl.email + "/" + loginUserControl.password + "";
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(baseurl).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    usersReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UsersReadDto>>(respJson);
                    if (usersReadDto.Count > 0)
                    {
                        if (usersReadDto[0].PasswordChange == 1)
                        {
                            return Redirect("~/Login/Regestration");
                        }
                        else
                        {
                            SessionUserInfo = usersReadDto[0];
                            usersResponce.UserResponceDtoList = usersReadDto;
                            usersResponce.StatusCode = 200;
                            usersResponce.Message = "İşlem Başarılı";
                            return Redirect("~/Home/Index");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "* E-Posta veya Şifre hatalı!");
                        return View("Index", loginUserControl);
                    }
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    usersResponce.UserResponceDtoList = null;
                    usersResponce.StatusCode = 404;
                    usersResponce.Message = "İşlem Başarısız";
                    ModelState.AddModelError(string.Empty, "* Servis Hatası Lütfen Satıcınızla Görüşün!");
                    return View("Index", loginUserControl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "* E-Posta veya Şifre hatalı!");
                    return View("Index", loginUserControl);
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "* E-Posta veya Şifre Boş Geçilemez!");
                return View("Index", loginUserControl);
            }
        }

        public ActionResult Regestration()
        {
            return View();
        }
        public async Task<ActionResult> EmailControl(string email)
        {
            string emailctrl = "0";
            try
            {
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;
                string urledn = url + "GetEmailControlBy?email=" + email;

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urledn).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = Convert.ToInt32(httpResponseMessage.Content.ReadAsStringAsync().Result);
                    if (respJson != 0)
                    {
                        Random rnd = new Random();
                        int month = rnd.Next(1, 13);
                        int dice = rnd.Next(12, 27);
                        int card = rnd.Next(52);
                        string endRdn = month.ToString() + dice.ToString() + card.ToString();
                        var checkEmail = EmailDateCheck(respJson);
                        if (checkEmail.Count() > 0 )
                        {
                            DateTime dtnow = DateTime.Now;
                            DateTime dtcheck = checkEmail[0].SendDate.AddHours(1);
                            if (dtnow >= dtcheck || checkEmail[0].Count > 3)
                            {
                                int contPlus = 1;
                                UpdateEmailCount(respJson, contPlus, endRdn);
                                SendMail(email, endRdn);
                                emailctrl = "1";
                            }
                            else
                            {
                                if (checkEmail[0].Count > 3)
                                {
                                    emailctrl = "2";
                                }
                                else
                                {
                                    int contPlus = checkEmail[0].Count + 1;
                                    UpdateEmailCount(respJson, contPlus, endRdn);
                                    SendMail(email, endRdn);
                                    emailctrl = "1";
                                }
                            }                            
                            
                        }
                        else
                        {
                            SendMail(email, endRdn);
                            EmailControlSave(email, endRdn, respJson);
                        }
                        emailctrl = "1";
                    }
                }
                return Json(emailctrl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(emailctrl, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<bool> UpdateEmailCount(int userId, int contPlus, string code)
        {
            try
            {
                EmailControlUpdate emailUpdate = new EmailControlUpdate();
                emailUpdate.Count = contPlus;
                emailUpdate.UserId = userId;
                emailUpdate.Code = Convert.ToInt32(code);
                emailUpdate.SendDate = DateTime.Now;
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(emailUpdate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlend = url + "UpdateEmailCount";
                response = await client.PutAsync(urlend, content);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<EmailControlRead> EmailDateCheck(int userId)
        {
            List<EmailControlRead> emailDateCheck = new List<EmailControlRead>();
            try
            {
                System.Net.Http.HttpResponseMessage httpResponseMessage = null;
                string urledn = url + "GetEmailDateCheckBy?userId=" + userId;
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urledn).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    emailDateCheck = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmailControlRead>>(respJson);

                }
                return emailDateCheck;

            }
            catch (Exception ex)
            {
                return emailDateCheck;
            }
        }
        public async Task<bool> EmailControlSave(string email, string code, int userId)
        {
            try
            {
                EmailControlCreate emailCreate = new EmailControlCreate();
                emailCreate.Email = email;
                emailCreate.Code = code;
                emailCreate.Count = 1;
                emailCreate.UserId = userId;
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(emailCreate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlend = url + "CreateEmailControl";
                response = await client.PostAsync(urlend, content);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendMail(string email, string code)
        {
            bool responce = false;
            using (MailMessage mm = new MailMessage("sacokral@yandex.com", email))
            {
                mm.Subject = "Giriş Kodu";
                mm.Body = "Şifre değiştirmek için giriş kodunuz :  " + code;
                mm.IsBodyHtml = false;
                using (SmtpClient sc = new SmtpClient("smtp.yandex.ru", 587))
                {
                    //sc.EnableSsl = true;
                    //sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //sc.UseDefaultCredentials = false;
                    //sc.Credentials = new NetworkCredential("sacokral@yandex.com", "200Ser2581");
                    //sc.Send(mm);
                    responce = true;
                }
            }
            return responce;
        }
        
        public async Task<ActionResult> PasswodrChange(LoginPasswordChange loginPassChange)
        {
            string boolstr = "0";
            try
            {
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(loginPassChange);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string parolam = FormsAuthentication.HashPasswordForStoringInConfigFile(loginPassChange.passwordagain, "md5");
                string urlend = url + "PasswodrChangeBy?code= "+ loginPassChange.ecode.Trim() + "&pass= "+ parolam + " ";
                response = await client.PutAsync(urlend, content);
                if (!response.IsSuccessStatusCode)
                {
                    boolstr = "0";
                }
                else if(response.IsSuccessStatusCode == true)
                {
                    boolstr = "1";
                }
                return Json(boolstr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(boolstr, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}