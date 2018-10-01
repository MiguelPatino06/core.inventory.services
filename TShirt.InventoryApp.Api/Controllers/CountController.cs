using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
    public class CountController : ApiController
    {
        private readonly ICountRepository _countRepository = new CountRepository();

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var result = _countRepository.GetAll();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetCountById(int id)
        {
            var result = _countRepository.GetById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public string SaveDetails(List<CountPlanDetailItem> codes)
        {
            return _countRepository.Save(codes);
        }

        [HttpGet]
        public IHttpActionResult GetCountByPlanAndProduct(int id, string product)
        {
            var result = _countRepository.GetByPlanAndProduct(id, product);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpPost]
        public bool SavePlan(CountPlan items)
        {
            return _countRepository.Update(items);
        }

        [HttpGet]
        public IHttpActionResult GetCountByIdPage(int id)
        {
            var result = _countRepository.GetByIdPage(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetCountByIdPageSkipTake(int id, int skip, int take)
        {
            var result = _countRepository.GetByIdPageTakeSkip(id, take, skip);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}