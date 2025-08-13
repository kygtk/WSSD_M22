///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.05
// Author       : jemoon
// Description  : GenericCollection of items
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Windows.Forms;
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Device
{
    public class BuzzerControl
    {
        #region Fields
        private IServerManager m_Server;
        private Simul m_Simul;
        private _GenericCollection<Buzzer> m_Items;
        #endregion

        #region Properties
        public _GenericCollection<Buzzer> Items
        {
            get { return m_Items; }
        }

        [Browsable(false), XmlIgnore()]
        public bool BuzzerOffSwitchPushed
        {
            get
            {
                bool pushed = false;
                if (m_Server != null)
                {
                    foreach (Buzzer device in m_Items)
                    {
                        pushed = device.IsPushed();
                        if (pushed && (m_Simul.Device || device.UseLogicalSwitch))
                        {
                            BuzzerOffSwitchPushed = false;
                            break;
                        }
                    }
                }

                return pushed;
            }
            set
            {
                if (m_Simul != null)
                {
                    foreach (Buzzer device in m_Items)
                    {
                        device.SetSwitchPushed(value);
                        if (value == true)
                        {
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public BuzzerControl(_GenericCollection<Buzzer> collection)
        {
            m_Items = collection;
            m_Server = m_Items.ServerManager;
            m_Simul = AppConfig.Instance.Simul;
        }
        #endregion

        #region Methods
        public void SetLampState(Lamp state)
        {
            foreach (Buzzer obj in m_Items)
            {
                obj.SetLampState(state);
            }
        }
        public void SetMelodyState(int id, Melody state)
        {
            foreach (Buzzer obj in m_Items)
            {
                obj.SetMelodyState(id, state);
            }
        }
        #endregion

        #region Override
        #endregion
    }
}
