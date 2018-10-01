using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
    public class ProductChangeController : ApiController
    {
        private readonly IProductChange _productRepository = new ProductChangeRepository();

        public IHttpActionResult GetOrderByCode(string code)
        {
            var result = _productRepository.GetOrderByCode(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        public IHttpActionResult GetDetailByCode(string code)
        {
            var result = _productRepository.GetDetailByCode(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        public IHttpActionResult GetAll()
        {
            var result = _productRepository.GetAll();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        public IHttpActionResult GetAllDetail()
        {
            var result = _productRepository.GetAllDetail();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        public IHttpActionResult GetListDetailByCode(string code)
        {
            var result = _productRepository.GetOrderRedDetailExtendProduct(code);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public bool SaveOrder(OrderReqExtend items)
        {
            return _productRepository.Save(items);
        }


        [HttpPost]
        public bool Save(OrderReqExtend items)
        {
            return _productRepository.Save(items);
        }

        [HttpPost]
        public bool SaveDetail(OrderReqDetailExtend items)
        {
            return _productRepository.UpdateDetail(items);
        }
    }
}
