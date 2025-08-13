using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Server;
using Dms.Device;
using System.Threading;
using Dms.Data;

namespace Dms.Simulator
{
    public class ThreadEqpSimulator : XSequence
    {
		#region Fields
		protected static ServerManager m_Server;
		public IfSignalFromCim ToEqp;
		public IfSignalToCim FromEqp;

		//protected static Queue<GlassInfo> m_GlassQueue = new Queue<GlassInfo>();
		//protected static Queue<uint> m_GlassQueueTimer = new Queue<uint>();
		#endregion

		#region Property
		#endregion

		#region RegisterSequences
		protected override void RegisterSequences()
		{
			RegisterSequence(new SeqSimRecipBodyRequest(this));
			RegisterSequence(new SeqSimLostDataRequest(this));
			RegisterSequence(new SeqSimGlassDataSend(this));
			RegisterSequence(new SeqSimUnloadGlassDataRequest(this));
			RegisterSequence(new SeqSimRecipeVariousReport(this));
			RegisterSequence(new SeqSimRecipeVariousDataRequest(this));
		}
		#endregion

		#region Constructor
		public ThreadEqpSimulator()
		{
		    m_Server = ServerManager.Instance;

			ToEqp =  eqpIfSignalFromCims._ECS_Signal;
			FromEqp = eqpIfSignalToCims._EQP_Signal;

		    RegisterSequences();
		}
		#endregion

		#region Sequence
		public override void Sequence()
		{
			try
			{
				Thread.Sleep(50);

				foreach (XSeqFunction seq in SeqFunctions)
				{
					seq.Do();
				}
			}
			catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
			{
				XFunc.ExceptionHandler.Add(err);
			}
		}
		#endregion
    }

	public class SeqSimRecipBodyRequest : XSeqFunction
	{
		#region Fields
		private ThreadEqpSimulator m_Control;
		#endregion

		#region Constructor
		public SeqSimRecipBodyRequest(ThreadEqpSimulator eqpsimulator)
		{
			m_Control = eqpsimulator;
		}
		#endregion

		#region Sequence
		public override int Do()
		{
			int seqNo = this.SeqNo;

			bool request = m_Control.ToEqp.DiRecipeBodyDataRequest.GetState();
			short recipeNumber = m_Control.ToEqp.AiRecipeBodyRequestRecipeNumber.GetState();

			switch (seqNo)
			{
				case 0:
					if(request)
					{
						m_Control.ToEqp.AiRecipeBodyRequestRecipeNumber.SetState(recipeNumber);
						XSimLog.WriteLog(string.Format("ECS : AiRecipeBodyRequestRecipeNumber set - {0}", recipeNumber));
						seqNo = 10;
					}
					break;
				case 10:
					{
						m_Control.ToEqp.DiRecipeBodyDataRequest.SetState(true);
						XSimLog.WriteLog("ECS : DiRecipeBodyDataRequest - ON");
						seqNo = 20;
					}
					break;
				case 20:
					if(m_Control.FromEqp.DoRecipeBodyDataReport.GetState())
					{
						XSimLog.WriteLog("EQP : DoRecipeBodyDataReport - ON");
						seqNo = 30;
					}
					break;
				case 30:
					{
						m_Control.ToEqp.DiRecipeBodyDataRequest.SetState(false);
						XSimLog.WriteLog("ECS : DiRecipeBodyDataRequest - OFF");
						seqNo = 40;
					}
					break;
				case 40:
					if (!m_Control.FromEqp.DoRecipeBodyDataReport.GetState())
					{
						XSimLog.WriteLog("EQP : DoRecipeBodyDataReport - OFF");
						seqNo = 0;
					}
					break;
			}

			this.SeqNo = seqNo;

			return -1;
		}
		#endregion

		#region Methods
		#endregion
	}

	public class SeqSimLostDataRequest : XSeqFunction
	{
		#region Fields
		private ThreadEqpSimulator m_Control;
		#endregion

