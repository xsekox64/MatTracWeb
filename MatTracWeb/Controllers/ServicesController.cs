using MatTracWeb.Models;
using MatTracWeb.ViewModels;
using MatTracWeb.ViewModels.ServiceInfo;
using MatTracWeb.ViewModels.Services;
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
    public class ServicesController : Controller
    {
        private const string url = "http://localhost:59067/api/ServicesInfo";
        private const string urlCustomer = "http://localhost:59067/api/CustomersCarsInfo/CustomerCarsId?customerCarsId=";
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
        //private List<ServicesParcaFiyatListReadDto> SessionParcaFiyatListRead
        //{
        //    get { return (List<ServicesParcaFiyatListReadDto>)Session["Services.SessionParcaFiyatListRead"]; }
        //    set { Session["Services.SessionParcaFiyatListRead"] = value; }
        //}
        // GET: Services
        public ActionResult Index(int customerCarsId)
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            CustomerCarSingletonCilent customerResponce = new CustomerCarSingletonCilent();
            List<ServicesCustomerCars> customerReadDto = new List<ServicesCustomerCars>();
            HttpResponseMessage httpResponseMessage = null;
            string urlEnd = urlCustomer + customerCarsId;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.GetAsync(urlEnd).Result;
            }
            if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServicesCustomerCars>>(respJson);
                if (customerReadDto.Count > 0)
                {
                    //SessionUserInfo = usersReadDto[0];
                    customerResponce.CustomerCarSingletonDtoList = customerReadDto;
                    customerResponce.StatusCode = 200;
                    customerResponce.Message = "İşlem Başarılı";
                }
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                customerResponce.CustomerCarSingletonDtoList = null;
                customerResponce.StatusCode = 404;
                customerResponce.Message = "İşlem Başarısız";
            }
            if (customerResponce.StatusCode == 200)
            {
                ViewBag.services = customerReadDto.FirstOrDefault();
                return View();
            }
            return null;
        }
        public async Task<ActionResult> CustomerDemandSave(CustomerDemand customerDemand)
        {
            try
            {
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(customerDemand);
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
        public ActionResult ServiseHistory(int customerCarsId)
        {
            if (customerCarsId == null || customerCarsId == 0)
            {
                return Redirect("~/Login/Index");
            }
            ViewBag.customerCarId = customerCarsId;
            return View();
        }
        public ActionResult ServiceHistoryListTemp(int customerCarId)
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


                ServiceHistoryListResponceDto srvHistoryResponce = new ServiceHistoryListResponceDto();
                List<ServiceHistoryListReadDto> srvHistoryListRead = new List<ServiceHistoryListReadDto>();

                var recordsTotal = GetServiceHistoryCount(customerCarId);


                if (!string.IsNullOrEmpty(searchValue))
                {
                    HttpResponseMessage httpResponseMessage = null;
                    string urlEnd = url + "/GetServiceInfoHistory?customerCarId=" + customerCarId + "&text=" + searchValue;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        httpResponseMessage = client.GetAsync(urlEnd).Result;
                    }
                    if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        srvHistoryListRead = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServiceHistoryListReadDto>>(respJson);
                        if (srvHistoryListRead.Count > 0)
                        {
                            //SessionUserInfo = usersReadDto[0];
                            srvHistoryResponce.ServiceHistoryListReadDtoList = srvHistoryListRead;
                            srvHistoryResponce.StatusCode = 200;
                            srvHistoryResponce.Message = "İşlem Başarılı";
                        }
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        srvHistoryResponce.ServiceHistoryListReadDtoList = null;
                        srvHistoryResponce.StatusCode = 404;
                        srvHistoryResponce.Message = "İşlem Başarısız";
                    }
                    var data = srvHistoryListRead.OrderBy(p => p.CustomerCarsId).Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {

                    HttpResponseMessage httpResponseMessage = null;
                    string urlEnd = url + "/GetCServiceHistoryList?customerCarsId=" + customerCarId + "&start=" + start + "&pagesize=" + pageSize;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        httpResponseMessage = client.GetAsync(urlEnd).Result;
                    }
                    if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        srvHistoryListRead = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServiceHistoryListReadDto>>(respJson);
                        if (srvHistoryListRead.Count > 0)
                        {
                            //SessionUserInfo = usersReadDto[0];
                            srvHistoryResponce.ServiceHistoryListReadDtoList = srvHistoryListRead;
                            srvHistoryResponce.StatusCode = 200;
                            srvHistoryResponce.Message = "İşlem Başarılı";
                        }
                    }
                    else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        srvHistoryResponce.ServiceHistoryListReadDtoList = null;
                        srvHistoryResponce.StatusCode = 404;
                        srvHistoryResponce.Message = "İşlem Başarısız";
                    }
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = srvHistoryListRead });
                }


            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        private int GetServiceHistoryCount(int customerCarsId)
        {
            HttpResponseMessage httpResponseMessage = null;
            string urlEnd = url + "/ServiceHistoryCount?customerCarsId=" + customerCarsId;
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

        public ActionResult Invoice(int customerCarsId, int Id)
        {
            if (customerCarsId == 0 || Id==0)
            {
                return Redirect("~/Login/Index");
            }
            ViewBag.customerCarId = customerCarsId;
            ViewBag.Id = Id;

            CustomerCarSingletonCilent customerResponce = new CustomerCarSingletonCilent();
            List<ServicesCustomerCars> customerReadDto = new List<ServicesCustomerCars>();
            HttpResponseMessage httpResponseMessage = null;
            string urlEnd = urlCustomer + customerCarsId;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.GetAsync(urlEnd).Result;
            }
            if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                customerReadDto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServicesCustomerCars>>(respJson);
                if (customerReadDto.Count > 0)
                {
                    //SessionUserInfo = usersReadDto[0];
                    customerResponce.CustomerCarSingletonDtoList = customerReadDto;
                    customerResponce.StatusCode = 200;
                    customerResponce.Message = "İşlem Başarılı";
                }
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                customerResponce.CustomerCarSingletonDtoList = null;
                customerResponce.StatusCode = 404;
                customerResponce.Message = "İşlem Başarısız";
            }
            if (customerResponce.StatusCode == 200)
            {
                ViewBag.services = customerReadDto.FirstOrDefault();
                return View();
            }
            return null;
        }
      
        public ActionResult PartsPrice()
        {
            if (SessionUserInfo == null)
            {
                return Redirect("~/Login/Index");
            }
            return View();
        }        
        public ActionResult PartsPriceList()
        {
            try
            {
                //buradayım
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 1;


                ServicesParcaFiyatListResponceDto srvParcaFiyatResponce = new ServicesParcaFiyatListResponceDto();
                List<ServicesParcaFiyatListReadDto> srvParcaFiyatListRead = new List<ServicesParcaFiyatListReadDto>();

                HttpResponseMessage httpResponseMessage = null;
                string urlEnd = url + "/ParcaFiyatList";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpResponseMessage = client.GetAsync(urlEnd).Result;
                }
                if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    srvParcaFiyatListRead = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServicesParcaFiyatListReadDto>>(respJson);
                    if (srvParcaFiyatListRead.Count > 0)
                    {
                        //SessionUserInfo = usersReadDto[0];
                        srvParcaFiyatResponce.ServicesParcaFiyatListReadDto = srvParcaFiyatListRead;
                        srvParcaFiyatResponce.StatusCode = 200;
                        srvParcaFiyatResponce.Message = "İşlem Başarılı";
                    }
                }
                else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    srvParcaFiyatResponce.ServicesParcaFiyatListReadDto = null;
                    srvParcaFiyatResponce.StatusCode = 404;
                    srvParcaFiyatResponce.Message = "İşlem Başarısız";
                }
                var recordsTotal = srvParcaFiyatListRead.Count();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    srvParcaFiyatListRead = srvParcaFiyatListRead.Where(m => m.IslemAdi.Contains(searchValue) || m.BirimFiyat.ToString().Contains(searchValue) ||
                        m.Iscilik.ToString().Contains(searchValue) || m.Amount.ToString().Contains(searchValue)).ToList();

                    //Paging     
                    var data = srvParcaFiyatListRead.OrderBy(p => p.ParcaFiyatId).Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data    
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {
                    var data = srvParcaFiyatListRead.OrderBy(p => p.ParcaFiyatId).Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> PartPriceSave(ServicePartFiyatSave servicePartFiyatSave)
        {
            try
            {
                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(servicePartFiyatSave);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlEnd = url + "/PartFiyatSave";
                response = await client.PostAsync(urlEnd, content);
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
        public async Task<ActionResult> PartPriceUpdateSave(ServicePartFiyatUpdate servicePartPriceUpdate)
        {
            try
            {

                HttpClient client = new HttpClient();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(servicePartPriceUpdate);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                string urlEnd = url + "/PartFiyatUpdate";
                response = await client.PutAsync(urlEnd, content);
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
        public async Task<ActionResult> PartPriceDelete(string parcaFiyatId)
        {
            try
            {
                if (parcaFiyatId != "")
                {
                    HttpClient client = new HttpClient();
                    string urlend = url + "?id=" + parcaFiyatId;
                    HttpResponseMessage response = null;
                    response = await client.DeleteAsync(urlend);
                    if (!response.IsSuccessStatusCode)
                    {
                        return Json("0", JsonRequestBehavior.AllowGet);
                    }
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else { return Json("0", JsonRequestBehavior.AllowGet); }

            }
            catch (Exception ex)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetServicesPartPrice()
        {
            ServicesParcaFiyatListResponceDto srvParcaFiyatResponce = new ServicesParcaFiyatListResponceDto();
            List<ServicesParcaFiyatListReadDto> srvParcaFiyatListRead = new List<ServicesParcaFiyatListReadDto>();

            HttpResponseMessage httpResponseMessage = null;
            string urlEnd = url + "/ParcaFiyatList";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.GetAsync(urlEnd).Result;
            }
            if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                srvParcaFiyatListRead = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServicesParcaFiyatListReadDto>>(respJson);
                if (srvParcaFiyatListRead.Count > 0)
                {
                    //SessionUserInfo = usersReadDto[0];
                    srvParcaFiyatResponce.ServicesParcaFiyatListReadDto = srvParcaFiyatListRead;
                    srvParcaFiyatResponce.StatusCode = 200;
                    srvParcaFiyatResponce.Message = "İşlem Başarılı";
                }
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                srvParcaFiyatResponce.ServicesParcaFiyatListReadDto = null;
                srvParcaFiyatResponce.StatusCode = 404;
                srvParcaFiyatResponce.Message = "İşlem Başarısız";
            }
            //return Json(new { data = srvParcaFiyatListRead });
            return Json(srvParcaFiyatListRead, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> InvoiceListTempRes(string Id)
        {
            ServicesInvoiceResponceDto srvInvoiceResponce = new ServicesInvoiceResponceDto();
            List<InvoiceListTemp> srvInvoiceLİst = new List<InvoiceListTemp>();

            HttpResponseMessage httpResponseMessage = null;
            string urlEnd = url + "/GetServiceInvoiceList?Id=" + Id;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpResponseMessage = client.GetAsync(urlEnd).Result;
            }
            if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respJson = httpResponseMessage.Content.ReadAsStringAsync().Result;
                srvInvoiceLİst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InvoiceListTemp>>(respJson);
                if (srvInvoiceLİst.Count > 0)
                {
                    //SessionUserInfo = usersReadDto[0];
                    srvInvoiceResponce.ServicesInvoiceListReadDto = srvInvoiceLİst;
                    srvInvoiceResponce.StatusCode = 200;
                    srvInvoiceResponce.Message = "İşlem Başarılı";
                }
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                srvInvoiceResponce.ServicesInvoiceListReadDto = null;
                srvInvoiceResponce.StatusCode = 404;
                srvInvoiceResponce.Message = "İşlem Başarısız";
            }
            return Json(srvInvoiceLİst, JsonRequestBehavior.AllowGet);
        }
    }
}
