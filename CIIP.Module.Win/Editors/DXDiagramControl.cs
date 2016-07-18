using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.Diagram.Core;
using DevExpress.Utils.Controls;
using DevExpress.XtraDiagram;
using DevExpress.XtraDiagram.ViewInfo;

namespace CIIP.Module.Win.Editors
{
    public class DXDiagramControl : DiagramControl, IXtraResizableControl
    {
        public DXDiagramControl()
        {
            this.Items.ListChanged += Items_ListChanged;
            
        }

        protected override DiagramController CreateDiagramController()
        {
            return new DiagramControllerEx(this);
        }
        
        protected override void OnSelectionChanged(DiagramSelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (SelectedItems.Count == 1)
            {
                var item = this.SelectedItems[0] as DiagramConnector;
                if (item != null)
                {
                    if (item.BeginItem != null && item.EndItem != null)
                    {
                        if (this.AddedConnector != null && item.Tag == null)
                            this.AddedConnector(this, item);
                    }
                    else
                    {
                        this.DeleteSelectedItems();
                    }
                }
            }
        }
        
        private void Items_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemDeleted)
            {
                var deleted = sourceItems.Except(this.Items.OfType<DiagramItem>());
                foreach (var item in deleted)
                {
                    if (item is DiagramShape)
                    {
                        if (DeleteShape != null)
                        {
                            DeleteShape(this, item as DiagramShape);
                        }
                    }
                    if (item is DiagramConnector)
                    {
                        if (DeleteConnector != null)
                        {
                            DeleteConnector(this, item as DiagramConnector);
                        }
                    }
                }
            }

            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                if (Items[e.NewIndex] is DiagramShape && AddedShape != null)
                {
                    this.AddedShape(this, Items[e.NewIndex] as DiagramShape);
                }
                //if (Items[e.NewIndex] is DiagramConnector && AddedConnector != null)
                //{
                //    this.AddedConnector(this, Items[e.NewIndex] as DiagramConnector);
                //}
            }
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded ||
                e.ListChangedType == System.ComponentModel.ListChangedType.ItemDeleted)
                sourceItems = this.Items.ToList();
        }

        private List<DiagramItem> sourceItems;

        protected override void OnShowingEditor(DiagramShowingEditorEventArgs e)
        {
            if(e.Item is DiagramConnector)
            {
                if (EditConnector != null)
                {
                    EditConnector(this, e.Item as DiagramConnector);
                }
            }
            if(e.Item is DiagramShape)
            {
                if (EditShape != null)
                {
                    EditShape(this, e.Item as DiagramShape);
                }
            }
            e.Cancel = true;
            base.OnShowingEditor(e);
        }

        public event Action<object,DiagramShape> EditShape;

        public event Action<object, DiagramConnector> EditConnector;

        public event Action<object, DiagramShape> DeleteShape;

        public event Action<object, DiagramConnector> DeleteConnector;

        public event Action<object, DiagramConnector> AddedConnector;

        public event Action<object, DiagramShape> AddedShape;

        bool IXtraResizableControl.IsCaptionVisible
        {
            get { return false; }
        }

        Size IXtraResizableControl.MaxSize
        {
            get { return this.MaximumSize; }
        }

        Size IXtraResizableControl.MinSize
        {
            get { return this.MinimumSize; }
        }
        
        protected override void OnBeforeDispose()
        {
            this.Items.ListChanged -= this.Items_ListChanged;
            base.OnBeforeDispose();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override DiagramControlViewInfo CreateViewInfo()
        {
            return new DiagramControlViewInfoEx(this);
        }
    }
}