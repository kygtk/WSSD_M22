///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2025.05.13
// Author       : byeongmin
// Description  : CC Link Inverter Control
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class CCV1000 : CCSpecialDeivce
    {
        #region Fields
        private string m_Name = "CCV1000";
        #endregion

        #region Properties
        public string Name
        {
            get { return m_Name; }
            set
            {
                RenewalChannelsName(m_Name, value);
                m_Name = value;
            }
        }
        #endregion

        #region Constructor
        public CCV1000()
        {
            //  Station Info
            m_Info.StationOccupies = 1;
            m_Info.StationType = CclinkStationType.RemoteDevice;
            m_Info.TerminalType = CclinkTerminalType.Inverter;
            m_Info.Points = 1;

            m_IoDataType = Common.IoDataType.Mix;

            m_IoType = Common.IoType.Mix;
            m_ChannelCount = 32 + 32 + 4 + 4;   //  DI + DO + RWr + RWw

            //  Parts Info
            m_PartCode = "";
            m_PartName = "";
            m_PartSpec = "";
            m_PartDescription = "Inverter Control";
            m_PartPrice = 0;

            //  Product Info
            m_ProductMaker = Maker.MitsubishiCCLink;    //  Yaskawa 인버터이지만, 일단 설정
            m_ProductName = "Yaskawa Inverter V1000 Series";
            m_ProductId = 0;
        }
        #endregion

        #region Overrides
        public override void CreateChannels()
        {
            m_Channels.Clear();

            //  DI 32
            for (int i = 0; i < 32; i++)
            {
                IoItem item = new IoItemDI();
                switch (i)
                {
                    case 0x00:
                        item.Name = "di_" + m_Name + "_FWD";
                        break;
                    case 0x01:
                        item.Name = "di_" + m_Name + "_REV";
                        break;
                    case 0x02:
                        item.Name = "di_" + m_Name + "_H2_01";
                        break;
                    case 0x03:
                        item.Name = "di_" + m_Name + "_VA";
                        break;
                    case 0x04:
                        item.Name = "di_" + m_Name + "_STALL_PREVENTION";
                        break;
                    case 0x05:
                        item.Name = "di_" + m_Name + "_UV";
                        break;
                    case 0x06:
                        item.Name = "di_" + m_Name + "_H2_02";
                        break;
                    case 0x07:
                        item.Name = "di_" + m_Name + "_H2_03";
                        break;

                    case 0x0A:
                        item.Name = "di_" + m_Name + "_CCLink_ERR";
                        break;
                    case 0x0B:
                        item.Name = "di_" + m_Name + "_MONIT_ING";
                        break;
                    case 0x0C:
                        item.Name = "di_" + m_Name + "_MONIT_END";
                        break;
                    case 0x0D:
                        item.Name = "di_" + m_Name + "_VW_END_1";
                        break;
                    case 0x0E:
                        item.Name = "di_" + m_Name + "_VW_END_2";
                        break;
                    case 0x0F:
                        item.Name = "di_" + m_Name + "_CW_END";
                        break;

                    case 0x1A:
                        item.Name = "di_" + m_Name + "_ERR";
                        break;
                    case 0x1B:
                        item.Name = "di_" + m_Name + "_RDY";
                        break;

                    default:
                        item.Name = "di_Spare";
                        break;
                }
                item.Channel = i;
                item.IoType = Common.IoType.DI;
                m_Channels.Add(item);
            }

            //  DO 32
            for (int i = 0; i < 32; i++)
            {
                IoItem item = new IoItem();
                switch (i)
                {
                    case 0x00:
                        item.Name = "do_" + m_Name + "_FWD";
                        break;
                    case 0x01:
                        item.Name = "do_" + m_Name + "_REV";
                        break;
                    case 0x02:
                        item.Name = "do_" + m_Name + "_TERM_S3";
                        break;
                    case 0x03:
                        item.Name = "do_" + m_Name + "_TERM_S4";
                        break;
                    case 0x04:
                        item.Name = "do_" + m_Name + "_TERM_S5";
                        break;
                    case 0x05:
                        item.Name = "do_" + m_Name + "_TERM_S6";
                        break;
                    case 0x06:
                        item.Name = "do_" + m_Name + "_TERM_S7";
                        break;

                    case 0x09:
                        item.Name = "do_" + m_Name + "_OUT_CUTOFF";
                        break;
                    case 0x0A:
                        item.Name = "do_" + m_Name + "_EXT_ERR";
                        break;
                    case 0x0B:
                        item.Name = "do_" + m_Name + "_RWR1_SWITCHING";
                        break;
                    case 0x0C:
                        item.Name = "do_" + m_Name + "_MONIT_REQ";
                        break;
                    case 0x0D:
                        item.Name = "do_" + m_Name + "_VW_REQ_1";
                        break;
                    case 0x0E:
                        item.Name = "do_" + m_Name + "_VW_REQ_2";
                        break;
                    case 0x0F:
                        item.Name = "do_" + m_Name + "_CW_REQ";
                        break;

                    case 0x14:
                        item.Name = "do_" + m_Name + "_TERM_S1";
                        break;
                    case 0x15:
                        item.Name = "do_" + m_Name + "_TERM_S2";
                        break;

                    case 0x1A:
                        item.Name = "do_" + m_Name + "_ERR_RESET";
                        break;

                    default:
                        item.Name = "do_Spare";
                        break;
                }
                item.Channel = i;
                item.IoType = Common.IoType.DO;
                m_Channels.Add(item);
            }

            //  AI 4 (RWr)
            for (int i = 0; i < 4; i++)
            {
                IoItem item = new IoItem();
                switch (i)
                {
                    case 0:
                        item.Name = "ai_" + m_Name + "_MonitorValue";
                        break;
                    case 1:
                        item.Name = "ai_" + m_Name + "_CurrentFREQ";
                        break;
                    case 2:
                        item.Name = "ai_" + m_Name + "_ResponseCode";
                        break;
                    case 3:
                        item.Name = "ai_" + m_Name + "_CallingData";
                        break;
                }
                item.Channel = i;
                item.IoType = Common.IoType.AI;
                m_Channels.Add(item);
            }

            //  AO 4 (RWw)
            for (int i = 0; i < 4; i++)
            {
                IoItem item = new IoItem();
                switch (i)
                {
                    case 0:
                        item.Name = "ao_" + m_Name + "_MonitorCode";
                        break;
                    case 1:
                        item.Name = "ao_" + m_Name + "_SettingFREQ";
                        break;
                    case 2:
                        item.Name = "ao_" + m_Name + "_CommandCode";
                        break;
                    case 3:
                        item.Name = "ao_" + m_Name + "_RecordingData";
                        break;
                }
                item.Channel = i;
                item.IoType = Common.IoType.AO;
                m_Channels.Add(item);
            }
        }

        private void RenewalChannelsName(string Pre_Name, string New_Name)
        {
            if (Pre_Name == "") return;
            foreach (IoItem io in m_Channels)
            {
                io.Name = io.Name.Replace(Pre_Name, New_Name);
            }
        }
        #endregion
    }
}
