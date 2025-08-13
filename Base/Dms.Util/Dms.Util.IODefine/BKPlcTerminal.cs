///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.08.18
// Author       : EUN
// Description  : Beckhoff TwinCAT PLC사용할 변수들에 대한 정보
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Dms.Common;

namespace Dms.Util.IODefine
{
    #region 나중에 다시 검토
    /*   public enum BKPlcVariableType
    {
        Bool,     //1BYTE
        Byte,     //1BYTE
        Int,       //2BYTE
        Word    //2BYTE
    }

    [Serializable()]
    abstract public class BKPlcTerminal : IoTerminal
    {
        #region Fields
        protected int m_Offset;
        protected int m_VarSize;
        protected BKPlcVariableType m_Type;
        protected static Dictionary<BKPlcVariableType, int> m_DicSize = new Dictionary<BKPlcVariableType, int>();
        protected static Dictionary<BKPlcVariableType, string> m_DicName = new Dictionary<BKPlcVariableType, string>();
        #endregion

        #region Properties
        //[Category("Variable Info")]
        //public int Size
        //{
        //    get { return m_ChannelCount; }
        //    set
        //    {
        //        if (value < 0) value = 0;
        //        m_ChannelCount = value;
        //        CreateChannels();
        //    }
        //}
        [Category("Variable Info")]
        public int Offset
        {
            get { return m_Offset; }
            set 
            { 
                m_Offset = value;
                UpdateOffset();
            }
        }
        #endregion

        #region Constructor
        public BKPlcTerminal()
        {
            if (m_DicSize.Count == 0)
            {
                m_DicSize.Add(BKPlcVariableType.Bool, 1);
                m_DicSize.Add(BKPlcVariableType.Byte, 1);
                m_DicSize.Add(BKPlcVariableType.Int, 2);
                m_DicSize.Add(BKPlcVariableType.Word, 2);

                m_DicName.Add(BKPlcVariableType.Bool, "b");
                m_DicName.Add(BKPlcVariableType.Byte, "bt");
                m_DicName.Add(BKPlcVariableType.Int, "n");
                m_DicName.Add(BKPlcVariableType.Word, "w");
            }
        }
        #endregion

        protected virtual void UpdateOffset()
        {
            int index = 0;
            foreach (IoItem item in m_Channels)
            {
                item.Id = m_Offset + (m_VarSize * index);
                index++;
            }
        }

        protected virtual void UpdateVarSize()
        {
            m_VarSize = m_DicSize[m_Type];
        }

        public override string ToString()
        {
            //return this.GetType().Name + "." + this.IoType;
            return this.GetType().Name + "." + Enum.GetName(typeof(BKPlcVariableType), m_Type) + m_Offset.ToString();
        }

        public override void CreateChannels()
        {
            int curChnnelsCount = this.Channels.Count;

            if (curChnnelsCount == m_ChannelCount)
            {
                //Noop
            }
            else if (curChnnelsCount > m_ChannelCount)
            {
                //현재 등록된 채널에서 초과분 만큼 빼줘야 겠지
                for (int i = curChnnelsCount; i > m_ChannelCount; i--)
                {
                    this.Channels.RemoveAt(i - 1);
                }
            }
            else
            {
                //현재 등록된 채널에서 부족분 만큼 더해준다
                for (int i = curChnnelsCount; i < m_ChannelCount; i++)
                {
                    IoItem item = new IoItem();
                    switch (this.IoType)
                    {
                        case Dms.Common.IoType.DI:
                            item = new IoItemDI();
                            item.Name = "di__";
                            break;
                        case Dms.Common.IoType.DO:
                            item.Name = "do__";
                            break;
                        case Dms.Common.IoType.AI:
                            item.Name = "ai__";
                            break;
                        case Dms.Common.IoType.AO:
                            item.Name = "ao__";
                            break;

                    }

                    item.Id = m_Offset + (m_VarSize * i);
                    item.Channel = i;
                    item.IoType = this.IoType;
                    this.Channels.Add(item);
                }
            }
        }
    }

    //[Serializable()]
    //public class BKPlcVariable : BKPlcTerminal
    //{
    //    [Category("Variable Info")]
    //    public int Size
    //    {
    //        get { return m_ChannelCount; }
    //        set
    //        {
    //            if (value < 0) value = 0;
    //            m_ChannelCount = value;
    //            CreateChannels();
    //        }
    //    }

    //    [Browsable(false)]
    //    public int VarSize
    //    {
    //        get { return m_VarSize; }
    //    }

    //    public override string ToString()
    //    {
    //        //return this.GetType().Name + "." + this.IoType;
    //        return this.GetType().Name + "." + Enum.GetName(typeof(BKPlcVariableType), m_Type);
    //    }
    //}

    [Serializable()]
    public class BKPlcIn : BKPlcTerminal
    {
        #region Properties
        [Category("Variable Info")]
        public int Size
        {
            get { return m_ChannelCount; }
            set
            {
                if (value < 0) value = 0;
                m_ChannelCount = value;
                CreateChannels();
            }
        }

        [Browsable(false)]
        public int VarSize
        {
            get { return m_VarSize; }
        }

        [Category("Variable Info")]
        public BKPlcVariableType VarType
        {
            get { return m_Type; }
            set 
            { 
                m_Type = value;
                UpdateVarSize();
                m_Channels.Clear(); //type이 바뀌었으므로 다시 만들어야 함
                CreateChannels();
            }
        }
        #endregion

        #region Constructor
        public BKPlcIn()
        {
            //this.IoType = Dms.Common.IoType.DI;
            m_ChannelCount = 1;
            //m_VarSize = 1;
            UpdateVarSize();

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Input variable";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.BeckhoffPLC;
            m_ProductName = "BeckhoffPlc Input Variable";
            m_ProductId = 0;
        } 
        #endregion

        #region Properties
        protected override void UpdateVarSize()
        {
            m_VarSize = m_DicSize[m_Type];
            if (m_Type == BKPlcVariableType.Bool)
            {
                m_IoType = Dms.Common.IoType.DI;
            }
            else
            {
                m_IoType = Dms.Common.IoType.AI;
            }
        }

        public override void CreateChannels()
        {
            int curChnnelsCount = this.Channels.Count;

            if (curChnnelsCount == m_ChannelCount)
            {
                //Noop
            }
            else if (curChnnelsCount > m_ChannelCount)
            {
                //현재 등록된 채널에서 초과분 만큼 빼줘야 겠지
                for (int i = curChnnelsCount; i > m_ChannelCount; i--)
                {
                    this.Channels.RemoveAt(i - 1);
                }
            }
            else
            {
                //현재 등록된 채널에서 부족분 만큼 더해준다
                for (int i = curChnnelsCount; i < m_ChannelCount; i++)
                {
                    IoItem item = new IoItem();
                    switch (this.IoType)
                    {
                        case Dms.Common.IoType.DI:
                            item = new IoItemDI();
                            item.Name = "di__";
                            break;
                        case Dms.Common.IoType.DO:
                            item.Name = "do__";
                            break;
                        case Dms.Common.IoType.AI:
                            item.Name = "ai__";
                            break;
                        case Dms.Common.IoType.AO:
                            item.Name = "ao__";
                            break;

                    }

                    //item.Id = m_Offset + (m_VarSize * i);
                    item.Channel = i;
                    item.IoType = this.IoType;
                    this.Channels.Add(item);
                }
            }
            UpdateOffset();
        }
        #endregion

        //public override string ToString()
        //{
        //    //return this.GetType().Name + "." + this.IoType;
        //    return this.GetType().Name + "." + Enum.GetName(typeof(BKPlcVariableType), m_Type);
        //}
    }

    [Serializable()]
    public class BKPlcOut : BKPlcTerminal
    {
        #region Properties
        [Category("Variable Info")]
        public int Size
        {
            get { return m_ChannelCount; }
            set
            {
                if (value < 0) value = 0;
                m_ChannelCount = value;
                CreateChannels();
            }
        }

        [Browsable(false)]
        public int VarSize
        {
            get { return m_VarSize; }
        }

        [Category("Variable Info")]
        public BKPlcVariableType VarType
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                UpdateVarSize();
                m_Channels.Clear(); //type이 바뀌었으므로 다시 만들어야 함
                CreateChannels();
            }
        }
        #endregion

        #region Constructor
        public BKPlcOut()
        {
            //this.IoType = Dms.Common.IoType.DI;
            m_ChannelCount = 1;
            //m_VarSize = 1;
            UpdateVarSize();

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Output variable";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.BeckhoffPLC;
            m_ProductName = "BeckhoffPlc Output Variable";
            m_ProductId = 0;
        }
        #endregion

        #region Override
        protected override void UpdateVarSize()
        {
            m_VarSize = m_DicSize[m_Type];

            if (m_Type == BKPlcVariableType.Bool)
            {
                m_IoType = Dms.Common.IoType.DO;
            }
            else
            {
                m_IoType = Dms.Common.IoType.AO;
            }
        }

        public override void CreateChannels()
        {
            int curChnnelsCount = this.Channels.Count;

            if (curChnnelsCount == m_ChannelCount)
            {
                //Noop
            }
            else if (curChnnelsCount > m_ChannelCount)
            {
                //현재 등록된 채널에서 초과분 만큼 빼줘야 겠지
                for (int i = curChnnelsCount; i > m_ChannelCount; i--)
                {
                    this.Channels.RemoveAt(i - 1);
                }
            }
            else
            {
                //현재 등록된 채널에서 부족분 만큼 더해준다
                for (int i = curChnnelsCount; i < m_ChannelCount; i++)
                {
                    IoItem item = new IoItem();
                    switch (this.IoType)
                    {
                        case Dms.Common.IoType.DI:
                            item = new IoItemDI();
                            item.Name = "di__";
                            break;
                        case Dms.Common.IoType.DO:
                            item.Name = "do__";
                            break;
                        case Dms.Common.IoType.AI:
                            item.Name = "ai__";
                            break;
                        case Dms.Common.IoType.AO:
                            item.Name = "ao__";
                            break;

                    }

                    //item.Id = m_Offset + (m_VarSize * i);
                    item.Channel = i;
                    item.IoType = this.IoType;
                    this.Channels.Add(item);
                }
            }
            UpdateOffset();
        }
        #endregion
    }

    //[Serializable()]
    //public class BKPlcBitIn : BKPlcVariable
    //{
    //    //[Category("Variable Info")]
    //    //public int Size
    //    //{
    //    //    get { return m_ChannelCount; }
    //    //    set
    //    //    {
    //    //        if (value < 0) value = 0;
    //    //        m_ChannelCount = value;
    //    //        CreateChannels();
    //    //    }
    //    //}
    //    //[Category("Variable Info")]
    //    //public BKPlcVariableType VarType
    //    //{
    //    //    get { return m_Type; }
    //    //    set { m_Type = value; }
    //    //}

    //    public BKPlcBitIn()
    //    {
    //        this.IoType = Dms.Common.IoType.DI;
    //        m_ChannelCount = 1;
    //        m_VarSize = 1;

    //        //Parts Info
    //        m_PartCode = "";
    //        m_PartName = "";
    //        m_PartSpec = "";
    //        m_PartDescription = "Input variable";
    //        m_PartPrice = 0;

    //        //Product Info
    //        m_ProductMaker = Maker.BeckhoffPLC;
    //        m_ProductName = "BeckhoffPlc Input Variable";
    //        m_ProductId = 0;
    //    }

    //    //public override string ToString()
    //    //{
    //    //    return this.GetType().Name + "." + this.IoType;
    //    //    //return this.GetType().Name + "." + Enum.GetName(typeof(BKPlcVariableType), m_Type);
    //    //}
    //}

    //[Serializable()]
    //public class BKPlcBitOut : BKPlcVariable
    //{
    //    public BKPlcBitOut()
    //    {
    //        this.IoType = Dms.Common.IoType.DO;
    //        m_ChannelCount = 1;
    //        m_VarSize = 1;

    //        //Parts Info
    //        m_PartCode = "";
    //        m_PartName = "";
    //        m_PartSpec = "";
    //        m_PartDescription = "Output variable";
    //        m_PartPrice = 0;

    //        //Product Info
    //        m_ProductMaker = Maker.BeckhoffPLC;
    //        m_ProductName = "BeckhoffPlc Output Variable";
    //        m_ProductId = 0;
    //    }
    //}

    //[Serializable()]
    //public class BKPlcWordIn : BKPlcVariable
    //{
    //    public BKPlcWordIn()
    //    {
    //        this.IoType = Dms.Common.IoType.AI;
    //        m_ChannelCount = 1;
    //        m_VarSize = 2;

    //        //Parts Info
    //        m_PartCode = "";
    //        m_PartName = "";
    //        m_PartSpec = "";
    //        m_PartDescription = "Input variable";
    //        m_PartPrice = 0;

    //        //Product Info
    //        m_ProductMaker = Maker.BeckhoffPLC;
    //        m_ProductName = "BeckhoffPlc Input Variable";
    //        m_ProductId = 0;
    //    }
    //}

    //[Serializable()]
    //public class BKPlcWordOut : BKPlcVariable
    //{
    //    public BKPlcWordOut()
    //    {
    //        this.IoType = Dms.Common.IoType.AO;
    //        m_ChannelCount = 1;
    //        m_VarSize = 2;

    //        //Parts Info
    //        m_PartCode = "";
    //        m_PartName = "";
    //        m_PartSpec = "";
    //        m_PartDescription = "Output variable";
    //        m_PartPrice = 0;

    //        //Product Info
    //        m_ProductMaker = Maker.BeckhoffPLC;
    //        m_ProductName = "BeckhoffPlc Output Variable";
    //        m_ProductId = 0;
    //    }
    //}

    [Serializable()]
    [Browsable(false)]
    public class BKPlcStruct : BKPlcTerminal
    {
        #region Constructor
        public BKPlcStruct()
        { 
        }
        #endregion

        public bool IsContainedType(Dms.Common.IoType ioType)
        {
            foreach (IoItem item in m_Channels)
            {
                if (item.IoType == ioType)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable()]
    public class BKPlcStructIn : BKPlcStruct
    {
        #region Fields
        private List<BKPlcIn> m_Items = new List<BKPlcIn>();
        #endregion

        #region Properties
        [Category("Variable Info")]
        [Editor(typeof(UIEditorBKPlcInTerminal), typeof(UITypeEditor))]
        public List<BKPlcIn> Items
        {
            get { return m_Items; }
            set
            {
                m_Items = value;
                //m_ChannelCount = m_Items.Count;
                CreateChannels();
            }
        } 
        #endregion

        #region Constructor
        public BKPlcStructIn()
        {
            //this.IoType = Dms.Common.IoType.DI;
            m_ChannelCount = 0;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Input Structure";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.BeckhoffPLC;
            m_ProductName = "BeckhoffPlc Input Structure";
            m_ProductId = 0;
        } 
        #endregion

        #region Override
        protected override void UpdateOffset()
        {
            int count = m_Items.Count;
            int offset = m_Offset;
            BKPlcIn item;
            for (int i = 0; i < count; i++)
            {
                item = m_Items[i];
                item.Offset = offset;
                offset += (item.VarSize * item.ChannelCount);
            }
        }

        public override void CreateChannels()
        {
            m_Channels.Clear();
            foreach (BKPlcIn item in m_Items)
            {
                item.CreateChannels();
                foreach (IoItem io in item.Channels)
                {
                    m_Channels.Add(io);
                }
            }
            m_ChannelCount = m_Channels.Count;
            UpdateOffset();
        } 
        #endregion
    }

    [Serializable()]
    public class BKPlcStructOut : BKPlcStruct
    { 
        #region Fields
        private List<BKPlcOut> m_Items = new List<BKPlcOut>();
        #endregion

        #region Properties
        [Category("Variable Info")]
        [Editor(typeof(UIEditorBKPlcOutTerminal), typeof(UITypeEditor))]
        public List<BKPlcOut> Items
        {
            get { return m_Items; }
            set
            {
                m_Items = value;
                //m_ChannelCount = m_Items.Count;
                CreateChannels();
            }
        } 
        #endregion

        #region Constructor
        public BKPlcStructOut()
        {
            //this.IoType = Dms.Common.IoType.DI;
            m_ChannelCount = 0;

            //Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Output Structure";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.BeckhoffPLC;
            m_ProductName = "BeckhoffPlc Output Structure";
            m_ProductId = 0;
        } 
        #endregion

        #region Override
        protected override void UpdateOffset()
        {
            int count = m_Items.Count;
            int offset = m_Offset;
            BKPlcOut item;
            for (int i = 0; i < count; i++)
            {
                item = m_Items[i];
                item.Offset = offset;
                offset += (item.VarSize * item.ChannelCount);
            }
        }

        public override void CreateChannels()
        {
            m_Channels.Clear();
            foreach (BKPlcOut item in m_Items)
            {
                item.CreateChannels();
                foreach (IoItem io in item.Channels)
                {
                    m_Channels.Add(io);
                }
            }
            m_ChannelCount = m_Channels.Count;
            UpdateOffset();
        } 
        #endregion
    }*/
    #endregion

    [Serializable()]
    public class TwinCATPlcTerminal : IoTerminal
    {
        public TwinCATPlcTerminal()
        { 
        }
    }
}
