using Microsoft.AspNetCore.Http;
using System.Linq;

namespace IQVIA.WebApp.Extensions
{
    public static class RequestExtensions
    {
        private const string AjaxKey = "X-Requested-With";
        private const string AjaxValues = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            var query = request.Query;
            if (query != null && query[AjaxKey] == AjaxValues)
            {
                return true;
            }

            var headers = request.Headers;
            if (headers != null && headers[AjaxKey] == AjaxValues)
            {
                return true;
            }

            return false;
        }

        public static string RemoteAddress(this HttpRequest request)
        {
            return request.Headers["HTTP_REMOTE_ADDR"].FirstOrDefault() ?? request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
