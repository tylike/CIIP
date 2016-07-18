using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

namespace CIIP.FormCode.DatabaseUpdate
{
    public class DataInit
    {
        private IObjectSpace ObjectSpace;
        public DataInit(IObjectSpace objectSpace)
        {
            this.ObjectSpace = objectSpace;
        }

        public void 创建取得编号方法()
        {
            var os = this.ObjectSpace as XPObjectSpace;
            if (os != null)
            {
                var str = string.Format("\r\nEXEC(\"\r\nif not exists(select * from syscolumns where id=object_id(N'单据编号方案') and name=N'序号') \r\nbegin\r\nALTER TABLE dbo.单据编号方案 ADD 序号 bigint NOT NULL CONSTRAINT DF_单据编号方案_序号 DEFAULT 0\r\nend\");\r\nEXEC(\"\r\nIF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAutoNumber]') AND type in (N'P', N'PC'))\r\nDROP PROCEDURE [dbo].[GetAutoNumber]\");\r\nEXEC(\"\r\n-- =============================================\r\n-- Author:\t\t旗舰软件工作室,陈伟\r\n-- Create date: 系统自动生成,{0}\r\n-- Description:\tQQ4603528,Tel:13482559973\r\n-- =============================================\r\nCREATE PROCEDURE GetAutoNumber @TypeName nvarchar(max)\r\nAS\r\nBEGIN\r\n\tSET NOCOUNT ON;\r\n\tbegin tran t1;\r\n\tupdate 单据编号方案 set 序号 = isnull(序号,0)+1 where 应用单据=@TypeName;\r\n\tselect 序号 from 单据编号方案 where 应用单据=@TypeName;\r\n\tcommit tran t1;\r\n\t\r\nEND\");\r\n", DateTime.Now.ToString(), "单据编号方案").Replace("'", "''").Replace("\"", "'");
                os.Session.ExecuteNonQuery(str);
            }
            else
            {
                throw new Exception("ObjectSpace错误，创建取得编号的存储过程没有成功！");
            }
        }

    }
}