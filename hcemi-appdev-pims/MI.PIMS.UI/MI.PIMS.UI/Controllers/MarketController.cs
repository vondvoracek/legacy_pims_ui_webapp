using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MI.PIMS.BO.Dtos;
using MI.PIMS.UI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MI.PIMS.UI.Controllers
{
    public class MarketController : Controller
    {
        readonly MarketRepository _repository;

        public MarketController(MarketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var markets = await _repository.GetSet();
            return Json(markets, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}