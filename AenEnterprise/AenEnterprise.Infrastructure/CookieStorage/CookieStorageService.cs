using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace AenEnterprise.Infrastructure.CookieStorage
{
    public class CookieStorageService:ICookieStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieStorageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Set(string key, string value, int? expireTime)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var option = new CookieOptions();
                if (expireTime.HasValue)
                    option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                else
                    option.Expires = DateTime.Now.AddDays(1);

                _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
            }
        }


        public string Get(HttpRequest request, string key)
        {
            return request.Cookies[key];
        }

        public void Remove(HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }

        
    }
}
