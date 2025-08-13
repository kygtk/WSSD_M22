using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;

namespace Dms.Device
{
    abstract public class _JobCondition : IJobCondition
    {
        #region Fields
        protected IComponentContainer m_ComponentContainer;
        #endregion

        #region IJobCondition 멤버

        public virtual bool GetGaugeCond(CvUnit cv)
        {
            _GenericCollection<Gauge> m_Gauges = m_ComponentContainer.GetCollection<Gauge>();
            bool Ng = false;

            foreach (Gauge gauge in m_Gauges.Items)
            {
                if (gauge.OwnerUnit != null)
                {
                    if (gauge.OwnerUnit.Id == cv.Id)
                    {
                        Ng |= gauge.IsAlarm;
                    }
                }
            }

            return !Ng;
        }

        public virtual HeavyInterlock HeavyInterlock
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual TagInterlock Interlock
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public virtual bool ProcessMode
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual bool SetupIdleUse
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual int SetupIdleRunTime
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual int SetupIdleStopTime
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual TagRecipe CurrentRecipe
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        //public virtual TagStepRecipe CurrentStepRecipe
        //{
        //    get
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }
        //    set
        //    {
        //        throw new Exception("The method or operation is not implemented.");
        //    }
        //}

        public virtual int CvProcessSpeed(int id)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool RbDirection(RbUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double RbProcessSpeed(RbUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double RbGap(RbUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool RbUse(RbUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool DBDirection(DBUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double DBProcessSpeed(DBUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool DBUse(DBUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double DBGap(DBUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void SetRecipe2JobCond(string recipeId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //public virtual void SetStepRecipe(int stepNo)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public virtual void SetTactTime(TactTimeAct act)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int TactTime
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual void SetRecipeReceiveFromHost()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool EuvLampUse(EuvLamp unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool ApUse(_Plasma unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double ApVoltage(_Plasma unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double ApN2Flow(_Plasma unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double ApCDAFlow(_Plasma unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual bool EuvUse(Dms.Device._EuvUnit unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int SetupHpmjRunTime//lkl 150929
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual int SetupHpmjStopTime//lkl 150929
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        public virtual bool HpmjUse(Dms.Device.Hpmj unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int HpmjPressure(Dms.Device.Hpmj unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //장비 모든 Unit의 Glass count;
        //수정필요시 override 할것
        private static _GenericCollection<TransferUnit> _transferUnits = null;
        public virtual int GetGlassCountInEqp()
        {
            if (_transferUnits == null)
            {
                _transferUnits = m_ComponentContainer.GetCollection<TransferUnit>();
            }

            int glassCount = 0;
            int unitCount = _transferUnits.Count;
            for (int i = 0; i < unitCount; i++)
            {
                TransferUnit unit = _transferUnits[i];
                glassCount += unit.GetGlassDataExistCountInUnit();
            }

            return glassCount;
        }

        //장비 Unit중 Process Unit의 Glass coount, ex)HDC는 TR, Hand 제외한 LD(in) ~ UL(in)까지만
        //수정필요시 override 할것
        private static _GenericCollection<CvUnit> _processTransferUnits = null;
        public virtual int GetGlassCountInProcess()
        {
            if (_processTransferUnits == null)
            {
                _processTransferUnits = m_ComponentContainer.GetCollection<CvUnit>();
            }

            int glassCount = 0;
            int unitCount = _processTransferUnits.Count;
            for (int i = 0; i < unitCount - 1; i++)
            {
                TransferUnit unit = _processTransferUnits[i];
                glassCount += unit.GetGlassDataExistCountInUnit();
            }

            if (unitCount > 0)
            {
                glassCount += _processTransferUnits[unitCount - 1].IsGlassDataExist(0) ? 1 : 0;
            }

            return glassCount;
        }

        public virtual int GetGlassCountMaxCapability()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int GetGlassCountPutIntoPossible()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual List<int> GetCurrentAlarmIds()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual List<int> GetCurrnetWarningIds()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double BasePressure()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual double ProcessPressure()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
