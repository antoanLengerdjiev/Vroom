using Microsoft.AspNetCore.Http;
using Vroom.Providers.Contracts;
using System.Security.Claims;
using System;
using Bytes2you.Validation;
using Vroom.Common;

namespace Vroom.Providers
{
    public class HttpContextProvider : IHttpContextProvider
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextProvider(IHttpContextAccessor _httpContextAccessor)
        {
            Guard.WhenArgument<IHttpContextAccessor>(_httpContextAccessor, GlobalConstants.GetMemberName(() => _httpContextAccessor)).IsNull().Throw();
            this._httpContextAccessor = _httpContextAccessor;
        }

        public HttpContextProvider()
        {
        }

        public string GetCurrentUsedId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetLoginUserName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public bool IsAuthenticated()
        {
            return this._httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IFormFileCollection GetPostedFormFiles()
        {
            return this._httpContextAccessor.HttpContext.Request.Form.Files;
        }

        public bool IsAjaxRequest()
        {
            var request = this._httpContextAccessor.HttpContext.Request;

            Guard.WhenArgument<HttpRequest>(request, GlobalConstants.GetMemberName(() => request)).IsNull().Throw();

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }
    }
}
