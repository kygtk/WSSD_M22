using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class IoTerminal : IoPart, ITerminalFactory
    {
        #region Fields
        protected int m_Id;
        protected int m_IdByIoType;
        protected IoType m_IoType;
        protected int m_ChannelCount;
        protected List<IoItem> m_Channels = new List<IoItem>();
        //private string m_Description = ""; 
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
        public int IdByIoType
        {
            get { return m_IdByIoType; }
            set { m_IdByIoType = value; }
        }

        [Category("Terminal Info")]
        [ReadOnly(true)]
        public IoType IoType
        {
            get { return m_IoType; }
            set { m_IoType = value; }
        }
        //[Category("Terminal Info")]
        //[ReadOnly(true)]
        //public TerminalType TerminalType
        //{
        //    get { return m_TerminalType; }
        //    set { m_TerminalType = value; }        
        //}
        [Category("Terminal Info")]
        [ReadOnly(true)]
        public int ChannelCount
        {
            get { return m_ChannelCount; }
            set { m_ChannelCount = value; }
        }
        [Category("Terminal Info")]
        [ReadOnly(true)]
        public List<IoItem> Channels
        {
            get { return m_Channels; }
            set { m_Channels = value; }
        }
        //[Category("Terminal Info")]
        //[ReadOnly(true)]
        //public string Description
        //{
        //    get { return m_Description; }
        //} 
        #endregion

        #region Constructor
        public IoTerminal()
        {

        }
        #endregion

        #region Methods
        public virtual void CreateChannels()
        {
            if (this.Channels.Count != 0) return;

            for (int i = 0; i < m_ChannelCount; i++)
            {
                IoItem item = new IoItem();
                switch (this.IoType)
                {
                    case IoType.DI:
                        item = new IoItemDI();
                        item.Name = "di__";
                        break;
                    case IoType.DO:
                        item.Name = "do__";
                        break;
                    case IoType.AI:
                        item.Name = "ai__";
                        break;
                    case IoType.AO:
                        item.Name = "ao__";
                        break;

                }

                item.Channel = i;
                item.IoType = this.IoType;
                this.Channels.Add(item);
            }
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.GetType().Name + "." + this.IoType;
        }
        #endregion

        #region ITerminalFactory ¸â¹ö
        public IoTerminal CreateObject()
        {
            return Activator.CreateInstance(this.GetType()) as IoTerminal;
        }
        #endregion
    }
}
