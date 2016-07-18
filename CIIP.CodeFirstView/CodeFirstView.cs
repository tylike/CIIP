using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CIIP.CodeFirstView
{
    public abstract class ListViewObject<TBusinessObject>:ViewObject
    {
        public override Type BusinessObjectType
        {
            get
            {
                return typeof(TBusinessObject);
            }
        }
        public IModelListView View { get; private set; }
        public IModelLayoutGroup DetailViewLayout { get; private set; }

        public IModelDetailView DetailView { get; private set; }
        
        public override void UpdateNode(IModelListView view)
        {
            try
            {
                this.View = view;
                this.DetailViewLayout = this.View.DetailView.Layout[0] as IModelLayoutGroup;
                
                this.DetailView = this.View.DetailView;

                Debug.WriteLine(view.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (this.DetailViewLayout == null)
                throw new Exception("没有找到DetailViewLayout!");

            LayoutListView();

            LayoutDetailView();
            
        }

        public void LayoutColumns(params Expression<Func<TBusinessObject, object>>[] columns)
        {
            LayoutListViewColumns(View, columns);
        }

        public void LayoutListViewColumns<T>(IModelListView listView, params Expression<Func<T, object>>[] columns)
        {
            var index = 0;
            List<string> names = new List<string>();
            foreach (var item in columns)
            {
                var name = GetMemberName(item);
                var column = listView.Columns[name];
                index += 1000;
                column.Index = index;
                names.Add(name);
            }
            if (names.Count != listView.Columns.Count)
            {
                var hiddens = listView.Columns.Where(x => !names.Contains(x.PropertyName)).ToArray();
                foreach (var h in hiddens)
                {
                    h.Index = -1;
                }
            }
        }

        public void AddPropertyToDetailView(Expression<Func<TBusinessObject, object>> property)
        {
            var name = GetMemberName(property);
            if (DetailView.Items[name] == null)
            {
                var n = DetailView.Items.AddNode<IModelPropertyEditor>(name);
                n.PropertyName = name;
            }
        }
        
        protected IModelLayoutGroup HGroup(int index, params Expression<Func<TBusinessObject, object>>[] property)
        {
            return HGroup(this.DetailView, this.DetailViewLayout, index, property);
        }

        protected IModelLayoutGroup HGroup<T>(IModelDetailView view, IModelLayoutGroup layout, int index, Expression<Func<T, object>>[] property)
        {
            try
            {
                //layout.GetNodes<IModelLayoutViewItem>();
                
                var gname = "R" + (layout.Count()+1);
                //var ng = (layout.GetNode(gname));
                //if (ng != null)
                //{
                    
                //}

                var group = layout.AddNode<IModelLayoutGroup>(gname);
                group.Index = index;
                group.Direction = FlowDirection.Horizontal;

                var lastIndex = group.NodeCount;
                foreach (var item in property)
                {
                    var rst = group.AddNode<IModelLayoutViewItem>("I" + lastIndex);
                    if (item != null)
                    {
                        var name = GetMemberName(item);
                        rst.ViewItem = view.Items[name];
                    }
                    lastIndex++;
                    rst.Index = lastIndex;
                }
                return group;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IModelTabbedGroup TabbedGroup(int index, params Expression<Func<TBusinessObject, object>>[] property)
        {
            var list = new List<string>();
            foreach (var item in property)
            {
                var name = GetMemberName(item);
                list.Add(name);
            }
            return TabbedGroup(index, list.ToArray());
        }

        protected IModelTabbedGroup TabbedGroup(int index, params string[] property)
        {
            var tab = DetailViewLayout.AddNode<IModelTabbedGroup>("T" + index);
            tab.Index = index;

            var lastIndex = 0;
            foreach (var item in property)
            {
                var group = tab.AddNode<IModelLayoutGroup>("G" + lastIndex);
                var rst = group.AddNode<IModelLayoutViewItem>("I" + lastIndex);
                rst.ViewItem = DetailView.Items[item];
                lastIndex++;
                group.Index = lastIndex;
            }
            return tab;
        }

        protected IModelTabbedGroup TabbedGroup<T>(int index, params Expression<Func<T, object>>[] exp)
        {
            var list = new List<string>();
            foreach (var item in exp)
            {
                list.Add(GetMemberName(item));
            }
            return TabbedGroup(index,list.ToArray());
        }

        public static string GetMemberName<T>(Expression<Func<T, object>> exp)
        {
            if (exp.Body is MemberExpression)
            {
                return string.Join(".", (exp.Body.ToString().Split('.').Skip(1)));
                //as MemberExpression).Member.Name;
            }
            if (exp.Body is UnaryExpression)
            {
                return ((exp.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }

            throw new Exception("未处理的表达式!");
        }

        public IModelListView GetChildListView(Expression<Func<TBusinessObject, object>> property)
        {
            var p = DetailView.Items[GetMemberName(property)] as IModelPropertyEditor;
            return p.View as IModelListView;
        }
    }

}
