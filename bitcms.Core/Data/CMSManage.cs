using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitcms.Data
{
    public partial class CMSManage : DataProvider.DataBase
    {
        public CMSManage()
            : base(false)
        {

        }
        /// <summary>
        /// 继承类请使用本构造函数，参数传true
        /// </summary>
        /// <param name="isinherit"></param>
        public CMSManage(bool isinherit)
            : base(isinherit)
        {

        }
    }
}
