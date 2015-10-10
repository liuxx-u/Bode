
namespace OSharp.Utility.IM
{
    public abstract class ImBase : IIm
    {
        /// <summary>
        /// 注册单个用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="user">提交的用户信息</param>
        public abstract T Regist<T>(object user);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public abstract T GetUser<T>(string userName);

        /// <summary>
        /// 批量获取用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="limit">显示数量</param>
        /// <returns></returns>
        public abstract T GetUsers<T>(int limit, string cursor = "");

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public abstract T DeleteUser<T>(string userName);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="newPsw">新密码</param>
        /// <returns></returns>
        public abstract T ResetPsw<T>(string userName, string newPsw);

        /// <summary>
        /// 重置昵称
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="newNickName">新昵称</param>
        /// <returns></returns>
        public abstract T ResetNickName<T>(string userName, string newNickName);

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="owner">要添加好友的用户名</param>
        /// <param name="friend">被添加的用户名</param>
        /// <returns></returns>
        public abstract T AddFriend<T>(string owner, string friend);

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="owner">要移除好友的用户名</param>
        /// <param name="friend">被移除的用户名</param>
        /// <returns></returns>
        public abstract T RemoveFriend<T>(string owner, string friend);

        /// <summary>
        /// 获取用户好友信息
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public abstract T GetFriends<T>(string userName);

        /// <summary>
        /// 获取用户黑名单信息
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public abstract T GetBlockList<T>(string userName);

        /// <summary>
        /// 向黑名单中添加用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="blockUsers">需要加入黑名单的用户名数组</param>
        /// <returns></returns>
        public abstract T AddToBlockList<T>(string userName, string[] blockUsers);

        /// <summary>
        /// 在黑名单中移除用户
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <param name="blockedUser">已拉黑的用户名</param>
        /// <returns></returns>
        public abstract T RemoveFromBlockList<T>(string userName, string blockedUser);

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public abstract T GetUsrStatus<T>(string userName);

        /// <summary>
        /// 获取用户的离线消息数
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public abstract T GetOfflineMsgCount<T>(string userName);
    }
}
