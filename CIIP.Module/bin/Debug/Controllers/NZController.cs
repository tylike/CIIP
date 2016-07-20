
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using 基础信息.产品;
using 基础信息;
using 基础信息.地理;
using 基础信息.往来单位;

namespace Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InitWMSDataController : WindowController
    {
        private System.ComponentModel.IContainer components = null;
        IObjectSpace ObjectSpace;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public InitWMSDataController()
        {
            this.components = new System.ComponentModel.Container();
            this.TargetWindowType = WindowType.Main;
            var initData = new SimpleAction(this.components);
            initData.Category = "Tools";
            initData.Caption = "生成默认数据";

            initData.Id = "InitData";
            initData.Execute += (snd, e) => {

                ObjectSpace = Application.CreateObjectSpace();

                var inited = ObjectSpace.GetObjectsCount(typeof(产品), null) > 0;
                var tdch = new CIIP.Module.DatabaseUpdate.TestDataGeneratorHelper();
                if (!inited)
                {
                    var t = CreateUnit("台");

                    var g = CreateUnit("个");

                    CreateProduct("Surface 3 64G 2G", t, 2000, 2999);

                    CreateProduct("Surface 3 128G 4G", t, 2000, 4888);

                    CreateProduct("Surface Pro 3 I3 64G 4G", t, 3000, 4499);

                    CreateProduct("Surface Pro 3 I5 128G 4G", t, 4500, 5699);

                    CreateProduct("Surface Pro 3 I5 256G 8G", t, 6388, 6999);

                    CreateProduct("Surface Pro 3 I7 256G 8G", t, 7000, 8888);

                    CreateProduct("Surface Pro 3 I7 512G 8G", t, 8000, 9999);

                    CreateProduct("Surface Pro 4 CoreM 128G 8G", t, 5688, 6688);

                    CreateProduct("Surface Pro 4 I5 128G 8G", t, 6388, 7388);

                    CreateProduct("Surface Pro 4 I5 256G 8G", t, 6388, 9688);

                    CreateProduct("Surface Pro 4 I7 256G 16G", t, 6388, 13388);

                    CreateProduct("Surface Notebook I5 128G 8G", t, 10000, 11088);

                    CreateProduct("Surface Notebook I5 256G 16G", t, 10000, 14088);

                    CreateProduct("Surface Notebook I7 256G 16G", t, 13000, 15588);

                    CreateProduct("Surface Notebook I7 512G 16G", t, 15000, 20088);

                    var microsoft = CreateCompany("Microsoft", "比尔.盖茨", "13800001111", "徐家汇", true, true);

                    var google = CreateCompany("Google", "拉里·佩奇", "13988881111", "徐家汇", true, true);

                    var baidu = CreateCompany("Baidu", "李彦宏", "13900001111", "陆家嘴", true, true);

                    var tencent = CreateCompany("Tencent", "15912111211", "马化腾", "陆家嘴", true, true);

                    var ali = CreateCompany("Alibaba", "15612121212", "马云", "陆家嘴", true, true);

                    //500个客户


                }

                var area = ObjectSpace.FindObject<省份>(null);
                if (area == null)
                {
                    var js = CreateSF("江苏");
                    CreateCity(js, "苏州", "姑苏区,相城区,吴中区,虎丘区,工业园区,吴江区,张家港市,常熟市,太仓巿,昆山市");
                    CreateCity(js, "南京", "鼓楼区,白下区,玄武区,秦淮区,建邺区,下关区,雨花台区,栖霞区,高淳县,溧水县,六合区,浦口区,江宁区");

                    var tjs = CreateSF("天津");
                    CreateCity(tjs, "天津", "和平区,河东区,河西区,南开区,河北区,红桥区,滨海新区,东丽区,西青区,津南区,北辰区,武清区,宝坻区,宁河区,静海区,蓟县");


                    var cqs = CreateSF("重庆");
                    CreateCity(cqs, "重庆",
                        "渝中区,大渡口区,江北区,沙坪坝区,九龙坡区,南岸区,北碚区,渝北区,巴南区,涪陵区,綦江区,大足区,长寿区,江津区,合川区,永川区,南川区,璧山区,铜梁区,潼南区,荣昌区,万州区,梁平县,城口县,丰都县,垫江县,忠县,开县,云阳县,奉节县,巫山县,巫溪县,黔江区,武隆县,石柱土家族自治县,秀山土家族苗族自治县,酉阳土家族苗族自治县,彭水苗族土家族自治县");

                    var zjs = CreateSF("浙江");
                    CreateCity(zjs, "杭州", "市区,上城区,下城区,江干区,拱墅区,西湖区,滨江区,萧山区,余杭区,富阳区,桐庐县,淳安县,建德市,临安市");

                    var scs = CreateSF("四川");
                    CreateCity(scs, "成都", "武侯区,锦江区,青羊区,金牛区,成华区,龙泉驿区,温江区,新都区,青白江区,双流区,郫县,蒲江县,大邑县,金堂县,新津县,都江堰市,彭州市,邛崃市,崇州市");

                    var gds = CreateSF("广东");
                    CreateCity(gds, "广州", "越秀区,荔湾区,海珠区,天河区,白云区,黄埔区,番禺区,花都区,南沙区,增城区,从化区");

                    var szs = CreateSF("深圳");
                    CreateCity(szs, "深圳", "福田区,罗湖区,南山区,盐田区,宝安区,龙岗区");

                    var shs = CreateSF("上海");
                    CreateCity(shs, "上海", "黄浦区,浦东新区,徐汇区,长宁区,静安区,普陀区,虹口区,杨浦区,闵行区,宝山区,嘉定区,金山区,松江区,青浦区,奉贤区,崇明县");

                    var bjs = CreateSF("北京");
                    CreateCity(bjs, "北京", "东城区,西城区,海淀区,朝阳区,丰台区,石景山区,门头沟区,通州区,顺义区,房山区,大兴区,昌平区,怀柔区,平谷区,密云区,延庆区");
                    ObjectSpace.CommitChanges();
                    var s = ObjectSpace.GetObjects<销售区域>();

                    var customers = tdch.GetRandomNames(500);
                    var rnd = new Random();
                    foreach (var customer in customers)
                    {
                        CreateCompany(customer, customer, "13900005555", "中国", false, true, s[rnd.Next(s.Count - 1)]);
                    }


                }
            };
            this.Actions.Add(initData);
            this.RegisterActions(this.components);


            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void CreateCity(省份 sf, string cityName, string sxq)
        {
            var city = ObjectSpace.CreateObject<城市>();
            city.名称 = cityName;
            city.省份 = sf;
            var sq = sxq.Split(',');
            foreach (var s in sq)
            {
                var sqo = ObjectSpace.CreateObject<销售区域>();
                sqo.名称 = s;
                sqo.城市 = city;
            }
        }

        private 省份 CreateSF(string name)
        {
            var bjs = ObjectSpace.CreateObject<省份>();
            bjs.名称 = name;
            return bjs;
        }

        public 往来单位 CreateCompany(string name, string contact, string telephone, string address, bool provider, bool customer, 销售区域 area = null)
        {
            var company = ObjectSpace.CreateObject<往来单位>();
            company.供应商 = true;
            company.名称 = name;
            company.客户 = customer;
            company.供应商 = provider;
            company.销售区域 = area;


            //var con = ObjectSpace.CreateObject<员工>();
            //con.名称 = contact;
            //con.手机 = telephone;
            //var add = ObjectSpace.CreateObject<地址>();
            //add.名称 = address;

            //company.联系人.Add(con);
            //company.地址.Add(add);

            return company;
        }

        产品 CreateProduct(string pname, 计量单位 defaultUnit, decimal buy, decimal sale)
        {
            var p = ObjectSpace.CreateObject<产品>();
            p.名称 = pname;
            p.进货价格 = buy;
            p.零售价 = sale;
            //p.默认单位 = defaultUnit;

            return p;
        }

        private 计量单位 CreateUnit(string name)
        {
            var unit = ObjectSpace.CreateObject<计量单位>();
            unit.名称 = name;
            return unit;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}

