using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Device;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.Server
{
    public class GlassDataHandler : IGlassDataHandler
    {
        #region Fields
        private GlassDataProvider m_Provider;
        private ServerManager m_Server;
        private GenInfoHandler m_GenInfo;//2010.06.29 kimgun
        #endregion

        #region Properties
        public int Count
        {
            get { return m_Provider.Count; }
        }
        #endregion

        #region Constructor
        public GlassDataHandler(GlassDataProvider provider)
        {
            m_Provider = provider;
        }
        #endregion

        #region Methods
        public void CreateHandler(ServerManager server)
        {
            m_Server = server;
            m_GenInfo = GenInfoHandler.Instance;
        }

        public void DeleteGarbage()
        {
            //
            // GlassData garbage collecting
            //
            int maxDataId = GlassDataIDHandler.MaxPositionCount - 1;
            m_Provider.DeleteDbGarbage(maxDataId);
            m_GenInfo.EQPGlassCount = m_Provider.Count;
        }

        public bool GetData(int position, ref TagGlassData data)
        {
            return m_Provider.GetData(position, ref data);
        }

        public void GetPortNo(int Position, ref int PortNo, ref int SlotNo)
        {
            TagGlassData Data = new TagGlassData();
            m_Provider.GetData(Position, ref Data);

            PortNo = Data.Item.GlassNumberCode.LotNo;
            SlotNo = Data.Item.GlassNumberCode.SlotNo;
        }

        public bool IsExist(int position)
        {
            return m_Provider.IsExist(position);
        }

        public bool IsProcessed(int Position)
        {
            TagGlassData Data = new TagGlassData();
            m_Provider.GetData(Position, ref Data);

            return Data.Processed;
        }

        public void Create(TagGlassData data)
        {
            if (true == m_Provider.Create(data))
            {
                //m_Server.GenInfos.EQPGlassCount++;
                //m_Server.GenInfos.TotalGlassCount++;
                //m_Server.GenInfos.CurGlassCount++;
                m_GenInfo.EQPGlassCount++;
                m_GenInfo.TotalGlassCount++;
                m_GenInfo.CurGlassCount++;
                m_GenInfo.TodayGlassCount++;//2010.06.29 kimgun




                //string msg = string.Format("GlassData Created : " + data.Item.GlassId);
                //m_Server.FireEvent(data, msg);

                m_Server.ApdItemsHandler.AddNewItem(data.PositionId);
            }
        }

        protected void Delete(TagGlassData data)
        {
            if (true == m_Provider.Delete(data.PositionId))
            {
                m_GenInfo.EQPGlassCount--;

                //string msg = string.Format("GlassData Created : " + data.Item.GlassId);
                //m_Server.FireEvent(data, msg);

                m_Server.ApdItemsHandler.Delete(data.PositionId);
            }
        }

        public void Delete(int positionId)
        {
            TagGlassData data = new TagGlassData();
            if (true == m_Provider.GetData(positionId, ref data))
            {
                Delete(data);
                m_Server.ApdItemsHandler.Delete(positionId);
            }
        }

        public void Move(int fromPositionId, int toPositionId)
        {
            if (true == m_Provider.Move(fromPositionId, toPositionId))
            {
                //TagGlassData data = new TagGlassData();
                //string msg = string.Format("GlassData Moved : {0} -> {1}", fromPositionId, toPositionId);
                //m_Server.FireEvent(data, msg);
                m_Server.ApdItemsHandler.Move(fromPositionId, toPositionId);
            }
        }

        public void Update(int position, TagGlassData data)
        {
            if (true == m_Provider.Update(position, data))
            {
                //string msg = string.Format("GlassData Updated : {0}", data.Item.GlassId);
                //m_Server.FireEvent(data, msg);           
            }
        }

        public void GetAllPositionId(out int[] ids)
        {
            m_Provider.GetAllPositionId(out ids);
        }

        public ulong GetPositionFlag()
        {
            //int[] ids;
            //m_Provider.GetAllPositionId(out ids);

            //ulong flag = 0;
            //foreach (int id in ids)
            //{
            //    flag |= ((ulong)0x01 << id);       
            //}

            //return flag;
            return m_Provider.GetPositionFlag();
        }

        #endregion
    }
}
