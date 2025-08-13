using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using Dms.Data;
using System.Windows.Forms;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Sequence
{
    public class ThreadGantryControl : XSequence
    {
        #region Fields
        protected static IServerManager m_Server;
        protected static _GenericCollection<GantryUnit> m_GantryUnits;
        protected static int m_UnitCount;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public ThreadGantryControl(int scanTime, IServerManager server)
            : base(scanTime)
        {
            m_Server = server;
            m_GantryUnits = DmsComponents.Instance.ComponentContainer.GetCollection<GantryUnit>();
            m_UnitCount = m_GantryUnits.Count;

            RegisterSequences();
        }
        #endregion

        #region RegisterSequences
        protected override void RegisterSequences()
        {
            //foreach (GantryUnit device in m_TrUnits)
            //{
            //    RegisterSequence(new SeqCvMotor(this, device));
            //    RegisterSequence(new SeqCvMotorCondition(this, device));
            //    RegisterSequence(new SeqCvMotorSpeedControl(this, device));
            //}

            //m_Server.AddSeqInitFunction(new SeqInitTr(this, m_Server));
        }
        #endregion

        #region Sequence
        public override void Sequence()
        {
            try
            {
                Thread.Sleep(m_ScanTime);

                if (m_Server.State != ActiveState.Run) return;
                if (!m_Server.ControllerIsRun) return;

                foreach (XSeqFunction seq in m_SeqFunctions)
                {
                    seq.Do();
                }
            }
            catch (Exception err)
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion

        #region Static Methods
        public static _GenericCollection<GantryUnit> Units
        {
            get
            {
                if (m_GantryUnits == null) m_GantryUnits = new _GenericCollection<GantryUnit>();
                return m_GantryUnits;
            }
        }
        #endregion   

        #region Virtual Methods
        public virtual void InitParameter()
        {
            //foreach (CvUnit unit in m_CvUnits)
            //{
            //    if (unit.Sequence[0] != null) unit.Sequence[0].InitSeq();
            //    if (unit.Sequence[1] != null) unit.Sequence[1].InitSeq();
            //    unit.IfFlag.Reset();
            //    unit.AutoAct = CvMotorAct.Stop;

            //    int motorCount = unit.Motors.Count;
            //    for (int i = 0; i < motorCount; i++)
            //    {
            //        unit.ManualAct[i] = CvMotorAct.Stop;
            //        unit.ManualSpeed[i] = 0;
            //    }
            //}

            BaseGlobalVar.GlassOutComp1 = true;// m_Server.SeqFlag.GlassOutComp1 = true;
            BaseGlobalVar.UlTimeOut = false;// m_Server.SeqFlag.UlTimeOut = false;
            BaseGlobalVar.LdTimeOut = false;// m_Server.SeqFlag.LdTimeOut = false;
        }
        #endregion

        #region General Methods

        #endregion
    }

    public class SeqMoveGantryRobotPosition : XSeqFunction
    {
        public override int Do()
        {
            return base.Do();
        }
    }
}
