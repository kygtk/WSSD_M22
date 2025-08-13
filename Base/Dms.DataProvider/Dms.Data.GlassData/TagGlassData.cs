using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Data
{
    [Flags]
    public enum GlassTypeFlags
    {
        GlassIdUpdated = 0x01 << 11,
        GlassDataScraped = 0x01 << 13,
        GlassDataUpdated = 0x01 << 15
    }


    [Serializable()]
    public class GlassNumberCode
    {
        public ushort Code;
        public ushort LotNo
        {
            get { return (ushort)((Code >> 8) & 0xFF); }
            set
            {
                Code = (ushort)(((value & 0xFF) << 8) | (Code & 0x00FF));
            }
        }
        public ushort SlotNo
        {
            get { return (ushort)(Code & 0x00FF); }
            set
            {
                Code = (ushort)((Code & 0xFF00) | (value & 0x00FF));
            }
        }

        #region Constructor
        public GlassNumberCode()
        {
        }

        public GlassNumberCode(ushort code)
        {
            Code = code;
        }

        public GlassNumberCode(string lotNo, string slotNo)
        {
            try
            {
                ushort ln = Convert.ToUInt16(lotNo);
                LotNo = ln;
                ushort sn = Convert.ToUInt16(slotNo);
                SlotNo = sn;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public GlassNumberCode(ushort lotNo, ushort slotNo)
        {
            LotNo = lotNo;
            SlotNo = slotNo;
        }
        #endregion

        public void Clone(GlassNumberCode code)
        {
            this.Code = code.Code;
        }

        public override string ToString()
        {
            return this.Code.ToString();
        }
    }

    public class TransferData // 10.12.25 minhan
    {
        public GlassNumberCode GlassNumberCode;
        public bool Processed;
        public string LotID;
        public string PortID;
        public string SlotID;
        public string PortNo;
        public string GlassID;
        public int GlassCode;
        public string RecipeID;
        public string CstID;
        public bool TRCrackAlarm; // 11.06.01 minhan
        public bool LoaderCrackAlarm;

        private Dictionary<string, string> m_DisplayedItem = new Dictionary<string, string>();

        public TransferData()
        {
            GlassNumberCode = new GlassNumberCode(1, 1);
            Processed = true;
            LotID = "DUMMY";
            GlassID = "DUMMY";
            CstID = "DUMMY";
            RecipeID = "1";
            PortID = "0";
            SlotID = "0";
            GlassCode = 0;
            TRCrackAlarm = false; // 11.06.01 minhan
            LoaderCrackAlarm = false;
        }
        public void UpdateGlassNoCode(string LotId, string SlotId) // 11.02.17 minhan
        {
            ushort buf;
            if (!UInt16.TryParse(LotId, out buf))
                GlassNumberCode.LotNo = 0;
            else
                GlassNumberCode.LotNo = buf;
            if (!UInt16.TryParse(SlotId, out buf))
                GlassNumberCode.SlotNo = 0;
            else
                GlassNumberCode.SlotNo = buf;
        }
        public void UpdateData(ushort lotNo, ushort slotNo)
        {
            GlassNumberCode.LotNo = lotNo;
            GlassNumberCode.SlotNo = slotNo;
        }

        public void UpdateData(ushort glassCode, string glassID, string recipeID)
        {
            try
            {
                GlassNumberCode.Code = glassCode;
                GlassID = glassID;
                RecipeID = recipeID;
            }
            catch (Exception e)
            {
                GlassNumberCode.Code = 0xFFFF;
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public void UpdateData(string code)
        {
            try
            {
                GlassNumberCode.Code = Convert.ToUInt16(code);
            }
            catch (Exception e)
            {
                GlassNumberCode.Code = 0xFFFF;
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public void UpdateData(int code)
        {
            GlassNumberCode.Code = (ushort)code;
        }

        public void UpdateData(TagGlassData glassData)
        {
            //GlassNumberCode.Code = glassData.GlassCode.Code;//2010.06.20 kimgun 삭제
            Processed = glassData.Processed;
            LotID = glassData.LotID;
            PortID = glassData.PortID;
            GlassID = glassData.GlassID;
            SlotID = glassData.SlotID;
            GlassCode = glassData.GlassCode;
            RecipeID = glassData.RecipeID;
            CstID = glassData.CstID; // 10.12.27 minhan
            TRCrackAlarm = glassData.TRCrackAlarm; // 11.06.01 minhan
            LoaderCrackAlarm = glassData.LoaderCrackAlarm; // 11.06.01 minhan
        }

        //Glass Data Dialog에 보여질 아이들을 설정한다.
        //GLSCODE, Glass ID, EQP Recipe Number, LOT ID, Processing Code, Glass Judge
        //여기에 추가된 아이들은 밑에 SetTagGlassDatabyDisplayedItem에도 추가해야 함.
        public Dictionary<string, string> GetDisplayedItem()
        {
            m_DisplayedItem.Add("Lot ID", LotID);// GlassNumberCode.LotNo.ToString());//2010.06.20 kimgun
            m_DisplayedItem.Add("Port No", PortID);//2010.06.20 kimgun
            m_DisplayedItem.Add("Slot No", SlotID);//GlassNumberCode.SlotNo.ToString());//2010.06.20 kimgun
            m_DisplayedItem.Add("Glass ID", GlassID);
            m_DisplayedItem.Add("Recipe ID", RecipeID);
            m_DisplayedItem.Add("Cst ID", CstID); // 10.12.27 minhan
            //m_DisplayedItem.Add("Glass Code", GlassCode.ToString()); // 11.02.09 minhan
            m_DisplayedItem.Add("Processed", Processed.ToString());//2010.06.20 kimgun
            m_DisplayedItem.Add("TRCrackAlarm", TRCrackAlarm.ToString()); // 11.06.01 minhan
            m_DisplayedItem.Add("LoaderCrackAlarm", LoaderCrackAlarm.ToString()); // 11.06.01 minhan

            return m_DisplayedItem;
        }

        public void SetTagGlassDatabyDisplayedItem(TagGlassData origin, Dictionary<string, string> displayedItem)
        {
            foreach (KeyValuePair<string, string> item in displayedItem)
            {
                if (item.Key == "Lot ID")
                {
                    origin.LotID = item.Value;
                }
                else if (item.Key == "Port No")//2010.06.20 kimgun
                {
                    origin.PortID = item.Value;
                }
                else if (item.Key == "Slot No")
                {
                    origin.SlotID = item.Value;
                }
                else if (item.Key == "Glass ID")
                {
                    origin.GlassID = item.Value;
                }
                else if (item.Key == "Recipe ID")
                {
                    origin.RecipeID = item.Value;
                }
                else if (item.Key == "Cst ID") // 10.12.25 minhan
                {
                    origin.CstID = item.Value;
                }
                else if (item.Key == "Glass Code")//2010.06.20 kimgun
                {
                    origin.GlassCode = Convert.ToUInt16(item.Value);
                }
                else if (item.Key == "Processed")//2010.06.20 kimgun
                {
                    origin.Processed = item.Value == "1";
                }
                else if (item.Key == "TRCrackAlarm") // 11.06.01 minhan
                {
                    origin.TRCrackAlarm = item.Value == "1";
                }
                else if (item.Key == "LoaderCrackAlarm") // 11.06.01 minhan
                {
                    origin.LoaderCrackAlarm = item.Value == "1";
                }
            }
        }

        public TransferData Clone()
        {
            TransferData data = new TransferData();
            data.GlassNumberCode.Code = this.GlassNumberCode.Code;
            data.Processed = this.Processed;
            data.LotID = this.LotID;
            data.PortID = this.PortID;
            data.SlotID = this.SlotID;
            data.GlassCode = this.GlassCode;
            data.GlassID = this.GlassID;
            data.RecipeID = this.RecipeID;
            data.CstID = this.CstID; // 10.12.25 minhan
            data.TRCrackAlarm = this.TRCrackAlarm; // 11.06.01 minhan
            data.LoaderCrackAlarm = this.LoaderCrackAlarm;
            //2010.06.21 kimgun
            //기존 base가 port no와 slot no를 GlassNumberCode에서 댕겨가기때문에..
            //GlassNumberCode는 일부 특정 사이트중 Melsec을 이용하여 한 word에 portNo와 SlotNo를 같이 넣어줄때 
            //parsing하려고 만든 것인데 LGD에서는 필요없기때문에 PortID와 SlotID만 넘겨주면 된다.
            ushort PortId;
            ushort SlotId;
            if (ushort.TryParse(this.PortID, out PortId) &&
                ushort.TryParse(this.SlotID, out SlotId))
                UpdateData(PortId, SlotId);
            return data;
        }
    }

    [Serializable()]
    public class TagGlassData // 10.12.25 minhan 이거 정리해야 하는데 참...마음 같아서는 싹 밀어버리고 싶네...--
    {
        public int PositionId = 0;
        public bool Processed = false;
        public string LotID = "DUMMY"; //16 Char
        public string RecipeID = "1"; //24 Char
        public string GlassID = "DUMMY"; //16 Char
        public int GlassCode = 0;
        public string CstID = "DUMMY"; //14 Char
        public string SlotID = "0"; //13 Char
        public string PortID = "0";
        public bool TRCrackAlarm = false; // 11.06.01 minhan
        public bool LoaderCrackAlarm = false;


        private TransferData m_Item = new TransferData();
        public TransferData Item
        {
            get { return m_Item; }
        }

        public TagGlassData()
        {

        }
        public void Clone(TagGlassData data)
        {
            this.PositionId = data.PositionId;
            this.Processed = data.Processed;
            this.LotID = data.LotID;
            this.RecipeID = data.RecipeID;
            this.GlassCode = data.GlassCode;
            this.GlassID = data.GlassID;
            this.CstID = data.CstID;
            this.SlotID = data.SlotID;
            this.PortID = data.PortID;
            this.TRCrackAlarm = data.TRCrackAlarm; // 11.06.01 minhan
            this.LoaderCrackAlarm = data.LoaderCrackAlarm;

            m_Item = data.Item.Clone();
            Update();

        }

        public bool Update()
        {
            m_Item.UpdateData(this);
            m_Item.UpdateGlassNoCode(this.PortID, this.SlotID); // 11.02.25 minhan
            return true;
        }
    }
}
