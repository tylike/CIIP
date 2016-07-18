using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;

namespace CIIP
{
    public static class IModelListViewExtendesion
    {
        public static void SetNewItemRow(this IModelListView view, NewItemRowPosition position)
        {
            var list = view as IModelListViewNewItemRow;
            list.NewItemRowPosition = position;
        }

        public static NewItemRowPosition GetNewItemRow(this IModelListView view)
        {
            return (view as IModelListViewNewItemRow).NewItemRowPosition;
        }
    }
}