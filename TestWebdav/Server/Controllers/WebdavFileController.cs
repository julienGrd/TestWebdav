using Microsoft.AspNetCore.Mvc;
using TestWebdav.Shared;

namespace TestWebdav.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebdavFileController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WebdavFileController> _logger;
        private readonly IHostEnvironment _hostEnv;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebdavFileController(ILogger<WebdavFileController> logger, IHostEnvironment pHostEnv, IHttpContextAccessor pHttpContextAccessor)
        {
            _logger = logger;
            _hostEnv = pHostEnv;
            _httpContextAccessor = pHttpContextAccessor;
        }

        [HttpGet]
        public IEnumerable<WebdavFileModel> Get()
        {
            var lDirectoryWebdav = Path.Combine(_hostEnv.ContentRootPath, "WebdavRepository");
            if (Directory.Exists(lDirectoryWebdav))
            {
                foreach(var f in System.IO.Directory.GetFiles(lDirectoryWebdav))
                {
                    var lFileName = System.IO.Path.GetFileName(f);
                    var lUrl = $"ms-word:ofe|u|{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/Webdav/{lFileName}";

                    yield return new WebdavFileModel(lFileName, lUrl);
                }
            }
        }
    }
}