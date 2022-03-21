using BinanceSimbols.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BinanceController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinanceController : Controller
    {
        private BinanceConstModel binance;
        public BinanceController(BinanceConstModel _binanceConstModel) {
            binance = _binanceConstModel;
        }

        [Route("PublicConstants")]
        [HttpGet]
        public IActionResult PublicConstants()
        {
            return Json(binance.PublicConsts);

        }
        [Authorize]
        [Route("PrivateConstants")]
        [HttpGet]
        public IActionResult PrivateConstants()
        {
            return Json(binance.PrivateConsts);

        }

        [Route("Symbols")]
        [HttpGet]
        public IActionResult GetSymbols()
        {
            return Json(binance.Symbols);

        }

        [Route("SymbolsFiltered")]
        [HttpGet]
        public IActionResult GetSymbolsFiltered(string contains)
        {
            contains = contains.ToUpper();
            return Json(binance.Symbols.Where(x => x.Name.ToUpper().EndsWith(contains)));

        }
    }
}