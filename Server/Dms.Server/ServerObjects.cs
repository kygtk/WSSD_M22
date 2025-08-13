using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Dms.Common;
using Dms.Device;
using System.Threading;

namespace Dms.Server
{
    partial class ServerManager
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        private DmsErrors InitializeServerObjects()
        {
            try
            {
                bool ok = true;
                ////////////////////////////////////////////////////////////////////////////////////////
                // Initialize DmsComponents
                foreach (IGenericCollection collection in m_DmsComponents.ContainedItems)
                {
                    if (!collection.Initialize(this, m_GenInfos))
                    {
                        ok = false;
                        break;
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////
                // Initialize EqpManager
                if (ok)
                {
                    m_EqpStateManager = EqpManager.Instance;
                    EqpUnits eqpUnits = m_DmsComponents.ComponentContainer.GetCollection<EqpUnit>() as EqpUnits;
                    if (eqpUnits == null || eqpUnits.Count < 1)
                    {
                        m_EqpStateManager.EqpUnit = new EqpUnit("NotDefinedEquipment");
                        m_EqpStateManager.EqpUnit.Initialize(this, m_GenInfos);
                    }
                    else
                    {
                        m_EqpStateManager.EqpUnit = eqpUnits[0] as EqpUnit;
                    }
                    m_EqpStateManager.Initialize(this, m_DmsComponents);
                }
                ////////////////////////////////////////////////////////////////////////////////////////

                return ok ? DmsErrors.Success : DmsErrors.InternalError;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                WriteExceptionLog(msg);
                MessageBox.Show(msg);
                //Application.Exit();

                return DmsErrors.InternalError;
            }
        }

        private DmsErrors UninitializeServerObjects()
        {
            if (this.UninitializeDel != null)
            {
                Delegate[] dels = this.UninitializeDel.GetInvocationList();

                int count = dels.Length;
                for (int i = count - 1; i >= 0; i--)
                {
                    ((UninitializeDelegate)dels[i])();
                }
            }

            Thread.Sleep(500);

            return DmsErrors.Success;
        }
        #endregion
    }
}
