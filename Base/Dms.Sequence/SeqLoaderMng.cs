using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Dms.Common;
using System.Windows.Forms;
using Dms.Device;

namespace Dms.Sequence
{
    public class ThreadLoaderMng : XSequence
    {
        #region Fields
        protected static _GenericCollection<LoaderUnit> m_Loaders = new _GenericCollection<LoaderUnit>();
        protected static int m_UnitCount;
        protected static IServerManager m_Server;
        protected static _GenInfoHandler m_GenInfos;
        #endregion
        
        #region Constructor
        public ThreadLoaderMng(int scanTime, IServerManager server) 
            : base(scanTime)
        {
        }
        #endregion

        #region Override
        protected override void RegisterSequences()
        {
            base.RegisterSequences();
        }

        public override void Sequence()
        {
            base.Sequence();
        }
        #endregion

        #region Methods
        #endregion
    }
}

