using Bode.Plugin.Core.Properties;
using OSharp.Utility.Logging;

namespace Bode.Plugin.Core.IM
{
    public class ImExecutor : IIm
    {
        private readonly IIm _instance;
        private static readonly ILogger Logger = LogManager.GetLogger<ImExecutor>();

        public ImExecutor()
        {
            IImAdapter adapter = ImManager.ImAdapter;
            if (adapter == null)
            {
                Logger.Warn(Resources.IM_NotInitialized);
            }
            else
            {
                _instance = adapter.ImInstance;
            }
        }

        /// <summary>
        /// 注册单个用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="user">提交的用户信息</param>
        public T Regist<T>(object user)
        {
            if (_instance == null) return default(T);

            return _instance.Regist<T>(user);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetUser<T>(string userName)
        {
            if (_instance == null) return default(T);
            return _instance.GetUser<T>(userName);
        }

        /// <summary>
        /// 批量获取用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="limit">显示数量</param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public T GetUsers<T>(int limit, string cursor = "")
        {
            if (_instance == null) return default(T);
            return _instance.GetUsers<T>(limit, cursor);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T DeleteUser<T>(string userName)
        {
            if (_instance == null) return default(T);
            return _instance.DeleteUser<T>(userName);
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
            if (_instance == null) return default(T);
            return _instance.ResetPsw<T>(userName, newPsw);
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
            if (_instance == null) return default(T);
            return _instance.ResetNickName<T>(userName, newNickName);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="owner">要添加好友的用户名</param>
        /// <param name="friend">被添加的用户名</param>
        /// <returns></returns>
        public T AddFriend<T>(string owner, string friend)
        {
            if (_instance == null) return default(T);
            return _instance.AddFriend<T>(owner, friend);
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
            if (_instance == null) return default(T);
            return _instance.RemoveFriend<T>(owner, friend);
        }

        /// <summary>
        /// 获取用户好友信息
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetFriends<T>(string userName)
        {
            if (_instance == null) return default(T);
            return _instance.GetFriends<T>(userName);
        }

        /// <summary>
        /// 获取用户黑名单信息
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetBlockList<T>(string userName)
        {
            if (_instance == null) return default(T);
            return _instance.GetBlockList<T>(userName);
        }

        /// <summary>
        /// 向黑名单中添加用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="blockUsers">需要加入黑名单的用户名数组</param>
        /// <returns></returns>
        public T AddToBlockList<T>(string userName, string[] blockUsers)
        {
            if (_instance == null) return default(T);
            return _instance.AddToBlockList<T>(userName, blockUsers);
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
            if (_instance == null) return default(T);
            return _instance.RemoveFromBlockList<T>(userName, blockedUser);
        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetUsrStatus<T>(string userName)
        {
            if (_instance == null) return default(T);
            return _instance.GetUsrStatus<T>(userName);
        }

        /// <summary>
        /// 获取用户的离线消息数
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public T GetOfflineMsgCount<T>(string userName)
        {
            if (_instance == null) return default(T);
            return _instance.GetOfflineMsgCount<T>(userName);
        }
    }
}
