using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class LostGlassDataProvider
    {
        #region Fields
        private LostGlassDataAdapter m_Adapter = null;
        #endregion

        #region Properties
        public LostGlassDataAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public int Count
        {
            get
            {
                lock (this)
                {
                    return m_Adapter.Table.Rows.Count;
                }
            }
        }
        #endregion

        #region Singleton code...
        public static readonly LostGlassDataProvider Instance = new LostGlassDataProvider();
        #endregion

        #region Constructor
        private LostGlassDataProvider()
        {
            m_Adapter = new LostGlassDataAdapter();
        }
        #endregion

        #region Methods
        public void DeleteAll()
        {
            lock (this)
            {
                m_Adapter.ClearDB();
            }
        }

        public void LoadFromDB()
        {
            lock (this)
            {
                m_Adapter.LoadFromDB();
            }
        }

        public void UpdateToDB()
        {
            lock (this)
            {
                m_Adapter.UpdateToDB();
            }
        }

        //public bool IsExist(int positionId)
        //{
        //    lock (this)
        //    {
        //        return m_Adapter.IsExist(positionId);
        //    }
        //}

        public bool GetData(int no, ref TagGlassData item)
        {
            lock (this)
            {
                return m_Adapter.GetData(no, ref item);
            }
        }

        public bool Create(TagGlassData item)
        {
            lock (this)
            {
                return m_Adapter.Add(item);
            }
        }

        //public bool Move(int fromPosition, int toPosition)
        //{
        //    return m_Adapter.Move(fromPosition, toPosition);
        //}

        public bool Delete(int no)
        {
            return m_Adapter.Remove(no);
        }

        //public bool Update(int position, TagGlassData toChange)
        //{
        //    return m_Adapter.Edit(position, toChange);
        //}

        public void DeleteDbGarbage(int maxDataId)
        {
            lock (this)
            {
                m_Adapter.DeleteGarbage(maxDataId);
            }
        }

        //public void GetAllPositionId(out int[] ids)
        //{
        //    lock (this)
        //    {
        //        m_Adapter.GetAllPositionId(out ids);
        //    }
        //}
        #endregion
    }
}
