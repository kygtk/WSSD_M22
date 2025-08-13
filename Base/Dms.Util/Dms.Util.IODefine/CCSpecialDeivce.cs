///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.26
// Author       : jemoon
// Description  : CC Link(CCSpecialDeivce)
//-------------------------------------------------------------------------
// Revison History
// * jemoon : 100503 - 사용되지 않은 변수 정리
//
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public enum AddressAllocType
    {
        Exclusive,  // X(n) -> Y(n+1)
        Inclusive   // X(n) -> Y(n)
    }

    [Serializable()]
    abstract public class CCSpecialDeivce : CClinkStation
    {
        #region Fields
        //protected AddressAllocType m_AddressAllocType = AddressAllocType.Inclusive;
        protected IoDataType m_IoDataType = IoDataType.Digital;
        //protected int m_SpecialChannelCount = 32;
        #endregion

        #region Properties
        //[Category("Option"), Description("Exclusive : X(n) -> Y(n+1), Inclusive : X(n) -> Y(n)")]
        //public AddressAllocType AddressAllocType
        //{
        //    get { return m_AddressAllocType; }
        //    //set { m_AddressAllocType = value; }
        //}
        //[Category("Option"), Description("Digital / Analog")]
        //public IoDataType IoDataType
        //{
        //    get { return m_IoDataType; }
        //    //set { m_IoDataType = value; }
        //}
        //[Category("Option"), Description("Station data points")]
        //public int SpecialChannelCount
        //{
        //    get { return m_SpecialChannelCount; }
        //}
        #endregion

        // CCSpecialDeivce
        #region Constructor
        public CCSpecialDeivce()
        {
        }
        #endregion

        #region Methods
        //jemoon : 사용하지 않음
        //public IoTerminal CreateTerminal(int localIndex)
        //{
        //    CCDummy terminal = new CCDummy();
        //    terminal.ChannelCount = m_ChannelCount / 2;

        //    switch (m_IoDataType)
        //    {
        //        case IoDataType.Digital:
        //            {
        //                terminal.IoType = (localIndex == 0) ? IoType.DI : IoType.DO;
        //            }
        //            break;
        //        case IoDataType.Analog:
        //            {
        //                terminal.IoType = (localIndex == 0) ? IoType.AI : IoType.AO;
        //            }
        //            break;
        //    }

        //    terminal.CreateChannels();
        //    return terminal;
        //}
        #endregion

        #region Override
        public override void CreateChannels()
        {
            this.Channels.Clear();

            int channelCount = m_ChannelCount / 2;

            switch (m_IoDataType)
            {
                case IoDataType.Digital:
                    {
                        for (int i = 0; i < channelCount; i++)
                        {
                            IoItem item = new IoItemDI();
                            item.Name = "di__";
                            item.Channel = i;
                            item.IoType = IoType.DI;
                            this.Channels.Add(item);
                        }
                        for (int i = 0; i < channelCount; i++)
                        {
                            IoItem item = new IoItem();
                            item.Name = "do__";
                            item.Channel = m_Info.AddressAllocType == AddressAllocType.Exclusive ? (i + channelCount) : i;
                            item.IoType = IoType.DO;
                            this.Channels.Add(item);
                        }
                    }
                    break;
                case IoDataType.Analog:
                    {
                        for (int i = 0; i < channelCount; i++)
                        {
                            IoItem item = new IoItem();
                            item.Name = "ai__";
                            item.Channel = i;
                            item.IoType = IoType.AI;
                            this.Channels.Add(item);
                        }
                        for (int i = 0; i < channelCount; i++)
                        {
                            IoItem item = new IoItem();
                            item.Name = "ao__";
                            item.Channel = m_Info.AddressAllocType == AddressAllocType.Exclusive ? i + channelCount : i;
                            item.IoType = IoType.AO;
                            this.Channels.Add(item);
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}
