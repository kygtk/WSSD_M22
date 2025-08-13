using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Reflection;

namespace Dms.Device
{
    public class TransferUnits : _GenericCollection<TransferUnit>
    {
        private static CvUnits m_CvUnits = new CvUnits();

        #region Constructor
        public TransferUnits()
        {
        }
        #endregion

        #region Properties
        public static CvUnits CvUnits
        {
            get { return m_CvUnits; }
        }
        #endregion

        #region Methods

        #endregion

        #region Override
        protected override bool Initialize()
        {
            int count = m_Items.Count;
            for (int i = 0; i < count; i++)
            {
                this[i].GenerateDataKey();

                if (i != 0)
                {
                    this[i].PrevUnit = this[i - 1];
                }
                if (i < m_Items.Count - 1)
                {
                    this[i].NextUnit = this[i + 1];
                }
            }

            if (m_CvUnits.Count == 0)
            {
                for (int i = 0; i < count; i++)
                {
                    CvUnit cvUnit = m_Items[i] as CvUnit;
                    if (cvUnit != null)
                    {
                        m_CvUnits.Add(cvUnit);
                    }
                }

                m_CvUnits.Initialize(m_Server, m_GenInfo);
            }

            return base.Initialize();
        }
        #endregion
    }
}
