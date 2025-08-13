using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;
using Dms.Data.DataSetAlarmTableAdapters;

namespace Dms.Data
{
    public class AlarmList
    {
        #region Fields
        private List<TagAlarm> m_Items = new List<TagAlarm>();
        #endregion

        #region Properties
        public List<TagAlarm> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion

        #region Constructor
        public AlarmList()
        {       
        }
        #endregion

        #region Destructor
        
        #endregion

        #region Methods
        public bool GetAlarm(int alarmId, TagAlarm alarm)
        {
            bool exist = false;
            int count = this.Count;
            TagAlarm temp;
            for (int i = 0; i < count; i++)
            {
                temp = Items[i];
                if (alarmId == temp.Id)
                {
                    alarm.Clone(temp);
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                alarm.Id = alarmId;
                alarm.Name = "Not Defined Alarm";
                alarm.Level = AlarmLevel.S;
                alarm.Code = AlarmCode.ParameterControlError;
            }

            return exist;
        }
       
        public void ReadXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "AlarmList");

                StreamReader sr = new StreamReader(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(AlarmList));

                AlarmList alarmList = xmlSer.Deserialize(sr) as AlarmList;

                m_Items.Clear();
                m_Items = alarmList.Items;
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void WriteXml()
        {
            try
            {
                string dirName = "Database";
                string fileName = string.Format("{0}\\{1}.xml", dirName, "AlarmList");

                StreamWriter sw = new StreamWriter(fileName);
                XmlSerializer xmlSer = new XmlSerializer(typeof(AlarmList));
                xmlSer.Serialize(sw, this);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void WriteText(string fileName)
        {
            try
            {
                StreamWriter sw = File.CreateText(fileName);
                sw.AutoFlush = true;

                string txt = "ID\tLevel\tCode\tDescription";
                sw.WriteLine(txt);
                sw.WriteLine("================================================================");

                foreach (TagAlarm alarm in m_Items)
                {
                    txt = string.Format("{0:d4}\t{1}\t{2}\t{3}", alarm.Id, alarm.Level, (int)alarm.Code, alarm.Name);
                    sw.WriteLine(txt);
                }

                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void WriteText()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save As...";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            dlg.FileName = this.GetType().Name + ".txt";
            dlg.DefaultExt = "txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                try
                {
                    this.WriteText(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void SaveToDB()
        {
            AlarmListAdapter adapter = new AlarmListAdapter();
            adapter.SaveToDB(this);
        }

        //public void LoadFromDB()
        //{
        //    AlarmListAdapter adapter = new AlarmListAdapter();
        //    adapter.LoadFromDB(this);
        //}
        #endregion
    }
}
