using System;
using OSharp.Core.Caching;
using OSharp.Core.Context;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;
using OSharp.Web.Http.Caching;

namespace OSharp.Web.Http.Authentication
{
    public class TokenAuthenticationHandler : TokenAuthenticationHandlerBase
    {
        private const string AuthDesKey = "bodeauth";
        private readonly IOnlineUserStore _onlineUserStore;

        public TokenAuthenticationHandler(IOnlineUserStore onlineUserStore)
        {
            _onlineUserStore = onlineUserStore;
        }


        protected override bool Authorize(string authenticationToken)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(authenticationToken))
                {
                    var strAuth = DesHelper.Decrypt(authenticationToken, AuthDesKey);
                    Operator user = strAuth.FromJsonString<Operator>() ?? new Operator();

                    if (!user.UserName.IsNullOrWhiteSpace() && _onlineUserStore.IsOnline(user.UserName))
                    {
                        OSharpContext.Current.SetOperator(user);
                        _onlineUserStore.ResetLastOperationTime(user.UserName);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
