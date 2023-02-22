using Microsoft.AspNetCore.Mvc;
using testt.Data;
using testt.Models;

namespace testt.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class PromoController : Controller
    {

        private readonly ApiDbContext _context;
        public PromoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public Promo Index()
        {
            var promo = new Promo { Id = new Guid(), PromoCode = "CHATIME2023", PromoName = "Chatime", 
                PromoDescription = "Promo Chatime", PromoStart = DateTime.Now, PromoEnd = DateTime.Now
            };

            _context.Add(promo);
            return _context.Promos.ToList<Promo>();
        }
    }

}
