using Microsoft.AspNetCore.Http;
using System;

namespace AenEnterprise.DomainModel.CookieStorage
{
    public class CookieImplementation : ICookieImplementation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieImplementation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(1);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }

        public void SetForInvoice(string key, string value, int? expireTime, bool secure, bool httpOnly)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(1);

            option.Secure = secure;
            option.HttpOnly = httpOnly;

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }

    }
}
