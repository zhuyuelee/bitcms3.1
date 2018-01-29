using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dictionaryid"></param>
        /// <returns></returns>
        public Entity.DictionaryInfo getDictionaryInfo(int dictionaryid)
        {
            return this.dbContext.Dictionary.Find(dictionaryid);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dictionaryid"></param>
        /// <returns></returns>
        public Entity.DictionaryInfo getDictionaryInfo(string code)
        {
            return this.dbContext.Dictionary.FirstOrDefault(g => g.DictionaryCode.Equals(code));
        }
        /// <summary>
        /// 获取DictionaryKey详情
        /// </summary>
        /// <param name="keyid"></param>
        /// <returns></returns>
        public Entity.DictionaryKeyInfo getDictionaryKeyInfo(int keyid)
        {
            return this.dbContext.DictionaryKey.Find(keyid);
        }

        /// <summary>
        /// 获取DictionaryKey详情
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Entity.DictionaryKeyInfo getDictionaryKeyInfo(string code, string value)
        {
            return this.dbContext.DictionaryKey.FirstOrDefault(g => g.DictionaryCode.Equals(code) && g.Value.Equals(value));
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DictionaryInfo> getDictionaryList()
        {
            return this.dbContext.Dictionary.ToList();
        }

        /// <summary>
        /// 获取字典key列表 
        /// </summary>
        /// <param name="code">小写</param>
        /// <returns></returns>
        public List<Entity.DictionaryKeyInfo> getDictionaryKeyList(string code, string value2 = null, string value3 = null)
        {
            var lambda = PredicateExtensions.True<Entity.DictionaryKeyInfo>();
            lambda = lambda.And(g => g.DictionaryCode.Equals(code));
            
            if (!string.IsNullOrEmpty(value2))
            {
                lambda = lambda.And(g => g.Value2 == value2);
            }
            if (!string.IsNullOrEmpty(value3))
            {
                lambda = lambda.And(g => g.Value3 == value3);
            }

            return this.dbContext.DictionaryKey.Where(lambda).OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 获取字典key列表 
        /// </summary>
        /// <param name="code">小写</param>
        /// <returns></returns>
        public List<Entity.DictionaryKeyInfo> getDictionaryKeyList(int dictionaryid)
        {
            return this.dbContext.DictionaryKey.Where(g => g.DictionaryId.Equals(dictionaryid)).OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="info"></param>
        public bool updateDictionary(Entity.DictionaryInfo info)
        {
           
            var updateInfo = this.getDictionaryInfo(info.DictionaryId);
            if (updateInfo == null)
            {
                this.dbContext.Dictionary.Add(info);
            }
            else
            {
                this.dbContext.Entry<Entity.DictionaryInfo>(updateInfo).CurrentValues.SetValues(info);
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新字典key
        /// </summary>
        /// <param name="info"></param>
        public bool updateDictionaryKey(Entity.DictionaryKeyInfo info)
        {
            var updateInfo = this.dbContext.DictionaryKey.Find(info.KeyId);
            if (info.DictionaryId > 0)
            {
                var dictionaryInfo = this.getDictionaryInfo(info.DictionaryId);
                if (dictionaryInfo != null)
                {
                    info.DictionaryCode = dictionaryInfo.DictionaryCode;
                }
            }

            if (updateInfo == null)
            {
                this.dbContext.DictionaryKey.Add(info);
            }
            else
            {
                this.dbContext.Entry<Entity.DictionaryKeyInfo>(updateInfo).CurrentValues.SetValues(info);

            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkDictionaryCode(string field, int dictionaryid)
        {
            return this.dbContext.Dictionary.Count(g => g.DictionaryCode.Equals(field) && g.DictionaryId != dictionaryid) <= 0;
        }
        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkDictionaryKey(int keyid, string code, string value)
        {
            var info = this.dbContext.DictionaryKey.FirstOrDefault(g => g.KeyId != keyid && g.DictionaryCode.Equals(code) && g.Value.Equals(value));
            return info == null;
        }
        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="dictionaryid"></param>
        public void deleteDictionary(int dictionaryid)
        {
            var info = this.getDictionaryInfo(dictionaryid);
            if (info != null)
            {
                //删除key
                this.dbContext.DictionaryKey.RemoveRange(getDictionaryKeyList(dictionaryid));
                //删除字典
                this.dbContext.Dictionary.Remove(info);

                this.dbContext.SaveChanges();
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "要删除的字典不存在！";
            }
        }

        /// <summary>
        /// 删除字典key
        /// </summary>
        /// <param name="id"></param>
        public void deleteDictionaryKey(int id)
        {
            var info = this.getDictionaryKeyInfo(id);
            if (info != null)
            {
                //删除key
                this.dbContext.DictionaryKey.Remove(info);

                this.dbContext.SaveChanges();
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "字典不存在！";
            }
        }
    }
}
