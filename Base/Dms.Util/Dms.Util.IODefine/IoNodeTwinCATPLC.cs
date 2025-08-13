using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class IoNodeTwinCATPLC : IoNode
    {
        #region Fields
        private string m_AmsNetId = "127.0.0.1.1.1";
        private int m_PortNo = 801;
        private int m_Offset;
        private int m_Size;
        private string m_NodeStartNo = "";
        #endregion

        #region Properties
        [Category("Connection Info")]
        public string AmsNetId
        {
            get { return m_AmsNetId; }
            set { m_AmsNetId = value; }
        }
        [Category("Connection Info")]
        public int PortNo
        {
            get { return m_PortNo; }
            set { m_PortNo = value; }
        }
        [Category("Connection Info")]
        public int Offset
        {
            get { return m_Offset; }
            set { m_Offset = value; }
        }
        [Category("Connection Info")]
        public int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }
        [Category("Connection Info")]
        public string NodeStartNo
        {
            get { return m_NodeStartNo; }
            set { m_NodeStartNo = value; }
        }
        #endregion

        #region Constructor
        public IoNodeTwinCATPLC()
        { 
        }
        public IoNodeTwinCATPLC(FieldBusType busType)
        {
            MakeNodeModule(busType);
        }
        #endregion

        #region Methods
        #endregion

        #region Override
        public override string ToString()
        {
            return m_Id + " : " + "TcAdsClient_" + m_PortNo.ToString() + "_" + Enum.GetName(typeof(Dms.Common.VarType), m_Id);
        }
        #endregion
    }
}
