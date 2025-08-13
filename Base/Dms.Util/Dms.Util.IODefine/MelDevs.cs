///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.10.23
// Author       : jemoon
// Description  : Melsec Devices 
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class MelDevs : MelsecTerminal
    {
        [Category("Address")]
        public int Size
        {
            get { return m_ChannelCount; }
            set 
            {
                if (value < 1) value = 1;
                // Melsec API의 Read/Write시 Byte단위만 허용됨.
				//if (m_IoType == IoType.DI || m_IoType == IoType.DO)
				//{
				//    value = ((value / 8) + ((value % 8) > 0 ? 1 : 0)) * 8;
				//    //if (value > 512) value = 512;
				//}
                m_ChannelCount = value;
                CreateChannels();
            }
        }

        // MelDevs 
        #region Constructor
        public MelDevs()
        {
        }
        #endregion

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
        }
    }
}
