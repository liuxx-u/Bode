using System;
using System.Collections.Concurrent;
using System.Linq;
using OSharp.Core.Caching;

namespace OSharp.Web.Http.Caching
{
    public class OnlineUserStore : IOnlineUserStore
    {
        private readonly TimeSpan _validityPeriod = TimeSpan.FromDays(30);
        private readonly ICache _onlineUserCache = CacheManager.GetCacher("OnlineUser");


        public bool IsOnline(string key)
        {
            return _onlineUserCache.Get(key) != null;
        }

        public void ResetLastOperationTime(string key)
        {
            _onlineUserCache.Set(key, DateTime.Now, _validityPeriod);
        }
    }
}
