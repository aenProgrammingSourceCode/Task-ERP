using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.CookieStorage
{
    public interface ICookieImplementation
    {
        void Set(string key, string value, int? expireTime);
        void SetForInvoice(string key, string value, int? expireTime,bool secure,bool httpOnly);
        string Get(string key);
        void Remove(string key);
    }
}