		#region Constructor
		public SeqSimLostDataRequest(ThreadEqpSimulator eqpsimulator)
		{
			m_Control = eqpsimulator;
		}
		#endregion

		#region Sequence
		public override int Do()
		{
			int seqNo = this.SeqNo;

			switch (seqNo)
			{
				case 0:
					if (m_Control.FromEqp.DoGlassDataRequest.GetState())
					{
						XSimLog.WriteLog(string.Format("EQP : DoGlassDataRequest - ON"));

						//SetData
						TagGlassData glassData = new TagGlassData();
						glassData.LotID = "LOST_GLS_LOT";
						glassData.GlassID = "LOST_GLS_ID";
						glassData.GlassCode.LotNo = 2;
						glassData.GlassCode.SlotNo = 3;
						glassData.RecipeID = "1";
						short[] stream = glassData.ConvertToStreamShort();
						m_Control.ToEqp.AiLostGlassDatas.SetAiStates(stream);
						XSimLog.WriteLog(string.Format("ECS : AiLostGlassDatas set"));

						m_Control.ToEqp.DiLostGlassRequestAckOk.SetState(true);
						XSimLog.WriteLog(string.Format("ECS : DiLostGlassRequestAckOk - ON"));

						m_Control.ToEqp.DiLostGlassRequestAckNg.SetState(false);
						XSimLog.WriteLog(string.Format("ECS : DiLostGlassRequestAckNg - OFF"));

						seqNo = 10;
					}
					break;
				case 10:
					{
						m_Control.ToEqp.DiLostGlassDataReport.SetState(true);
						XSimLog.WriteLog("ECS : DiLostGlassDataReport - ON");
						seqNo = 20;
					}
					break;
				case 20:
					if (!m_Control.FromEqp.DoGlassDataRequest.GetState())
					{
						XSimLog.WriteLog("EQP : DoRecipeBodyDataReport - OFF");
						seqNo = 30;
					}
					break;
				case 30:
					{
						m_Control.ToEqp.DiLostGlassRequestAckOk.SetState(false);
						XSimLog.WriteLog(string.Format("ECS : DiLostGlassRequestAckOk - OFF"));

						m_Control.ToEqp.DiLostGlassRequestAckNg.SetState(false);
						XSimLog.WriteLog(string.Format("ECS : DiLostGlassRequestAckNg - OFF"));

						m_Control.ToEqp.DiLostGlassDataReport.SetState(false);
						XSimLog.WriteLog("ECS : DiLostGlassDataReport - OFF");

						seqNo = 0;
					}
					break;
			}

			this.SeqNo = seqNo;

			return -1;
		}
		#endregion

		#region Methods
		#endregion
	}

	public class SeqSimRecipeVariousReport : XSeqFunction
	{
		#region Fields
		private ThreadEqpSimulator m_Control;
		#endregion

		#region Constructor
		public SeqSimRecipeVariousReport(ThreadEqpSimulator eqpsimulator)
		{
			m_Control = eqpsimulator;
		}
		#endregion

		#region Sequence
		public override int Do()
		{
			int seqNo = this.SeqNo;

			switch (seqNo)
			{
				case 0:
					if (m_Control.FromEqp.DoRecipeVariousDataReport.GetState())
					{
						XSimLog.WriteLog(string.Format("EQP : DoRecipeVariousDataReport - ON"));

						m_Control.ToEqp.AiRecipeVariousReportConfirmCeid.SetState((short)RecipeVariousCEID.RecipeChangeReport);
						m_Control.ToEqp.AiRecipeVariousReportConfirmAck.SetState((short)RecipeReportConfirmAck.Accepted);

						XSimLog.WriteLog(string.Format("ECS : AiRecipeVariousReportConfirmAck set"));
						seqNo = 10;
					}
					break;
				case 10:
					{
						m_Control.ToEqp.DiRecipeVariousDataConfirm.SetState(true);
						XSimLog.WriteLog("ECS : DiRecipeVariousDataConfirm - ON");
						seqNo = 20;
					}
					break;
				case 20:
					if (!m_Control.FromEqp.DoRecipeVariousDataReport.GetState())
					{
						XSimLog.WriteLog("EQP : DoRecipeVariousDataReport - OFF");
						seqNo = 30;
					}
					break;
				case 30:
					{
						m_Control.ToEqp.DiRecipeVariousDataConfirm.SetState(false);
						XSimLog.WriteLog("ECS : DiRecipeVariousDataConfirm - OFF");
						seqNo = 0;
					}
					break;
			}

			this.SeqNo = seqNo;

			return -1;
		}
		#endregion

