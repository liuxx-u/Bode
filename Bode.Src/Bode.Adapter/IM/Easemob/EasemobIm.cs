using System;
using Newtonsoft.Json;
using OSharp.Utility.IM;
using Newtonsoft.Json.Linq;
using System.Configuration;
using OSharp.Utility.Extensions;

namespace Bode.Adapter.IM.Easemob
{
    public class EasemobIm : IIm
    {
        static readonly string CLIENT_ID, CLIENT_SECRET, URL;

        static string TOKEN = "YWMtraNPTvOgEeS_TpkRaPjQuwAAAU5cRrsPyiXNLIUA93kChdW3TegvmFJoCR8";
        static DateTime TOKEN_VALIDITY;
        static readonly object LockObj = new object();

        static EasemobIm()
        {
            TOKEN_VALIDITY = DateTime.Now.AddMonths(2);
            CLIENT_ID = ConfigurationManager.AppSettings["HX-ClientId"];
            CLIENT_SECRET = ConfigurationManager.AppSettings["HX-ClientSecret"];
            URL = ConfigurationManager.AppSettings["HX-Url"];
        }

        #region Implementation of IIM

        /// <summary>
        /// 注册单个用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="user">提交的用户信息</param>
        public T Regist<T>(object user)
        {
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.UploadString(URL + "/users", "POST", user.ToJsonString());
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch
            {
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);
                var strRep = client.DownloadString(URL + "/users/" + userName);
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
            CheckToken();

            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var url = URL + "/users?limit=" + limit;
                if (!string.IsNullOrWhiteSpace(cursor))
                {
                    url += "&cursor=" + cursor;
                }
                var strRep = client.DownloadString(url);
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.UploadString(URL + "/users/" + userName, "DELETE", "");
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var data = new { newpassword = newPsw };
                var strRep = client.UploadString(URL + "/users/" + userName + "/password", "PUT", data.ToJsonString());
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var data = new { nickname = newNickName };
                var strRep = client.UploadString(URL + "/users/" + userName, "PUT", data.ToJsonString());
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
            CheckToken();

            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.UploadString(URL + "/users/" + owner + "/contacts/users/" + friend, "POST", "");
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
            CheckToken();

            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.UploadString(URL + "/users/" + owner + "/contacts/users/" + friend, "DELETE", "");
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.DownloadString(URL + "/users/" + userName + "/contacts/users");
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.DownloadString(URL + "/users/" + userName + "/blocks/users");
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var data = new { usernames = blockUsers };
                var strRep = client.UploadString(URL + "/users/" + userName + "/blocks/users", "POST", data.ToJsonString());
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.UploadString(URL + "/users/" + userName + "/blocks/users/" + blockedUser, "DELETE", "");
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);
                client.Headers.Add("Content-Type", "application/json");

                var strRep = client.DownloadString(URL + "/users/" + userName + "/status");
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
            CheckToken();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add("Authorization", "Bearer " + TOKEN);

                var strRep = client.DownloadString(URL + "/users/" + userName + "/offline_msg_count");
                return JsonConvert.DeserializeObject<T>(strRep);
            }
            catch { return default(T); }
        }

        #endregion

        private void CheckToken()
        {
            lock (LockObj)
            {
                if (string.IsNullOrWhiteSpace(TOKEN) || TOKEN_VALIDITY <= DateTime.Now)
                {
                    var client = new System.Net.WebClient();
                    client.Headers.Add("Content-Type", "application/json");
                    var postData = new
                    {
                        grant_type = "client_credentials",
                        client_id = CLIENT_ID,
                        client_secret = CLIENT_SECRET
                    };

                    var strRep = client.UploadString(URL + "/token", "POST", postData.ToJsonString());
                    var o = JsonConvert.DeserializeObject<JObject>(strRep);
                    TOKEN = o["access_token"].ToString();
                    TOKEN_VALIDITY = DateTime.Now.AddSeconds((double)o["expires_in"]);
                }
            }
        }
    }
}
