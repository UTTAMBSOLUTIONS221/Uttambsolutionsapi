using DBL;
using DBL.Entities;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Uttambsolutionsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UttambSolutionsEmailController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        public UttambSolutionsEmailController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }

        [HttpPost("Sendnewcustomersubscriptionemail")]
        [AllowAnonymous]
        public async Task<Genericmodel> Sendnewcustomersubscriptionemail(Newcustomersubscription obj)
        {
            return await bl.Sendnewcustomersubscriptionemail(obj);
        }
    }
}
