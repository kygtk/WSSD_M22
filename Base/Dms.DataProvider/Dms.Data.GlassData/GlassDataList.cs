using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Data
{
    [Serializable()]
    public class GlassDataList
    {
        private static object m_LockKey = new object();

        private List<TagGlassData> m_Items = new List<TagGlassData>();

        public List<TagGlassData> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        private static int m_FindKeyPositionId = 0;
        private static bool FindPositionId(TagGlassData source)
        {
            return source.PositionId == m_FindKeyPositionId;
        }

        public bool IsExist(int positionId)
        {
            TagGlassData data = Find(positionId);
            bool exist = data != null;
            return exist;
        }

        public TagGlassData Find(int positionId)
        {
            lock (m_LockKey)
            {
                m_FindKeyPositionId = positionId;

                TagGlassData find = m_Items.Find(FindPositionId);

                return find;
            }
        }

        public bool Add(TagGlassData item)
        {
            lock (m_LockKey)
            {
                bool find = IsExist(item.PositionId);

                if (!find)
                {
                    m_Items.Add(item);
                }

                return !find;
            }
        }

        public bool Remove(int positionId)
        {
            lock (m_LockKey)
            {
                TagGlassData find = Find(positionId);
                if (find == null)
                {
                    return false;
                }
                else
                {
                    m_Items.Remove(find);
                    return true;
                }
            }
        }

        public bool Remove(TagGlassData item)
        {
            lock (m_LockKey)
            {
                TagGlassData find = Find(item.PositionId);
                if (find == null)
                {
                    return false;
                }
                else
                {
                    m_Items.Remove(find);
                    return true;
                }
            }
        }

        public bool Move(int fromPosition, int toPosition)
        {
            lock (m_LockKey)
            {
                TagGlassData from = Find(fromPosition);
                TagGlassData to = Find(toPosition);

                bool valid = true;
                valid &= from != null;
                valid &= to == null;

                if (valid)
                {
                    from.PositionId = toPosition;
                }

                return valid;
            }
        }

        public bool Edit(int positionId, TagGlassData item)
        {
            lock (m_LockKey)
            {
                TagGlassData data = Find(positionId);
                bool ok = (data != null);
                if (ok)
                {
                    data.Clone(item);
                    data.PositionId = positionId;
                }

                return ok;
            }
        }

        public bool WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = null;

            try
            {
                xmlSer = new XmlSerializer(this.GetType());

                // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.Message.ToString());

                if (sw != null) sw.Close();

                return false;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.CopyTo(fileName + ".old", true);
                }

                sw = new StreamWriter(fileName);
                xmlSer.Serialize(sw, this);
                sw.Close();

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.Message.ToString());

                if (sw != null) sw.Close();

                return false;
            }
        }

        public bool ReadXml(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());
                GlassDataList readData;
                readData = xmlSer.Deserialize(sr) as GlassDataList;
                sr.Close();

                m_Items.Clear();
                m_Items = readData.Items;

                foreach (TagGlassData item in m_Items)
                {
                    item.Update();
                }

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.Message.ToString());
                return false;
            }
        }
    }
}
