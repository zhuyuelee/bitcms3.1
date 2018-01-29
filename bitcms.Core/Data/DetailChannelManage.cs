using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取内容频道详情
        /// </summary>
        /// <param name="dictionaryid"></param>
        /// <returns></returns>
        public Entity.DetailChannelInfo getDetailChannelInfo(string code)
        {
            return this.dbContext.DetailChannel.Find(code);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailChannelInfo> getDetailChannelList(int enabled = 0)
        {
            var lambda = PredicateExtensions.True<Entity.DetailChannelInfo>();
            if (enabled == 1)
            {
                lambda = lambda.And(g => g.Enabled == 1);
            }
            return this.dbContext.DetailChannel.Where(lambda).ToList();
        }

        /// <summary>
        /// 更新内容频道
        /// </summary>
        /// <param name="info"></param>
        public bool updateDetailChannel(Entity.DetailChannelInfo info,int adminmenuid, string icon)
        {

            var channelInfo = this.getDetailChannelInfo(info.ChannelCode);
            if (channelInfo == null)
            {
                this.dbContext.DetailChannel.Add(info);

                if (adminmenuid > 0)
                {
                    var keylist = this.getDictionaryKeyList("detailchannel");
                    if (keylist.Count > 0)
                    {
                        var fatherKeyInfo = new Entity.AdminMenuInfo()
                        {
                            MenuName = info.ChannelName,
                            RoleType = "admin",
                            Display = 1,
                            OrderNo = 200,
                            FatherId = adminmenuid,
                            Explain = info.ChannelName,
                            Icon = icon
                        };

                        this.updateAdminMenu(fatherKeyInfo);

                        foreach (var keyInfo in keylist)
                        {
                            this.updateAdminMenu(new Entity.AdminMenuInfo()
                            {
                                MenuName = keyInfo.Title,
                                Area = keyInfo.Value2,
                                Controller = keyInfo.Value3,
                                Parm = "channel=" + info.ChannelCode,
                                RoleType = "admin",
                                Display = 1,
                                OrderNo = 200,
                                FatherId = fatherKeyInfo.AdminMenuId,
                                Explain = keyInfo.Explain,
                                Icon = keyInfo.Value4,
                            });
                        }
                    }
                }
            }
            else
            {
                this.dbContext.Entry<Entity.DetailChannelInfo>(channelInfo).CurrentValues.SetValues(info);
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkDetailChannelCode(string code)
        {
            return this.dbContext.DetailChannel.Count(g => g.ChannelCode.Equals(code)) > 0;
        }
    }
}
