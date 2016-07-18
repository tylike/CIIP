using System;
using System.Collections.Generic;
using System.Linq;

namespace CIIP.CodeFirstView
{
    public class ViewsManager
    {

        //public static void Initialize(XafApplication app)
        //{
        //    var asm = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.StartsWith("DevExpress.") && !x.FullName.StartsWith("System."));

        //        //app.Modules.Where(
        //        //    x => !x.AssemblyName.StartsWith("DevExpress.") && !x.AssemblyName.StartsWith("System."));
        //    var types =
        //        asm.SelectMany(x => x.GetType().Assembly.GetTypes())
        //            .Where(t => typeof(ViewObject).IsAssignableFrom(t) && !t.IsAbstract);
        //    ViewObjectTypes = types.ToList();
        //    ViewObjects = new List<ViewObject>();

        //    foreach (var item in ViewObjectTypes)
        //    {
        //        ViewObjects.Add(Activator.CreateInstance(item) as ViewObject);
        //    }
        //}
        static List<ViewObject> _viewObjects;
        public static List<ViewObject> ViewObjects
        {
            get
            {
                if (_viewObjects == null)
                {
                    var asm = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.StartsWith("DevExpress.") && !x.FullName.StartsWith("System.") && !x.FullName.StartsWith("Microsoft.") && !x.FullName.StartsWith("mscorlib."));

                    //app.Modules.Where(
                    //    x => !x.AssemblyName.StartsWith("DevExpress.") && !x.AssemblyName.StartsWith("System."));
                    var types =
                        asm.SelectMany(x => x.GetTypes())
                            .Where(t => typeof(ViewObject).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType);
                    ViewObjectTypes = types.ToList();
                    _viewObjects = new List<ViewObject>();

                    foreach (var item in ViewObjectTypes)
                    {
                        _viewObjects.Add(Activator.CreateInstance(item) as ViewObject);
                    }
                }
                return _viewObjects;
            }
        }

        public static List<Type> ViewObjectTypes
        {
            get; private set;
        }

    }
}