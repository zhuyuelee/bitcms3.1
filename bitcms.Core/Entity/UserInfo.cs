using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Table("bitcms_user")]
    public class UserInfo : BasicUserInfo
    {
        /// <summary>
        /// Score
        /// </summary>		
        [Column("score")]
        public int Score { get; set; }
        /// <summary>
        /// IP
        /// </summary>		
        [Column("ip")]
        public string IP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Column("lastlanddate")]
        public string LastLandDate { get; set; }

        /// <summary>
        /// 会员过期时间
        /// </summary>
        [Column("deadline")]
        public DateTime? Deadline { get; set; }
        /// <summary>
        /// 来自
        /// </summary>
        [Column("comefrom")]
        public string ComeFrom { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        [Column("follownum")]
        public int FollowNum { get; set; }
    }

    /// <summary>
    /// 用户角色信息
    /// </summary>
    [Table("bitcms_userview")]
    public class UserViewInfo : BasicUserInfo
    {
        /// <summary>
        /// Score
        /// </summary>		
        [Column("score")]
        public int Score { get; set; }
        /// <summary>
        /// IP
        /// </summary>		
        [Column("ip")]
        public string IP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Column("lastlanddate")]
        public string LastLandDate { get; set; }

        /// <summary>
        /// 会员过期时间
        /// </summary>
        [Column("deadline")]
        public DateTime? Deadline { get; set; }
        /// <summary>
        /// 来自
        /// </summary>
        [Column("comefrom")]
        public string ComeFrom { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        [Column("follownum")]
        public int FollowNum { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        [Column("roletype")]
        public string RoleType
        {
            set;
            get;
        }
        /// <summary>
        /// 角色名
        /// </summary>
        [Column("rolename")]
        public string RoleName { get; set; }
    }

    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class BasicUserInfo
    {
        /// <summary>
        /// UserId
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("userid")]
        public int UserId { get; set; }
        /// <summary>
        /// RoleId
        /// </summary>
        [Column("roleId")]
        public int RoleId { get; set; }
        /// <summary>
        /// UserName
        /// </summary>		
        [Column("username")]
        public string UserName { get; set; }
        /// <summary>
        /// Avatar
        /// </summary>		
        [Column("avatar")]
        public string Avatar { get; set; }
        /// <summary>
        /// LockUser
        /// </summary>		
        [Column("lockuser")]
        public int LockUser { get; set; }
        /// <summary>
        /// VerifyMember
        /// </summary>		
        [Column("verifymember")]
        public int VerifyMember { get; set; }
        /// <summary>
        /// 会员推荐
        /// </summary>
        [Column("show")]
        public int Show { get; set; }
        /// <summary>
        /// InDate
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }
        /// <summary>
        /// 性别 0未知 1男性 2女性
        /// </summary>	
        [Column("gender")]
        public int Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Column("birthday")]
        public string BirthDay { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>		
        [Column("cityid")]
        public int CityId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Column("city")]
        public string City { get; set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        [Column("location")]
        public string Location { get; set; }
        /// <summary>
        /// 个人介绍
        /// </summary>
        [Column("introduce")]
        public string Introduce { get; set; }
    }

    /// <summary>
    /// 会员密码
    /// </summary>
    [Table("bitcms_userpassword")]
    public class UserPasswordInfo
    {
        /// <summary>
        /// UserId
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("userid", Order = 1)]
        public int UserId { get; set; }
        /// <summary>
        /// Password
        /// </summary>		
        [Column("password")]
        public string Password { get; set; }
        /// <summary>
        /// 密码类型 0普通用户密码 1管理员密码
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("passwordtype", Order = 2)]
        public passwordType PasswordType { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Column("lastdate")]
        public DateTime LastDate { get; set; }
    }

    /// <summary>
    /// 会员账号绑定
    /// </summary>
    [Table("bitcms_userbind")]
    public class UserBindInfo
    {
        /// <summary>
        /// UserBindId
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("userbindid")]
        public int UserBindId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>		
        [Column("userid")]
        public int UserId { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>		
        [Column("modulecode")]
        public string ModuleCode { get; set; }

        /// <summary>
        /// 绑定会员编码
        /// </summary>
        [Column("usercode")]
        public string UserCode { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column("nickname")]
        public string NickName { get; set; }

        /// <summary>
        /// 认证
        /// </summary>
        [Column("verify")]
        public int Verify { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("indate")]
        public DateTime InDate { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Column("verifycode")]
        public string VerifyCode { get; set; }
        /// <summary>
        /// 扩展字段1
        /// </summary>
        [Column("extend1")]
        public string Extend1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        [Column("extend2")]
        public string Extend2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        [Column("extend3")]
        public string Extend3 { get; set; }
        /// <summary>
        /// 验证码过期时间
        /// </summary>
        [Column("deadline")]
        public DateTime? DeadLine { get; set; }
    }
}
