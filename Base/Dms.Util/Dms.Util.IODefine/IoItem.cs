using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public delegate void SetIoStateEventHandler(object sender, IoStateEventArgs e);

    [Serializable()]
    public class IoItem
    {
        #region Fields
        protected int m_Id = -1;
        protected int m_Node = 0;
        protected int m_Terminal = 0;
        protected int m_Channel = 0;
        protected string m_WiringNo = ""; // jemoon : 081022 - wiringNo 정보추가 (ex. 1X001)
        protected string m_Name = "";
        protected string m_Description = "";
        protected string m_State = " ";
        protected IoType m_IoType = IoType.DI;
        #endregion

        #region Event

        #endregion

        #region Properties
        [Category("Address")]
        [ReadOnly(true)]
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        [Category("Address")]
        [ReadOnly(true)]
        public int Node
        {
            get { return m_Node; }
            set { m_Node = value; }
        }

        [Category("Address")]
        [ReadOnly(true)]
        public int Terminal
        {
            get { return m_Terminal; }
            set { m_Terminal = value; }
        }

        [Category("Address")]
        [ReadOnly(true)]
        public int Channel
        {
            get { return m_Channel; }
            set { m_Channel = value; }
        }

        [Category("Address")]
        //[ReadOnly(true)]
        public string WiringNo
        {
            get { return m_WiringNo; }
            set { m_WiringNo = value; }
        }

        [Category("I/O Info")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [Category("I/O Info")]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        [Category("I/O Info")]
        [ReadOnly(true)]
        public IoType IoType
        {
            get { return m_IoType; }
            set { m_IoType = value; }
        }

        [Category("I/O Info")]
        [ReadOnly(true), XmlIgnore()]
        public string State
        {
            get { return m_State; }
            set { m_State = value; }
        }
        #endregion

        #region Constructor
        public IoItem()
        {
        }
        public IoItem(IoType type)
        {
            m_IoType = type;
        }
        public IoItem(IoType type, int id)
        {
            m_IoType = type;
            m_Id = id;
        }
        #endregion

        #region Methods
        public virtual IoItem Clone()
        {
            IoItem item = new IoItem();
            item.Id = m_Id;
            item.Node = m_Node;
            item.Terminal = m_Terminal;
            item.Channel = m_Channel;
            item.Description = m_Description;
            item.IoType = m_IoType;
            item.Name = m_Name;
            item.WiringNo = m_WiringNo;

            return item;
        }

        public override string ToString()
        {
            if (this.Name.Length == 0)
            {
                return this.GetType().Name;
            }
            else
            {
                return this.Name;
            }
        }
        #endregion
    }

    [Serializable()]
    public class IoItemDI : IoItem
    {
        #region Fields
        private ActiveType m_ActiveType = ActiveType.A;
        #endregion

        #region Properties
        [Category("I/O Info")]
        public ActiveType ActiveType
        {
            get { return m_ActiveType; }
            set { m_ActiveType = value; }
        }
        #endregion

        #region Constructor
        public IoItemDI() : base()
        {
        }
        public IoItemDI(IoType type) : base(type)
        {
        }
        #endregion

        #region Override
        public new IoItemDI Clone()
        {
            IoItemDI item = new IoItemDI();
            item.Id = m_Id;
            item.Node = m_Node;
            item.Terminal = m_Terminal;
            item.Channel = m_Channel;
            item.Description = m_Description;
            item.IoType = m_IoType;
            item.Name = m_Name;
            item.WiringNo = m_WiringNo;
            item.ActiveType = m_ActiveType;

            return item;
        }
        #endregion
    }
}
