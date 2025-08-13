using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace Dms.Ctl
{
    public static partial class WMX
    {
        public static class EngineCtl
        {
            #region Fields
            private static bool m_Initialized = false;

            private static Thread m_ThreadUpdateEngineState = null;
            private static bool m_Flag_ThreadUpdateEngineState = false;

            private static EngineState m_EngineState = EngineState.Unknown;
            #endregion

            #region Properties
            public static bool Initialized
            {
                get { return m_Initialized; }
            }

            public static EngineState EngineState
            {
                get { return m_EngineState; }
            }
            #endregion

            #region Methods
            internal static void Initialize()
            {
                StartEngine();
                StartComm();

                Thread.Sleep(10);

                InitializeThread();

                m_Initialized = true;
            }

            private static void InitializeThread()
            {
                m_Flag_ThreadUpdateEngineState = true;

                m_ThreadUpdateEngineState = new Thread(() => UpdateEngineState_InSubThread())
                {
                    Name = "UpdateEngineState",
                    IsBackground = true,
                };
                m_ThreadUpdateEngineState.Start();
            }

            private static void UpdateEngineState_InSubThread(int sleeptimems = 10)
            {
                while (m_Flag_ThreadUpdateEngineState)
                {
                    EngineStatus es = new EngineStatus();
                    m_Wmx.GetEngineStatus(ref es);
                    m_EngineState = es.State;

                    Thread.Sleep(sleeptimems);
                }
            }

            internal static void Uninitialize()
            {
                UninitializeThread();

                StopComm();
                //  StopEngine();   일단 엔진은 종료하지 않음

                m_Initialized = false;
            }

            private static void UninitializeThread()
            {
                m_Flag_ThreadUpdateEngineState = false;

                while (m_ThreadUpdateEngineState.IsAlive)
                    Thread.Sleep(1);
            }

            public static int StartEngine()
            {
                int eCode;

                eCode = m_Wmx.StartEngine(c_WMXPath);
                return eCode;
            }

            public static int RestartEngine()
            {
                int eCode;

                eCode = m_Wmx.RestartEngine(c_WMXPath);

                return eCode;
            }

            public static int StopEngine()
            {
                int eCode;

                eCode = m_Wmx.StopEngine();

                return eCode;
            }

            public static int StartComm()
            {
                int eCode;

                eCode = m_Wmx.StartCommunication();

                return eCode;
            }

            public static int StopComm()
            {
                int eCode;

                eCode = m_Wmx.StopCommunication();

                return eCode;
            }
            #endregion
        }
    }
}
