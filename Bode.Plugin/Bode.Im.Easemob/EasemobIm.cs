using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using Bode.Plugin.Core.IM;
using OSharp.Utility.Extensions;
using OSharp.Core.Caching;
using OSharp.Utility.Logging;

namespace Bode.Im.Easemob
{
    public class EasemobIm : IIm
    {
        private static readonly ILogger Logger = LogManager.GetLogger<EasemobIm>();
        private static readonly ICache Cache = CacheManager.GetCacher<EasemobIm>();

        private readonly string _clientId, _clientSecret, _url, _token;
        private readonly WebClient _webClient = new WebClient();

        public EasemobIm()
        {
            _clientId = ConfigurationManager.AppSettings["IM-HX-ClientId"];
            _clientSecret = ConfigurationManager.AppSettings["IM-HX-ClientSecret"];
            _url = ConfigurationManager.AppSettings["IM-HX-Url"];

            //获取token
            _token = GetToken();

            //添加请求头
            _webClient.Headers.Add("Authorization", "Bearer " + _token);
            _webClient.Headers.Add("Content-Type", "application/json");
        }

        #region Implementation of IIM

        /// <summary>
        /// 注册单个用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="user">提交的用户信息</param>
        public T Regist<T>(object user)
        {
            try
            {
                var strRep = _webClient.UploadString(_url + "/users", "POST", user.ToJsonString());
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch
            {
                Logger.Info(_token + "_ERROR");
                return default(T);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetUser<T>(string userName)
        {
            try
            {
                var strRep = _webClient.DownloadString(_url + "/users/" + userName);
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 批量获取用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="limit">显示数量</param>
        /// <returns></returns>
        public T GetUsers<T>(int limit, string cursor = "")
        {
            try
            {
                var url = _url + "/users?limit=" + limit;
                if (!string.IsNullOrWhiteSpace(cursor))
                {
                    url += "&cursor=" + cursor;
                }
                var strRep = _webClient.DownloadString(url);
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T DeleteUser<T>(string userName)
        {
            try
            {
                var strRep = _webClient.UploadString(_url + "/users/" + userName, "DELETE", "");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="newPsw">新密码</param>
        /// <returns></returns>
        public T ResetPsw<T>(string userName, string newPsw)
        {
            try
            {
                var data = new { newpassword = newPsw };
                var strRep = _webClient.UploadString(_url + "/users/" + userName + "/password", "PUT", data.ToJsonString());
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 重置昵称
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="newNickName">新昵称</param>
        /// <returns></returns>
        public T ResetNickName<T>(string userName, string newNickName)
        {
            try
            {
                var data = new { nickname = newNickName };
                var strRep = _webClient.UploadString(_url + "/users/" + userName, "PUT", data.ToJsonString());
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 添加好友(互加)
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="owner">要添加好友的用户名</param>
        /// <param name="friend">被添加的用户名</param>
        /// <returns></returns>
        public T AddFriend<T>(string owner, string friend)
        {
            try
            {
                var strRep = _webClient.UploadString(_url + "/users/" + owner + "/contacts/users/" + friend, "POST", "");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="owner">要移除好友的用户名</param>
        /// <param name="friend">被移除的用户名</param>
        /// <returns></returns>
        public T RemoveFriend<T>(string owner, string friend)
        {
            try
            {
                var strRep = _webClient.UploadString(_url + "/users/" + owner + "/contacts/users/" + friend, "DELETE", "");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 获取用户好友信息
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetFriends<T>(string userName)
        {
            try
            {
                var strRep = _webClient.DownloadString(_url + "/users/" + userName + "/contacts/users");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 获取用户黑名单信息
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetBlockList<T>(string userName)
        {
            try
            {
                var strRep = _webClient.DownloadString(_url + "/users/" + userName + "/blocks/users");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 向黑名单中添加用户(单方)
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="blockUsers">需要加入黑名单的用户名数组</param>
        /// <returns></returns>
        public T AddToBlockList<T>(string userName, string[] blockUsers)
        {
            try
            {
                var data = new { usernames = blockUsers };
                var strRep = _webClient.UploadString(_url + "/users/" + userName + "/blocks/users", "POST", data.ToJsonString());
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 在黑名单中移除用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="blockedUser">已拉黑的用户名</param>
        /// <returns></returns>
        public T RemoveFromBlockList<T>(string userName, string blockedUser)
        {
            try
            {
                var strRep = _webClient.UploadString(_url + "/users/" + userName + "/blocks/users/" + blockedUser, "DELETE", "");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetUsrStatus<T>(string userName)
        {
            try
            {
                var strRep = _webClient.DownloadString(_url + "/users/" + userName + "/status");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// 获取用户的离线消息数
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetOfflineMsgCount<T>(string userName)
        {
            try
            {
                var strRep = _webClient.DownloadString(_url + "/users/" + userName + "/offline_msg_count");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        #endregion
        
        private string GetToken() 
        {
            var token = Cache.Get<string>("IM_TOKEN");
            if(string.IsNullOrWhiteSpace(token))
            {
                _webClient.Headers.Add("Content-Type", "application/json");
                var postData = new
                {
                    grant_type = "client_credentials",
                    client_id = _clientId,
                    client_secret = _clientSecret
                };

                var strRep = _webClient.UploadString(_url + "/token", "POST", postData.ToJsonString());
                var o = JsonConvert.DeserializeObject<JObject>(strRep);
                token = o["access_token"].ToString();

                var expDate = DateTime.Now.AddSeconds((double)o["expires_in"]);
                Cache.Set("IM_TOKEN", token, expDate);
            }
            Logger.Info(token);
            return token;
        }
    }
}
