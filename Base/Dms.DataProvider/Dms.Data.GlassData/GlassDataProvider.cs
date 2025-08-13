using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class GlassDataProvider
    {
        #region Fields
        private GlassDataAdapter m_Adapter = null;
        #endregion

        #region Properties
        public GlassDataAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public int Count
        {
            get 
            {
                lock (this)
                {
					return m_Adapter.Count;
                }
            }
        }
        #endregion

        #region Singleton code...
        public static readonly GlassDataProvider Instance = new GlassDataProvider();
        #endregion

        #region Constructor
        private GlassDataProvider()
        {
            m_Adapter = new GlassDataAdapter();
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

        //public void UpdateFromDB()
        //{
        //    lock (this)
        //    {
        //        m_Adapter.UpdateFromDB();
        //    }
        //}

        protected void UpdateToDB()
        {
            lock (this)
            {
                m_Adapter.UpdateToDB();
            }
        }

        public bool IsExist(int positionId)
        {
            lock (this)
            {
                return m_Adapter.IsExist(positionId);
            }
        }

        public bool GetData(int positionId, ref TagGlassData item)
        {
            lock (this)
            {
                return m_Adapter.GetData(positionId, ref item);
            }
        }

        public bool Create(TagGlassData item)
        {
            lock (this)
            {
                return m_Adapter.Add(item);
            }
        }

        public bool Move(int fromPosition, int toPosition)
        {
            return m_Adapter.Move(fromPosition, toPosition);
        }

        public bool Delete(int position)
        {
            return m_Adapter.Remove(position);
        }

        public bool Update(int position, TagGlassData toChange)
        {
            return m_Adapter.Edit(position, toChange);
        }

        public void DeleteDbGarbage(int maxDataId)
        {
            lock (this)
            {
                m_Adapter.DeleteGarbage(maxDataId);
            }
        }

        public void GetAllPositionId(out int[] ids)
        {
            lock (this)
            {
                m_Adapter.GetAllPositionId(out ids);
            }
        }

        public ulong GetPositionFlag()
        {
            int[] ids;
            GetAllPositionId(out ids);

            ulong flag = 0;
            foreach (int id in ids)
            {
                flag |= ((ulong)0x01 << id);
            }

            return flag;
        }
        #endregion
    }
}