		#region Methods
		#endregion
	}

	public class SeqSimGlassDataSend : XSeqFunction
	{
		#region Fields
		private ThreadEqpSimulator m_Control;
		#endregion

		#region Constructor
		public SeqSimGlassDataSend(ThreadEqpSimulator eqpsimulator)
		{
			m_Control = eqpsimulator;
		}
		#endregion

		#region Sequence
		public override int Do()
		{
			int seqNo = this.SeqNo;

			switch (seqNo)
			{
				case 0:
					if (m_Control.FromEqp.DoLoadGlassDataRequest.GetState())
					{
						XSimLog.WriteLog(string.Format("EQP : DoLoadGlassDataRequest - ON"));

						//SetData
						TagGlassData glassData = new TagGlassData();
						glassData.LotID = "LD_GLS_LOT";
						glassData.GlassID = "LD_GLS_ID";
						glassData.GlassCode.LotNo = 2;
						glassData.GlassCode.SlotNo = 3;
						glassData.RecipeID = "1";
						short[] stream = glassData.ConvertToStreamShort();
						m_Control.ToEqp.AiGlassDatas.SetAiStates(stream);
						XSimLog.WriteLog(string.Format("ECS : AiGlassDatas set"));

						short existFlag = 0;
						m_Control.ToEqp.AiGlassDataSendFlag.SetState(existFlag);
						XSimLog.WriteLog(string.Format("ECS : AiGlassDataSendFlag - {0}", existFlag.ToString()));

						seqNo = 10;
					}
					break;
				case 10:
					{
						m_Control.ToEqp.DiGlassDataSend.SetState(true);
						XSimLog.WriteLog("ECS : DiGlassDataSend - ON");
						seqNo = 20;
					}
					break;
				case 20:
					if (!m_Control.FromEqp.DoLoadGlassDataRequest.GetState())
					{
						XSimLog.WriteLog("EQP : DoLoadGlassDataRequest - OFF");
						seqNo = 30;
					}
					break;
				case 30:
					{
						m_Control.ToEqp.DiGlassDataSend.SetState(false);
						XSimLog.WriteLog("ECS : DiGlassDataSend - OFF");

						seqNo = 0;
					}
					break;
			}

			this.SeqNo = seqNo;

			return -1;
		}
		#endregion

		#region Methods
		#endregion
	}

	public class SeqSimUnloadGlassDataRequest : XSeqFunction
	{
		#region Fields
		private ThreadEqpSimulator m_Control;
		#endregion

		#region Constructor
		public SeqSimUnloadGlassDataRequest(ThreadEqpSimulator eqpsimulator)
		{
			m_Control = eqpsimulator;
		}
		#endregion

