using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.Infrastructure.CookieStorage
{
    public interface ICookieStorageService
    {
        void Set(string key, string value, int? expireTime);
        string Get(HttpRequest request, string key);
        void Remove(HttpResponse response, string key); 
    }
}
