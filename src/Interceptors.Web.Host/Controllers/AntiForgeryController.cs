using Microsoft.AspNetCore.Antiforgery;
using Interceptors.Controllers;

namespace Interceptors.Web.Host.Controllers
{
    public class AntiForgeryController : InterceptorsControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
