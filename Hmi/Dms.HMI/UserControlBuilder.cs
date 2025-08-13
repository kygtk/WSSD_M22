///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : UserControlBuilder
//-------------------------------------------------------------------------
// Revison History
// * 2008.03.21 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;

namespace Dms.HMI
{
    public class UserControlBuilder
    {
        #region Methods
        static public void InitializeAll(Form mainForm, DeviceTags tagContainer)
        {
            AppConfig app = AppConfig.Instance;
            ServerMode serverMode = ClientManager.Instance.ServerMode;
            if (app.AutoStart || (app.UserControlBuild && (serverMode == ServerMode.Remoting)))
            {
                DmsUserControl.Initialize(mainForm, tagContainer);
            }
        }
        #endregion

        //#region Methods
        //static public void InitializeAll(Form mainForm, DeviceTags tagContainer)
        //{
        //    AppConfig app = AppConfig.Instance;
        //    ServerMode serverMode = ClientManager.Instance.ServerMode;
        //    if (app.AutoStart || (app.UserControlBuild && (serverMode == ServerMode.Remoting)))
        //    {
        //        foreach (System.Windows.Forms.Control control in mainForm.Controls)
        //        {
        //            Panel panel = control as Panel;
        //            if (panel != null) Initialize(panel, tagContainer);
        //        }
        //        foreach (Form form in mainForm.MdiChildren)
        //        {
        //            Initialize(form, tagContainer);
        //        }
        //    }
        //}

        //static private void Initialize(System.Windows.Forms.Control control, DeviceTags tagContainer)
        //{
        //    Type controlType = control.GetType();
        //    if (controlType == typeof(TabControl))
        //    {
        //        Initialize(control as TabControl, tagContainer);
        //    }
        //    else if (controlType == typeof(TabPage))
        //    {
        //        Initialize(control as TabPage, tagContainer);
        //    }
        //    else if (controlType == typeof(UserControl))
        //    {
        //        Initialize(control as UserControl, tagContainer);
        //    }
        //    else if (controlType == typeof(GroupBox))
        //    {
        //        Initialize(control as GroupBox, tagContainer);
        //    }
        //    else if (controlType == typeof(CylinderGroupBox))
        //    {
        //        Initialize(control as CylinderGroupBox, tagContainer);
        //    }
        //    else if (controlType == typeof(ActuatorGroup))
        //    {
        //        Initialize(control as ActuatorGroup, tagContainer);
        //    }
        //    else if ((control as DmsUserControl) != null)
        //    {
        //        Initialize(control as DmsUserControl, tagContainer);
        //    }
        //    else if ((control as UserControl) != null)
        //    {
        //        Initialize((control as UserControl), tagContainer);
        //    }
        //    else if ((control as Panel) != null)
        //    {
        //        Initialize((control as Panel), tagContainer);
        //    }
        //}

        //static private void Initialize(Form form, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in form.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(Panel panel, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in panel.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(TabControl tab, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in tab.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(TabPage page, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in page.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(UserControl ucon, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in ucon.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(GroupBox groupBox, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in groupBox.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(CylinderGroupBox groupBox, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in groupBox.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(ActuatorGroup groupBox, DeviceTags tagContainer)
        //{
        //    foreach (System.Windows.Forms.Control control in groupBox.Controls)
        //    {
        //        Initialize(control, tagContainer);
        //    }
        //}

        //static private void Initialize(DmsUserControl dmsUCon, DeviceTags tagContainer)
        //{
        //    dmsUCon.Initialize(tagContainer);
        //}
        //#endregion
    }
}