		#region Sequence
		public override int Do()
		{
			int seqNo = this.SeqNo;

			bool request = m_Control.ToEqp.DiUnloadGlassDataRequest.GetState();

			switch (seqNo)
			{
				case 0:
					if (request)
					{
						m_Control.ToEqp.DiUnloadGlassDataRequest.SetState(true);
						XSimLog.WriteLog(string.Format("ECS : DiUnloadGlassDataRequest - ON"));
						seqNo = 10;
					}
					break;
				case 10:
					if (m_Control.FromEqp.DoUnloadDataRequestConfirm.GetState())
					{
						XSimLog.WriteLog("EQP : DoUnloadDataRequestConfirm - ON");
						
						m_Control.ToEqp.DiUnloadGlassDataRequest.SetState(false);
						XSimLog.WriteLog(string.Format("ECS : DiUnloadGlassDataRequest - OFF"));
						seqNo = 20;
					}
					break;
				case 20:
					if (!m_Control.FromEqp.DoUnloadDataRequestConfirm.GetState())
					{
						XSimLog.WriteLog("EQP : DoUnloadDataRequestConfirm - OFF");
						seqNo = 0;
					}
					break;
			}

			this.SeqNo = seqNo;

			return -1;
		}
		#endregion

		#region Methods
		#endregion
	}

	public class SeqSimRecipeVariousDataRequest : XSeqFunction
	{
		#region Fields
		private ThreadEqpSimulator m_Control;
		#endregion

		#region Constructor
		public SeqSimRecipeVariousDataRequest(ThreadEqpSimulator eqpsimulator)
		{
			m_Control = eqpsimulator;
		}
		#endregion

		#region Sequence
		public override int Do()
		{
			int seqNo = this.SeqNo;

			bool request = m_Control.ToEqp.DiRecipeVariousDataRequest.GetState();
			
			switch (seqNo)
			{
				case 0:
					if (request)
					{
						short ceid = (short)RecipeVariousCEID.ReciepList;
						short recipeLevel = (short)RecipeLevel.EqpRecipe;
						short recipeType = (short)RecipeType.Machine;
						short recipeNumber = 1;
						short[] version = new short[3];
						short unitNumber = 0;

						short[] specificData = new short[m_Control.ToEqp.AiEqpSpecificDatas.Count];
						short[] parameter = new short[m_Control.ToEqp.AiRecipeParameters.Count];
						
						m_Control.ToEqp.AiRequestCeid.SetState(ceid);
						m_Control.ToEqp.AiRecipeVariousRequestRecipeLevel.SetState(recipeLevel);
						m_Control.ToEqp.AiRecipeVariousRequestRecipeType.SetState(recipeType);
						m_Control.ToEqp.AiRecipeVariousRequestRecipeNumber.SetState(recipeNumber);
						m_Control.ToEqp.AiRecipeVariousRequestRecipeVersions.SetAiStates(version);
						m_Control.ToEqp.AiRequestUnitNumber.SetState(unitNumber);
						m_Control.ToEqp.AiEqpSpecificDatas.SetAiStates(specificData);
						m_Control.ToEqp.AiRecipeParameters.SetAiStates(specificData);

						XSimLog.WriteLog(string.Format("ECS : RecipeVariousRequestData Set"));

						m_Control.ToEqp.DiRecipeVariousDataRequest.SetState(true);
						XSimLog.WriteLog(string.Format("ECS : DiRecipeVariousDataRequest ON"));

						seqNo = 10;
					}
					break;
				case 10:
					if(m_Control.FromEqp.DoRecipeVariousRequestConfirm.GetState())
					{
						XSimLog.WriteLog("EQP : DoRecipeVariousRequestConfirm - ON");

						m_Control.ToEqp.DiRecipeVariousDataRequest.SetState(false);
						XSimLog.WriteLog("ECS : DiRecipeVariousDataRequest - OFF");
						seqNo = 20;
					}
					break;
				case 20:
					if (!m_Control.FromEqp.DoRecipeVariousRequestConfirm.GetState())
					{
						XSimLog.WriteLog("EQP : DoRecipeVariousRequestConfirm - OFF");
						seqNo = 0;
					}
					break;
			}

			this.SeqNo = seqNo;

			return -1;
		}
		#endregion

		#region Methods
		#endregion
	}
}
