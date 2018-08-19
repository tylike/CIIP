using System;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using CIIP.FormCode;
using System.Drawing;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;
using CIIP.Module.BusinessObjects;
using CIIP.Module.BusinessObjects.Security;
using CIIP.Module.BusinessObjects.SYS;

namespace CIIP.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion)
        {
        }

        //public 员工 CreateDefaultEmployee(系统用户 user)
        //{
        //    var emp = ObjectSpace.CreateObject<员工>();

        //    var company = ObjectSpace.CreateObject<往来单位>();
        //    emp.名称 = "管理员";
        //    emp.手机 = "13660088006";

        //    company.客户 = true;

        //    company.供应商 = true;

        //    company.名称 = "京东";

        //    company.联系人.Add(emp);
        //    user.员工 = emp;
        //    emp.系统用户 = user;

        //    var add = ObjectSpace.CreateObject<地址>();
        //    add.详细地址 = "上海市黄浦区";

        //    company.地址.Add(add);
        //    var tdch = new TestDataGeneratorHelper();
        //    var employees = tdch.GetRandomNames(30);
        //    foreach (var em in employees)
        //    {
        //        var e = ObjectSpace.CreateObject<员工>();
        //        e.名称 = em;
        //        e.手机 = "13866559988";
        //        company.联系人.Add(e);
        //    }
        //    return emp;
        //}

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            var sampleUser = ObjectSpace.FindObject<系统用户>(new BinaryOperator("UserName", "User"));
            if (sampleUser == null)
            {
                sampleUser = ObjectSpace.CreateObject<系统用户>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }

            var defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            var userAdmin = ObjectSpace.FindObject<系统用户>(new BinaryOperator("UserName", "Admin"));
            if (userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<系统用户>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
                //CreateDefaultEmployee(userAdmin);
            }

            // If a role with the Administrators name doesn't exist in the database, create this role
            var adminRole = ObjectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<SecuritySystemRole>();
                adminRole.Name = "Administrators";
            }


            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);



            //var inited = ObjectSpace.GetObjectsCount(typeof (产品), null) > 0;
            //var tdch = new TestDataGeneratorHelper();
            //if (!inited)
            //{
            //    var t = CreateUnit("台");

            //    var g = CreateUnit("个");

            //    CreateProduct("Surface 3 64G 2G", t, 2000, 2999);

            //    CreateProduct("Surface 3 128G 4G", t, 2000, 4888);

            //    CreateProduct("Surface Pro 3 I3 64G 4G", t, 3000, 4499);

            //    CreateProduct("Surface Pro 3 I5 128G 4G", t, 4500, 5699);

            //    CreateProduct("Surface Pro 3 I5 256G 8G", t, 6388, 6999);

            //    CreateProduct("Surface Pro 3 I7 256G 8G", t, 7000, 8888);

            //    CreateProduct("Surface Pro 3 I7 512G 8G", t, 8000, 9999);

            //    CreateProduct("Surface Pro 4 CoreM 128G 8G", t, 5688, 6688);

            //    CreateProduct("Surface Pro 4 I5 128G 8G", t, 6388, 7388);

            //    CreateProduct("Surface Pro 4 I5 256G 8G", t, 6388, 9688);

            //    CreateProduct("Surface Pro 4 I7 256G 16G", t, 6388, 13388);

            //    CreateProduct("Surface Notebook I5 128G 8G", t, 10000, 11088);

            //    CreateProduct("Surface Notebook I5 256G 16G", t, 10000, 14088);

            //    CreateProduct("Surface Notebook I7 256G 16G", t, 13000, 15588);

            //    CreateProduct("Surface Notebook I7 512G 16G", t, 15000, 20088);
                
            //    var microsoft = CreateCompany("Microsoft", "比尔.盖茨", "13800001111", "徐家汇", true, true);

            //    var google = CreateCompany("Google", "拉里·佩奇", "13988881111", "徐家汇", true, true);

            //    var baidu = CreateCompany("Baidu", "李彦宏", "13900001111", "陆家嘴", true, true);

            //    var tencent = CreateCompany("Tencent", "15912111211", "马化腾", "陆家嘴", true, true);

            //    var ali = CreateCompany("Alibaba", "15612121212", "马云", "陆家嘴", true, true);

            //    //500个客户
               
               
            //}
            
            //var area = ObjectSpace.FindObject<省份>(null);
            //if (area == null)
            //{
            //    var js = CreateSF("江苏");
            //    CreateCity(js, "苏州", "姑苏区,相城区,吴中区,虎丘区,工业园区,吴江区,张家港市,常熟市,太仓巿,昆山市");
            //    CreateCity(js, "南京", "鼓楼区,白下区,玄武区,秦淮区,建邺区,下关区,雨花台区,栖霞区,高淳县,溧水县,六合区,浦口区,江宁区");

            //    var tjs = CreateSF("天津");
            //    CreateCity(tjs, "天津", "和平区,河东区,河西区,南开区,河北区,红桥区,滨海新区,东丽区,西青区,津南区,北辰区,武清区,宝坻区,宁河区,静海区,蓟县");


            //    var cqs = CreateSF("重庆");
            //    CreateCity(cqs, "重庆",
            //        "渝中区,大渡口区,江北区,沙坪坝区,九龙坡区,南岸区,北碚区,渝北区,巴南区,涪陵区,綦江区,大足区,长寿区,江津区,合川区,永川区,南川区,璧山区,铜梁区,潼南区,荣昌区,万州区,梁平县,城口县,丰都县,垫江县,忠县,开县,云阳县,奉节县,巫山县,巫溪县,黔江区,武隆县,石柱土家族自治县,秀山土家族苗族自治县,酉阳土家族苗族自治县,彭水苗族土家族自治县");

            //    var zjs = CreateSF("浙江");
            //    CreateCity(zjs, "杭州", "市区,上城区,下城区,江干区,拱墅区,西湖区,滨江区,萧山区,余杭区,富阳区,桐庐县,淳安县,建德市,临安市");

            //    var scs = CreateSF("四川");
            //    CreateCity(scs, "成都", "武侯区,锦江区,青羊区,金牛区,成华区,龙泉驿区,温江区,新都区,青白江区,双流区,郫县,蒲江县,大邑县,金堂县,新津县,都江堰市,彭州市,邛崃市,崇州市");

            //    var gds = CreateSF("广东");
            //    CreateCity(gds, "广州", "越秀区,荔湾区,海珠区,天河区,白云区,黄埔区,番禺区,花都区,南沙区,增城区,从化区");

            //    var szs = CreateSF("深圳");
            //    CreateCity(szs, "深圳", "福田区,罗湖区,南山区,盐田区,宝安区,龙岗区");

            //    var shs = CreateSF("上海");
            //    CreateCity(shs, "上海", "黄浦区,浦东新区,徐汇区,长宁区,静安区,普陀区,虹口区,杨浦区,闵行区,宝山区,嘉定区,金山区,松江区,青浦区,奉贤区,崇明县");

            //    var bjs = CreateSF("北京");
            //    CreateCity(bjs, "北京", "东城区,西城区,海淀区,朝阳区,丰台区,石景山区,门头沟区,通州区,顺义区,房山区,大兴区,昌平区,怀柔区,平谷区,密云区,延庆区");
            //    ObjectSpace.CommitChanges();
            //    var s = ObjectSpace.GetObjects<销售区域>();

            //    var customers = tdch.GetRandomNames(500);
            //    var rnd = new Random();
            //    foreach (var customer in customers)
            //    {
            //        CreateCompany(customer, customer, "13900005555", "中国", false, true, s[rnd.Next(s.Count - 1)]);
            //    }
            //}


            //var dd = ObjectSpace.FindObject<日期维度>(null);
            //if (dd == null)
            //{
            //    for (var x = DateTime.Parse("2013-1-1"); x <= DateTime.Parse("2017-12-31"); x = x.AddDays(1))
            //    {
            //        var t = ObjectSpace.CreateObject<日期维度>();
            //        t.TheDate = x;
            //    }
            //}
            //var solutions = ObjectSpace.GetObjects<单据编号方案>();
            
            //var froms = XafTypesInfo.Instance.FindTypeInfo(typeof(单据)).Descendants.Where(x => x.IsPersistent);
            //foreach (var item in froms)
            //{
            //    //需要看看为啥为空
            //    var solution = solutions.FirstOrDefault(x => x.应用单据!=null && x.应用单据.FullName == item.FullName);
            //    if (solution == null)
            //    {
            //        solution = ObjectSpace.CreateObject<单据编号方案>();
            //        solution.应用单据 = item.Type;
            //        solution.名称 = item.Name + "编号方案";
            //        var item1 = ObjectSpace.CreateObject<单据编号当前日期规则>();
            //        item1.格式化字符串 = "yyyyMMddHHmmssfff";
            //        solution.编号规则.Add(item1);
            //    }
                
            //}

            var nameSpace = ObjectSpace.FindObject<Namespace>(new BinaryOperator("FullName", "系统类型"));
            if (nameSpace == null)
            {
                nameSpace = ObjectSpace.CreateObject<Namespace>();
                nameSpace.名称 = "系统类型";
                var createIndex = ObjectSpace.GetObjectsCount(typeof(BusinessObject), null);

                var strType = AddSimpleObject(typeof(string), "字符串", nameSpace, "一串文本内容");

                var dec = AddSimpleObject(typeof(decimal), "数字(decimal)", nameSpace, "通常用于记录带小数点的数字,比如表示货币金额时");
                var intt = AddSimpleObject(typeof(int), "整数(int)", nameSpace, "没有小数的数字");
                var datetimeType = AddSimpleObject(typeof(DateTime), "日期时间", nameSpace, "用于表示日期时间");
                var boolType = AddSimpleObject(typeof(bool), "布尔", nameSpace, "用于表示是/否,真/假,等2种可选值的情况");
                var imageType = AddSimpleObject(typeof(Image), "图片", nameSpace, "用于表示图像信息");
                var colorType = AddSimpleObject(typeof(Color), "颜色", nameSpace, "用于表示颜色信息");
                AddSimpleObject(typeof(sbyte), "整数(sbyte 8位有符号)", nameSpace, "8位有符号整数");
                AddSimpleObject(typeof(long), "整数(long 64位有符号)", nameSpace, "");
                AddSimpleObject(typeof(byte), "整数(byte 8位无符号)", nameSpace, "");
                AddSimpleObject(typeof(ushort), "整数(ushort 16位无符号)", nameSpace, "");
                AddSimpleObject(typeof(uint), "整数(uint 32位无符号)", nameSpace, "");
                AddSimpleObject(typeof(ulong), "整数(ulong 64位无符号)", nameSpace, "");
                AddSimpleObject(typeof(float), "数字(float 单精度浮点)", nameSpace, "");
                AddSimpleObject(typeof(double), "数字(double 双精度浮点)", nameSpace, "");
                AddSimpleObject(typeof(TimeSpan), "时间段(TimeSpan)", nameSpace, "");
                AddSimpleObject(typeof(Guid), "Guid", nameSpace, "全球唯一标识");
            }

            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ERPModule.IsNewVersion = true;
        }

        public SimpleType AddSimpleObject(Type type, string caption, Namespace nameSpace, string description)
        {
            var t = ObjectSpace.FindObject<SimpleType>(new BinaryOperator("FullName", type.FullName));
            if (t == null)
            {
                t = ObjectSpace.CreateObject<SimpleType>();
                t.名称 = type.Name;
                t.Caption = caption;
                t.Category = nameSpace;
                t.Description = description;
                t.FullName = type.FullName;
            }
            return t;
        }

        //private void CreateCity(省份 sf,string cityName,string sxq)
        //{
        //    var city = ObjectSpace.CreateObject<城市>();
        //    city.名称 = cityName;
        //    city.省份 = sf;
        //    var sq = sxq.Split(',');
        //    foreach (var s in sq)
        //    {
        //        var sqo = ObjectSpace.CreateObject<销售区域>();
        //        sqo.名称 = s;
        //        sqo.城市 = city;
        //    }
        //}

        //private 省份 CreateSF(string name)
        //{
        //    var bjs = ObjectSpace.CreateObject<省份>();
        //    bjs.名称 = name;
        //    return bjs;
        //}

        //public 往来单位 CreateCompany(string name, string contact, string telephone, string address, bool provider, bool customer,销售区域 area = null)
        //{
        //    var company = ObjectSpace.CreateObject<往来单位>();
        //    company.供应商 = true;
        //    company.名称 = name;
        //    company.客户 = customer;
        //    company.供应商 = provider;
        //    company.销售区域 = area;

        //    var con = ObjectSpace.CreateObject<员工>();
        //    con.名称 = contact;
        //    con.手机 = telephone;
        //    var add = ObjectSpace.CreateObject<地址>();
        //    add.名称 = address;

        //    company.联系人.Add(con);
        //    company.地址.Add(add);
        //    return company;
        //}

        //产品 CreateProduct(string pname, 计量单位 defaultUnit, decimal buy, decimal sale)
        //{
        //    var p = ObjectSpace.CreateObject<产品>();
        //    p.名称 = pname;
        //    p.进货价格 = buy;
        //    p.零售价 = sale;
        //    p.默认单位 = defaultUnit;
        //    return p;
        //}

        //private 计量单位 CreateUnit(string name)
        //{
        //    var unit = ObjectSpace.CreateObject<计量单位>();
        //    unit.名称 = name;
        //    return unit;
        //}
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private SecuritySystemRole CreateDefaultRole() {
            SecuritySystemRole defaultRole = ObjectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<SecuritySystemRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectAccessPermission<SecuritySystemUser>("[Oid] = CurrentUserId()", SecurityOperations.ReadOnlyAccess);
                defaultRole.AddMemberAccessPermission<SecuritySystemUser>("ChangePasswordOnFirstLogon", SecurityOperations.Write, "[Oid] = CurrentUserId()");
                defaultRole.AddMemberAccessPermission<SecuritySystemUser>("StoredPassword", SecurityOperations.Write, "[Oid] = CurrentUserId()");
                defaultRole.SetTypePermissionsRecursively<SecuritySystemRole>(SecurityOperations.Read, SecuritySystemModifier.Allow);
                defaultRole.SetTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecuritySystemModifier.Allow);
                defaultRole.SetTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecuritySystemModifier.Allow);
            }
            return defaultRole;
        }
    }
}
