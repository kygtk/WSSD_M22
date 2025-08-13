using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class IoNodeCrevisModbus : IoNode
    {
        #region Fields
        private string m_IpAddress = "192.168.100.1";
        private ushort m_PortNo = 502;
        #endregion

        #region Properties
        [Category("Connection Info")]
        public string IpAddress
        {
            get { return m_IpAddress; }
            set { m_IpAddress = value; }
        }
        [Category("Connection Info")]
        public ushort PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        #endregion
        
        #region Constructor
        public IoNodeCrevisModbus()
        {
        }
        public IoNodeCrevisModbus(FieldBusType busType)
        {
            MakeNodeModule(busType);
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Id + " : " + "Crevis Master";
        } 
        #endregion
    }
}
