using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

namespace Dms.Cim.Common
{
    [Serializable]
    public class TagCurDataItem
    {
        private string m_name;
        private string m_value;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public TagCurDataItem()
        {
        }

        public void Clone(TagCurDataItem item)
        {
            this.m_name = item.m_name;
            this.m_value = item.m_value;
        }
    }
    
    [Serializable]
    public class TagApdItem
    {
        private string m_name;
        private string m_value;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public TagApdItem()
        {
        }

        public void Clone(TagApdItem item)
        {
            this.m_name = item.m_name;
            this.m_value = item.m_value;
        }
    }
    
    [Serializable]
    public class TagTraceItem
    {
        private string m_name;
        private string m_value;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public TagTraceItem()
        {
        }

        public void Clone(TagTraceItem item)
        {
            this.m_name = item.Name;
            this.m_value = item.Value;
        }
    }

    [Serializable]
    public class TagLotItem
    {
        private bool m_hostreport;
        private string m_name;
        private string m_type;
        private string m_value;
        private double m_maxvalue;
        private double m_avgvalue;
        private double m_minvalue;
        private double m_sumvalue;

        public bool HostReport
        {
            get { return m_hostreport; }
            set { m_hostreport = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public double MaxValue
        {
            get { return m_maxvalue; }
            set { m_maxvalue = value; }
        }

        public double AvgValue
        {
            get { return m_avgvalue; }
            set { m_avgvalue = value; }
        }

        public double MinValue
        {
            get { return m_minvalue; }
            set { m_minvalue = value; }
        }

        public double SumValue
        {
            get { return m_sumvalue; }
            set { m_sumvalue = value; }
        }

        public TagLotItem()
        {
        }

        public void Clone(TagLotItem item)
        {
            this.m_hostreport = item.m_hostreport;
            this.m_name = item.m_name;
            this.m_type = item.m_type;
            this.m_value = item.m_value;
            this.m_maxvalue = item.m_maxvalue;
            this.m_avgvalue = item.m_avgvalue;
            this.m_minvalue = item.m_minvalue;
            this.m_sumvalue = item.m_sumvalue;
        }
    }


    [Serializable]
    public class CurData
    {
        private List<TagCurDataItem> m_Items = new List<TagCurDataItem>();

        public List<TagCurDataItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagCurDataItem this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Add(TagCurDataItem tagCurDataItem)
        {
            m_Items.Add(tagCurDataItem);
        }
        /*
                public void Initialize(int MaxNo)
                {
                    for (int i = 0; i < MaxNo; i++)
                    {
                        TagApdItem tagApdItem = new TagApdItem();

                        this.Add(tagApdItem);
                    }
                }
        */
    }

    public class ApdData
    {
        private List<TagApdItem> m_Items = new List<TagApdItem>();

        public List<TagApdItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagApdItem this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Add(TagApdItem tagApdItem)
        {
            m_Items.Add(tagApdItem);
        }
/*
        public void Initialize(int MaxNo)
        {
            for (int i = 0; i < MaxNo; i++)
            {
                TagApdItem tagApdItem = new TagApdItem();

                this.Add(tagApdItem);
            }
        }
*/
    }

    [Serializable]
    public class TraceData
    {
        private List<TagTraceItem> m_Items = new List<TagTraceItem>();

        public List<TagTraceItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagTraceItem this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Add(TagTraceItem tagTraceItem)
        {
            m_Items.Add(tagTraceItem);
        }
        /*
                public void Initialize(int MaxNo)
                {
                    for (int i = 0; i < MaxNo; i++)
                    {
                        TagApdItem tagApdItem = new TagApdItem();

                        this.Add(tagApdItem);
                    }
                }
        */
    }

    [Serializable]
    public class LotData
    {
        private List<TagLotItem> m_Items = new List<TagLotItem>();

        public List<TagLotItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public TagLotItem this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Add(TagLotItem tagApdItem)
        {
            m_Items.Add(tagApdItem);
        }
    }



    [Serializable]
    public class ApdDatas
    {
        private Dictionary<string, TagApdItem> m_Items = new Dictionary<string, TagApdItem>();

        public ApdDatas()
        {
        }

        public Dictionary<string, TagApdItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public bool AddItem(string sKey, TagApdItem value)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return false;
                }
                else
                {
                    m_Items.Add(sKey, value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public TagApdItem GetItem(string sKey)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return m_Items[sKey];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public void SetItem(List<TagApdItem> apddata)
        {
            try
            {
                int count = apddata.Count;

                for (int i = 0; i < count; i++)
                {
                    TagApdItem item = apddata[i];

                    if (m_Items.ContainsKey(item.Name))
                    {
                        m_Items[item.Name] = item;
                    }
                }
            }
            catch //(System.Exception e)
            {
            	
            }
        }

        public Dictionary<string, TagApdItem>.KeyCollection GetKey()
        {
            return m_Items.Keys;
        }

        public ApdData GetItems()
        {
            ApdData apddata = new ApdData();

            Dictionary<string, TagApdItem>.ValueCollection values = m_Items.Values;
 
            foreach( TagApdItem apd in values)
            {
                if( apd != null ) apddata.Add( apd );
                else
                {
                    TagApdItem item = new TagApdItem();
                    item.Value = "0";

                    apddata.Add(item);
                }
            }

            return apddata;
        }

        public List<TagApdItem> GetValues()
        {
            List<TagApdItem> listitem = new List<TagApdItem>();

            Dictionary<string, TagApdItem>.ValueCollection values = m_Items.Values;

            foreach (TagApdItem apd in values)
            {
                if (apd != null) listitem.Add(apd);
            }

            return listitem;
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public bool Initialize( List<string> dvname)
        {
            int count = dvname.Count;

            try
            {
                for (int i = 0; i < count; i++)
                {
                    AddItem(dvname[i], null);
                }

                return true;
            }
            catch //(System.Exception e)
            {
                return false;            	
            }
        }
    }

    [Serializable]
    public class TraceDatas
    {
        private Dictionary<string, TagTraceItem> m_Items = new Dictionary<string, TagTraceItem>();

        public TraceDatas()
        {
        }

        public Dictionary<string, TagTraceItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public bool AddItem(string sKey, TagTraceItem value)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return false;
                }
                else
                {
                    m_Items.Add(sKey, value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public TagTraceItem GetItem(string sKey)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return m_Items[sKey];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public void SetItem(List<TagTraceItem> tracedata)
        {
            try
            {
                int count = tracedata.Count;

                for (int i = 0; i < count; i++)
                {
                    TagTraceItem item = tracedata[i];

                    if (m_Items.ContainsKey(item.Name))
                    {
                        m_Items[item.Name] = item;
                    }
                }
            }
            catch //(System.Exception e)
            {

            }
        }

        public Dictionary<string, TagTraceItem>.KeyCollection GetKey()
        {
            return m_Items.Keys;
        }

        public TraceData GetItems()
        {
            TraceData tracedata = new TraceData();

            Dictionary<string, TagTraceItem>.ValueCollection values = m_Items.Values;

            foreach (TagTraceItem apd in values)
            {
                if (apd != null) tracedata.Add(apd);
                else
                {
                    TagTraceItem item = new TagTraceItem();
                    item.Value = "0";

                    tracedata.Add(item);
                }
            }

            return tracedata;
        }

        public List<TagTraceItem> GetValues()
        {
            List<TagTraceItem> listitem = new List<TagTraceItem>();

            Dictionary<string, TagTraceItem>.ValueCollection values = m_Items.Values;

            foreach (TagTraceItem trace in values)
            {
                if (trace != null) listitem.Add(trace);
            }

            return listitem;
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public bool Initialize(List<string> dvname)
        {
            int count = dvname.Count;

            try
            {
                for (int i = 0; i < count; i++)
                {
                    AddItem(dvname[i], null);
                }

                return true;
            }
            catch //(System.Exception e)
            {
                return false;
            }
        }
    }

    public class tagLotData
    {
        private string dvname;
        private string dvval;

        public string Name
        {
            get { return dvname; }
            set { dvname = value; }
        }

        public string Val
        {
            get { return dvval; }
            set { dvval = value; }
        }

        public tagLotData()
        {

        }
    }

    public class LotDatas
    {
        private int m_ApdCount = 0;
        private List<tagLotData> m_LotData = new List<tagLotData>();
        private Dictionary<string, TagLotItem> m_Items = new Dictionary<string, TagLotItem>();

        public LotDatas()
        {
            m_ApdCount = 0;
        }

        public Dictionary<string, TagLotItem> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public bool AddItem(string sKey, TagLotItem value)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return false;
                }
                else
                {
                    m_Items.Add(sKey, value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public TagLotItem GetItem(string sKey)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return m_Items[sKey];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public void SetItem(List<TagApdItem> apddata, bool lastunit)
        {
            try
            {
                int count = apddata.Count;
                TagLotItem lotitem = null;

                for (int i = 0; i < count; i++)
                {
                    TagApdItem item = apddata[i];

                    double val = 0.0;

                    if (m_Items.ContainsKey(item.Name))
                    {
                        lotitem = GetItem(item.Name);

                        if (lotitem.Type == "ORG")
                        {
                            lotitem.Value = item.Value;
                        }
                        else if( lotitem.Type == "LOT" )
                        {
                            val = Convert.ToDouble(item.Value);

                            if (lotitem.MaxValue < val) lotitem.MaxValue = val;

                            if (m_ApdCount == 0 && lotitem.MinValue < val) lotitem.MinValue = val;
                            else if (lotitem.MinValue > val) lotitem.MinValue = val;

                            lotitem.SumValue += val;
                        }
                    }
                }

                if( lastunit ) m_ApdCount += 1;
            }
            catch //(System.Exception e)
            {

            }
        }

        public void Add(string dvname, string value)
        {
            if (m_Items.ContainsKey(dvname))
            {
                TagLotItem lotitem = GetItem(dvname);

                lotitem.Value = value;
            }
        }

        public void Add(string dvname, double max, double min, double ave)
        {
            if (m_Items.ContainsKey(dvname))
            {
                TagLotItem lotitem = GetItem(dvname);

                lotitem.MaxValue = max;
                lotitem.MinValue = min;
                lotitem.AvgValue = ave;
            }
        }

        public void Calculate()
        {
            foreach (TagLotItem item in m_Items.Values)
            {
                if (item.Type != "ORG")
                {
                    if( m_ApdCount > 0 ) item.AvgValue = item.SumValue / m_ApdCount;
                }
            }
        }

        public List<tagLotData> GetData()
        {
            m_LotData.Clear();

            foreach (TagLotItem item in m_Items.Values)
            {
                if (item.Type == "ORG")
                {
                    tagLotData data = new tagLotData();

                    data.Name = item.Name;
                    data.Val = item.Value;

                    m_LotData.Add(data);
                }
                else if( item.Type == "LOT")
                {
                    tagLotData Maxdata = new tagLotData();
                    Maxdata.Name = item.Name + "_MAX";
                    Maxdata.Val = item.MaxValue.ToString();
                    m_LotData.Add(Maxdata);

                    tagLotData Mindata = new tagLotData();
                    Mindata.Name = item.Name + "_MIN";
                    Mindata.Val = item.MinValue.ToString();
                    m_LotData.Add(Mindata);


                    tagLotData Avgdata = new tagLotData();
                    Avgdata.Name = item.Name + "_AVG";
                    Avgdata.Val = item.AvgValue.ToString();
                    m_LotData.Add(Avgdata);
                }
            }

            return m_LotData;
        }


        // 성도 천마랑 겹치기 때문에 새로 하나 만듬
        // 성도 천마는 아이템이 전부 보고 방식이였는데 History추가하며너서 전부 보고가 아닌 경우가 생김
        public List<tagLotData> GetDataReport()
        {
            m_LotData.Clear();

            foreach (TagLotItem item in m_Items.Values)
            {
                if (item.HostReport)
                {
                    if (item.Type == "ORG")
                    {
                        tagLotData data = new tagLotData();

                        data.Name = item.Name;
                        data.Val = item.Value;

                        m_LotData.Add(data);
                    }
                    else if (item.Type == "LOT")
                    {
                        tagLotData Maxdata = new tagLotData();
                        Maxdata.Name = item.Name + "_MAX";
                        Maxdata.Val = item.MaxValue.ToString();
                        m_LotData.Add(Maxdata);

                        tagLotData Mindata = new tagLotData();
                        Mindata.Name = item.Name + "_MIN";
                        Mindata.Val = item.MinValue.ToString();
                        m_LotData.Add(Mindata);


                        tagLotData Avgdata = new tagLotData();
                        Avgdata.Name = item.Name + "_AVG";
                        Avgdata.Val = item.AvgValue.ToString();
                        m_LotData.Add(Avgdata);
                    }
                }
            }

            return m_LotData;
        }

        public Dictionary<string, TagLotItem>.KeyCollection GetKey()
        {
            return m_Items.Keys;
        }

        public LotData GetItems()
        {
            LotData lotdata = new LotData();

            Dictionary<string, TagLotItem>.ValueCollection values = m_Items.Values;

            foreach (TagLotItem item in values)
            {
                if (item.Type == "ORG")
                {
                    lotdata.Add(item);
                }
                else if (item.Type == "LOT")
                {
                    TagLotItem MaxItem = new TagLotItem();
                    MaxItem.Name = item.Name + "_MAX";
                    MaxItem.Value = item.MaxValue.ToString();
                    lotdata.Add(MaxItem);

                    TagLotItem MinItem = new TagLotItem();
                    MinItem.Name = item.Name + "_MIN";
                    MinItem.Value = item.MinValue.ToString();
                    lotdata.Add(MinItem);

                    TagLotItem AvgItem = new TagLotItem();
                    AvgItem.Name = item.Name + "_AVG";
                    AvgItem.Value = item.AvgValue.ToString();
                    lotdata.Add(AvgItem);
                }
            }

            return lotdata;
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public bool Initialize( List<TagLotItem> dvname)
        {
            int count = dvname.Count;

            try
            {
                for (int i = 0; i < count; i++)
                {
                    TagLotItem item = new TagLotItem();

                    item.HostReport = dvname[i].HostReport;
                    item.Name = dvname[i].Name;
                    item.Type = dvname[i].Type;
                    item.Value = "0";
                    item.MaxValue = 0.0;
                    item.MinValue = 0.0;
                    item.AvgValue = 0.0;
                    item.SumValue = 0.0;

                    AddItem(dvname[i].Name, item);
                }

                return true;
            }
            catch //(System.Exception e)
            {
                return false;
            }
        }

        public void Clear()
        {
            foreach (TagLotItem item in m_Items.Values)
            {
                item.MaxValue = 0.0;
                item.MinValue = 0.0;
                item.AvgValue = 0.0;
                item.SumValue = 0.0;
                item.Value = "0";
            }
        }
    }

    [Serializable]
    public class EcidDefaultValules
    {
        private Dictionary<string, string> m_Items = new Dictionary<string, string>();

        public EcidDefaultValules()
        {
        }

        public Dictionary<string, string> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public bool AddItem(string sKey, string value)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return false;
                }
                else
                {
                    m_Items.Add(sKey, value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public string GetItem(string sKey)
        {
            try
            {
                if (m_Items.ContainsKey(sKey))
                {
                    return m_Items[sKey];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public Dictionary<string, string>.KeyCollection GetKey()
        {
            return m_Items.Keys;
        }

        public List<string> GetValues()
        {
            List<string> listitem = new List<string>();

            Dictionary<string, string>.ValueCollection values = m_Items.Values;

            foreach (string val in values)
            {
                if (val != null && val != "") listitem.Add(val);
            }

            return listitem;
        }

        public int Count
        {
            get { return m_Items.Count; }
        }
    }


}
