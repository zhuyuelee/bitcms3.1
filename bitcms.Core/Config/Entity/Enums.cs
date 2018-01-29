
namespace bitcms.Entity
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum Error
    {
        系统繁忙 = -1,
        请求成功 = 0,
        错误 = 1,
        登录超时 = 9000,
        无查看权限 = 9001,
        无提交权限 = 9002,
        签名失败 = 4100,
        Domain限制访问 = 5100,
    }

    /// <summary>
    /// 数据库管理类型
    /// </summary>
    public enum ContextType
    {
        /// <summary>
        /// 读模式
        /// </summary>
        read,
        /// <summary>
        /// 写模式
        /// </summary>
        update
    }
    /// <summary>
    /// 广告类型
    /// </summary>
    public enum AdsType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 多媒体
        /// </summary>
        Image,
        /// <summary>
        /// 代码
        /// </summary>
        Code
    }
    /// <summary>
    /// 图库设置
    /// </summary>
    public enum GallerySet
    {
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled,
        /// <summary>
        /// 自定义
        /// </summary>
        Customs,
        /// <summary>
        /// 固定标签
        /// </summary>
        Tags
    }
    /// <summary>
    /// 会员密码类型
    /// </summary>
    public enum passwordType
    {
        /// <summary>
        /// 会员密码
        /// </summary>
        user = 0,
        /// <summary>
        /// 管理员密码
        /// </summary>
        manage = 1
    }
    /// <summary>
    /// 图库类型
    /// </summary>
    public enum GalleryType
    {
        /// <summary>
        /// 图片
        /// </summary>
        picture = 0,
        /// <summary>
        /// 音频
        /// </summary>
        audio = 1,
        /// <summary>
        /// 视频
        /// </summary>
        video = 2,
        /// <summary>
        /// 附件文件
        /// </summary>
        attachment = 3
    }
    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModuleType
    {
        /// <summary>
        /// OAuth授权
        /// </summary>
        OAuth = 0,
        /// <summary>
        /// API调用
        /// </summary>
        API = 10,
        /// <summary>
        /// 账号绑定
        /// </summary>
        UserBind = 20,
    }
}
