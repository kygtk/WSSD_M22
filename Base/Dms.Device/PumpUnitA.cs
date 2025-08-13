using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.ComponentModel;
using System.Xml.Serialization;


namespace Dms.Device
{
    public class PumpUnitA : PumpUnit
    {
        #region Fields
        private TankUnit m_Tank2 = null;
        #endregion

        #region Properties
        [Category("Setting")]
        public TankUnit TankUnit2
        {
            get { return m_Tank2; }
            set { m_Tank2 = value; }
        }
        #endregion

        #region Constructor
        public PumpUnitA() : base()
        { 
        }
        #endregion

        #region Methods
        public bool IsTanksLevelFault(Logic logic)
        {
            bool levelFault = false;
            if (logic == Logic.OR)
            {
                levelFault |= base.Tank.IsLevelFault();
                levelFault |= m_Tank2.IsLevelFault();
            }
            else
            {
                levelFault = base.Tank.IsLevelFault() && m_Tank2.IsLevelFault();
            }
            return levelFault;
        }

        public bool IsTanksLevelFault(string tankName)
        {
            bool levelFault = false;
            if (base.Tank.Name == tankName)
            {
                levelFault = base.Tank.IsLevelFault();
            }
            else if (m_Tank2.Name == tankName)
            {
                levelFault = m_Tank2.IsLevelFault();
            }
            return levelFault;
        }

        public bool IsTanksRunEnableLevel(Logic logic)
        {
            bool runEnable = false;
            if (logic == Logic.OR)
            {
                runEnable |= base.Tank.IsRunEnableLevel();
                runEnable |= m_Tank2.IsRunEnableLevel();
            }
            else
            {
                runEnable = base.Tank.IsRunEnableLevel() && m_Tank2.IsRunEnableLevel();
            }
            return runEnable;
        }

        public bool IsTanksRunEnableLevel(string tankName)
        {
            bool runEnable = false;
            if (base.Tank.Name == tankName)
            {
                runEnable = base.Tank.IsRunEnableLevel();
            }
            else if(m_Tank2.Name == tankName)
            {
                runEnable = m_Tank2.IsRunEnableLevel();
            }
            return runEnable;
        }

        public bool IsTanksReady(Logic logic)
        {
            bool ready = false;
            if (logic == Logic.OR)
            {
                ready |= base.Tank.IsTankReady();
                ready |= m_Tank2.IsTankReady();
            }
            else
            {
                ready = base.Tank.IsTankReady() && m_Tank2.IsTankReady();
            }
            return ready;
        }

        public bool IsTanksReady(string tankName)
        {
            bool ready = false;
            if (base.Tank.Name == tankName)
            {
                ready = base.Tank.IsTankReady();
            }
            else if(m_Tank2.Name == tankName)
            {
                ready = m_Tank2.IsTankReady();
            }
            return ready;
        }
        #endregion
    }
}
