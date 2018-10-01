using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();

        [HttpGet]
        public IHttpActionResult GetOrder(string code)
        {
            var result = _orderRepository.GetOrderByCode(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IEnumerable<ViewOrderExtend> GetOrdersDetails(string code)
        {
            return _orderRepository.GetOrdersDetails(code);

        }


        //public IEnumerable<OrderDetailProduct> GetOrdersDetailsProduct(string code)
        //{
        //    return _orderRepository.GetOrderDetailProductByCode(code);
        //}


        [HttpPost]
        public IEnumerable<OrderDetailProduct> GetOrdersDetailsProduct(List<OrderDetailProduct> codes)
        {
            return _orderRepository.GetOrderDetailProductByCode(codes);
        }

        
        [HttpPost]
        public HttpResponseMessage PostOrderDetailProduct(OrderDetailProduct detail)
        {
            if (detail != null)
            {
                var result = _orderRepository.AddOrderDetailProduct(detail);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        [HttpPost]
        public IEnumerable<ViewOrderExtend> GetOrdersDetails(List<OrderTShirt> codes)
        {
            if (codes != null)
                return _orderRepository.GetOrdersDetails(codes);
            else
                return null;
        }


        [HttpPost]
        public HttpResponseMessage PostRct(RctExtendModel rct)
        {
            if (rct != null)
            {
                var result = _orderRepository.SaveOrderAndGenerateXML(rct);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
