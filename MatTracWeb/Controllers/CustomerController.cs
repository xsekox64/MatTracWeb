using MatTracWeb.Models;
using MatTracWeb.Models.Customers;
using MatTracWeb.ViewModels;
using MatTracWeb.ViewModels.Services;
using Newtonsoft.Json;
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
    public class CustomerController : Controller
    {
        private const string url = "http://localhost:59067/api/Customer";
        private const string urlServiceInfo = "http://localhost:59067/api/ServicesInfo/";

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
        // GET: Customer
        public ActionResult Index()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View(SessionUserInfo);
        }

        public ActionResult CustomerDetail(string customerId)
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            CustomerCarsReadDto customerCarsReadDto = new CustomerCarsReadDto();
            HttpResponseMessage httpResponseMessage = null;
            string urlend = url + "/GetCustomerCarsList?customerId=" + Convert.ToInt32(customerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.GetAsync(urlend).Result;
            }
            if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                customerCarsReadDto = JsonConvert.DeserializeObject<CustomerCarsReadDto>(respJson);
                if(customerCarsReadDto != null)
                {
                    ViewBag.data = customerCarsReadDto;
                    SessionCustomerCarsRead = customerCarsReadDto;
                }
            }            
            List<CustomerDemand> servicetapName = new List<CustomerDemand>();
            HttpResponseMessage httpResponseMessageSrv = null;
            string urlSrv = urlServiceInfo + Convert.ToInt32(customerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessageSrv = client.GetAsync(urlSrv).Result;
            }
            if (httpResponseMessageSrv != null && httpResponseMessageSrv.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessageSrv.Content.ReadAsStringAsync().Result;
                servicetapName = JsonConvert.DeserializeObject<List<CustomerDemand>>(respJson);                              
                if (servicetapName != null)
                {                    
                    ViewBag.servTapName = servicetapName.Take(5);
                }
            }
            return View();
        }

        public ActionResult AracBilgisi()
        {

            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View();
        }

        public async Task<ActionResult> CustomerSaveSvc(CustomerCreateDto customerCreate)
        {
            try
            {                
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(customerCreate);
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
       
        public ActionResult CustomerListTemp()
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


                CustomerResponceDTO customerResponce = new CustomerResponceDTO();
                List<CustomerReadDto> customerReadDto = new List<CustomerReadDto>();

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
                        customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerReadDto>>(respJson);
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
                        customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerReadDto>>(respJson);
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


        public ActionResult CustomerListCombo()
        {
            try
            {                
                CustomerResponceDTO customerResponce = new CustomerResponceDTO();
                List<CustomerReadDto> customerReadDto = new List<CustomerReadDto>();
                HttpResponseMessage httpResponseMessage = null;
                string urlEnd = url + "/textSearch?textSearch" ;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urlEnd).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerReadDto>>(respJson);
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
                return Json(customerReadDto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> GetMusteriGetir()
        {
            try
            {
                List<CustomerReadDto> getCustomerBy = new List<CustomerReadDto>();
                HttpResponseMessage httpResponseMessage = null;                
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(url).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    getCustomerBy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerReadDto>>(respJson);
                }
                return Json(getCustomerBy, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> CustomerCarSave(CustomerCarsCreate customerCarsCreate)
        {
            try
            {
                if(customerCarsCreate.CustomerId == 0)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
                
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(customerCarsCreate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlend = url + "/CreateCustomerCarsBy";
                response = await client.PostAsync(urlend, content);
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

        public async Task<ActionResult> CustomerUpdate(CustomerUpdateDto customerUpdate)
        {
            try
            {
                if (customerUpdate.CustomerId == 0)
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }

                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(customerUpdate);
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

        public ActionResult ServisFormu()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View();
        }

        public ActionResult CustomerSave()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View();
        }
    }
}