using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Uttambsolutionsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModulesController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        public ModulesController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }
        #region System Modules
        [HttpGet("Getsystemmoduledata")]
        public async Task<IEnumerable<Systemmodule>> Getsystemmoduledata()
        {
            return await bl.Getsystemmoduledata();
        }
        #endregion
    }
}
