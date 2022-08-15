using Microsoft.AspNetCore.Http;

namespace Vroom.Providers.Contracts
{
    public interface IHttpContextProvider
    {
        string GetLoginUserName();

        string GetCurrentUsedId();

        bool IsAuthenticated();

        bool IsAjaxRequest();
        IFormFileCollection GetPostedFormFiles();
    }
}