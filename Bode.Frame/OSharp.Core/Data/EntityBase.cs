// -----------------------------------------------------------------------
//  <copyright file="EntityBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-02-26 18:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSharp.Core.Data
{
    /// <summary>
    /// 可持久化到数据库的数据模型基类
    /// </summary>
    /// <typeparam name="TKey">主键数据类型</typeparam>
    public abstract class EntityBase<TKey> : IEntity<TKey>, IRecyclable, ICreatedTime, ITimestamp
    {
        /// <summary>
        /// 初始化创建时间
        /// </summary>
        protected EntityBase()
        {
            CreatedTime = DateTime.Now;
        }

        #region 属性

        /// <summary>
        /// 获取或设置 实体唯一标识，主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName(@"主键")]
        public TKey Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 版本控制标识，用于处理并发
        /// </summary>
        [ConcurrencyCheck]
        [Timestamp]
        public byte[] Timestamp { get; set; }

        /// <summary>
        /// 获取或设置 是否已逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 判断两个实体是否是同一数据记录的实体
        /// </summary>
        /// <param name="obj">要比较的实体信息</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            EntityBase<TKey> entity = obj as EntityBase<TKey>;
            if (entity == null)
            {
                return false;
            }
            return entity.Id.Equals(Id);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>
        /// 当前 <see cref="T:System.Object"/> 的哈希代码。<br/>
        /// 如果<c>Id</c>为<c>null</c>则返回0，
        /// 如果不为<c>null</c>则返回<c>Id</c>对应的哈希值
        /// </returns>
        public override int GetHashCode()
        {
            if (Id == null)
            {
                return 0;
            }
            return Id.ToString().GetHashCode();
        }

        #endregion
    }
}