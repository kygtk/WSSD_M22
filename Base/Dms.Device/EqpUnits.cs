///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.15
// Author       : jemoon
// Description  : Collection of EqpUnit
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.18 - jemoon : code review - GenericCollecion 상속
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Device
{
    [Editor(typeof(UIEditorPropertyEdit), typeof(UITypeEditor))]
    public class EqpUnits : _GenericCollection<EqpUnit>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public EqpUnits()
        {
        }
        #endregion

        #region Methods
        protected void Initialize(EqpUnit parent, EqpUnits cfgEqpUnits)
        {
            foreach (EqpUnit cfg in cfgEqpUnits)
            {
                // 설정파일에서 하나 꺼내서 첫번째 SubUnit을 세팅한다.
                cfg.Parent = parent;
                cfg.Initialize(m_Server, m_GenInfo);

                // 만약 SubUnit에 또 하부 Unit이 있다면 이후, 재귀호출로~
                Initialize(cfg, cfg.EqpUnits);

                // 마지막 Unit이 세팅완료 되어야 여기로 진입, 부모Unit에 할당한다.
                //parent.EqpUnits.Add(unit);
            }
        }
        #endregion

        #region Override
        public override bool Initialize(IServerManager server, _GenInfoHandler geninfos)
        {
            m_Server = server;
            m_Simul = AppConfig.Instance.Simul;

            foreach (EqpUnit cfg in m_Items)
            {
                // 우선 Main Eqp를 세팅하고
                cfg.Initialize(m_Server, geninfos);

                // Main Eqp에 속한 Unit을 세팅한다.
                this.Initialize(cfg, cfg.EqpUnits);
            }

            return base.Initialize(server, geninfos);
        }
        #endregion
    }
}
