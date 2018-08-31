using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Templates.Bars.ActionControls;
using DevExpress.ExpressApp.Win.Templates.Ribbon;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.ComponentModel;
using System.Linq;

namespace CIIP.Designer.Module.Win    
{
    public class RibbonFromModelWithoutEventController : WindowController, IModelExtender
    {
        private IModelActions modelActions;

        void IModelExtender.ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelAction, IModelRibbonAction>();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            this.modelActions = Application.Model.ActionDesign.Actions;
            this.Application.CustomizeTemplate += this.Application_CustomizeTemplate;
        }

        protected override void OnDeactivated()
        {
            this.Application.CustomizeTemplate -= this.Application_CustomizeTemplate;
            this.modelActions = null;
            base.OnDeactivated();
        }
        private void Application_CustomizeTemplate(object sender, CustomizeTemplateEventArgs e)
        {
            if (e.Template is RibbonForm ribbonForm && ribbonForm.Ribbon is XafRibbonControlV2)
            {
                XafRibbonControlV2 ribbonControl = ribbonForm.Ribbon as XafRibbonControlV2;
                ribbonControl.BeginInit();
                foreach (IModelRibbonAction modelAction in modelActions.Select(action => action).Cast<IModelRibbonAction>().Where(a => !string.IsNullOrEmpty(a.TargetRibbonPage)))
                {
                    RibbonPage page = null;
                    page = ribbonControl.Pages.GetPageByText(modelAction.TargetRibbonPage);
                    if (page == null)
                    {
                        page = new RibbonPage(modelAction.TargetRibbonPage);
                        page.Name = modelAction.TargetRibbonPage;
                        ribbonControl.Pages.Add(page);
                    }

                    var group = page.Groups.GetGroupByText(modelAction.TargetRibbonGroup);
                    if (group == null)
                    {
                        var ribbonGroup = new RibbonPageGroup(modelAction.TargetRibbonGroup);
                        ribbonGroup.Name = modelAction.TargetRibbonGroup;
                        ribbonGroup.AllowTextClipping = false;

                        page.Groups.Add(ribbonGroup);

                        var barLinkContainerExItem = new BarLinkContainerExItem();
                        ribbonControl.Items.Add(barLinkContainerExItem);
                        ribbonGroup.ItemLinks.Add(barLinkContainerExItem);

                        var actionContainer = new BarLinkActionControlContainer();
                        actionContainer.BeginInit();
                        ribbonControl.ActionContainers.Add(actionContainer);
                        actionContainer.ActionCategory = modelAction.TargetRibbonGroup;
                        actionContainer.BarContainerItem = barLinkContainerExItem;
                        actionContainer.EndInit();
                    }
                }
                ribbonControl.EndInit();
            }
        }
    }

    public interface IModelRibbonAction : IModelAction
    {
        /// <summary>
        /// Gets or sets the target ribbon page.
        /// </summary>
        /// <value>The target ribbon page.</value>
        [Localizable(true)]
        string TargetRibbonPage { get; set; }

        /// <summary>
        /// Gets or sets the target ribbon group.
        /// </summary>
        /// <value>The target ribbon group.</value>
        [Localizable(true)]
        string TargetRibbonGroup { get; set; }

        /// <summary>
        /// Gets or sets the ribbon style.
        /// </summary>
        /// <value>The ribbon style.</value>
        RibbonItemStyles RibbonStyle { get; set; }
    }
}