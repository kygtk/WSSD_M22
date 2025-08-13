using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class IoNodeMovensysEcMaster : IoNode
    {
        #region Properties
        [Browsable(false)]
        public override List<IoTerminal> Terminals
        {
            get { return base.Terminals; }
            set { base.Terminals = value; }
        }

        [Browsable(true)]
        public override List<EcSlave> Slaves
        {
            get { return base.Slaves; }
            set { base.Slaves = value; }
        }
        #endregion

        #region Constructor
        public IoNodeMovensysEcMaster()
        {
        }

        public IoNodeMovensysEcMaster(FieldBusType busType)
            : base(busType)
        {
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Id + " : " + "Movensys EC Master";
        }
        #endregion
    }
}
