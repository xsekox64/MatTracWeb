using MatTracWeb.Models;
using MatTracWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MatTracWeb.Controllers
{
    public class AracController : Controller
    {
        private const string url = "http://localhost:59067/api/CustomersCarsInfo";
        //private const string urlServiceInfo = "http://localhost:59067/api/ServicesInfo/";

        private UsersReadDto SessionUserInfo
        {
            get { return (UsersReadDto)Session["UsersReadDto.SessionUserInfo"]; }
            set { Session["UsersReadDto.SessionUserInfo"] = value; }
        }
        private CustomerCarsReadDto SessionCustomerCarsRead
        {
            get { return (CustomerCarsReadDto)Session["CustomerCont.SessionCustomerCarsRead"]; }
            set { Session["CustomerCont.SessionCustomerCarsRead"] = value; }
        }
        // GET: Arac
        public ActionResult Index()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View(SessionUserInfo);
        }
        public ActionResult AracListTemp()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 1;


                CustomerCarsInfoResponceDTO customerResponce = new CustomerCarsInfoResponceDTO();
                List<CustomerCarsInfoReadDto> customerReadDto = new List<CustomerCarsInfoReadDto>();

                var recordsTotal = GetCustomerCount();


                if (!string.IsNullOrEmpty(searchValue))
                {
                    HttpResponseMessage httpResponseMessage = null;
                    string urlEnd = url + "/" + searchValue;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        httpResponseMessage = client.GetAsync(urlEnd).Result;
                    }
                    if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerCarsInfoReadDto>>(respJson);
                        if (customerReadDto.Count > 0)
                        {
                            //SessionUserInfo = usersReadDto[0];
                            customerResponce.CustomerResponceDtoList = customerReadDto;
                            customerResponce.StatusCode = 200;
                            customerResponce.Message = "İşlem Başarılı";
                        }
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        customerResponce.CustomerResponceDtoList = null;
                        customerResponce.StatusCode = 404;
                        customerResponce.Message = "İşlem Başarısız";
                    }
                    var data = customerReadDto.OrderBy(p => p.CustomerId).Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {

                    HttpResponseMessage httpResponseMessage = null;
                    string urlEnd = url + "/" + start + "/" + pageSize;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        httpResponseMessage = client.GetAsync(urlEnd).Result;
                    }
                    if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerCarsInfoReadDto>>(respJson);
                        if (customerReadDto.Count > 0)
                        {
                            //SessionUserInfo = usersReadDto[0];
                            customerResponce.CustomerResponceDtoList = customerReadDto;
                            customerResponce.StatusCode = 200;
                            customerResponce.Message = "İşlem Başarılı";
                        }
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        customerResponce.CustomerResponceDtoList = null;
                        customerResponce.StatusCode = 404;
                        customerResponce.Message = "İşlem Başarısız";
                    }
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = customerReadDto });
                }


            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        private int GetCustomerCount()
        {
            HttpResponseMessage httpResponseMessage = null;
            string urlEnd = url + "/CustomerCount";
            int count = 0;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.GetAsync(urlEnd).Result;
            }
            if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                count = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(respJson);
            }
            return count;
        }
        
    }
}