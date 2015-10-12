using System;
using OSharp.Core.Dependency;

namespace OSharp.Web.Http.Caching
{
    public interface IOnlineUserStore : ISingletonDependency
    {
        bool IsOnline(string key);

        void ResetLastOperationTime(string key);
    }
}
