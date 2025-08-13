using System;
using Dms.Common;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dms.Ctl
{
    public class ReceivDataEventArgs : EventArgs
    {
        private string m_ReceivData;

        public string ReceivData
        {
            get { return m_ReceivData; }
        }
        public ReceivDataEventArgs(object receivedData)
        {
            m_ReceivData = (string)receivedData;
        }
    }

    public class XComm
    {
        private bool m_Simulate = true;
        private bool m_Initialized;
        private XLog m_XCommcLog = new XLog("XCommLog", XLog.LogStampType.UseStamp);

        public bool Simulate
        {
            get { return m_Simulate; }
            set { m_Simulate = value; }
        }
        public bool Initialized
        {
            get { return m_Initialized; }
            set { m_Initialized = value; }
        }

        public delegate void ReceivedDataEventHandler(object sender);

		private System.IO.Ports.SerialPort m_SerialPort;

		[Browsable(false)]
		public System.IO.Ports.SerialPort SerialPort
		{
			get { return m_SerialPort; }
		}

        public event ReceivedDataEventHandler ReceivedData;

        public XComm()
        {

        }

        public XComm(string portName)
        {

        }

        //public XComm(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        //{
        //    PortName = portName;
        //    BaudRate = baudRate;
        //    Parity = parity;
        //    DataBits = dataBits;
        //    StopBits = stopBits;

        //    //Initialize();
        //    //Open(portName, baudRate, parity, dataBits, stopBits);
        //}

        public void Initialize()
        {
            m_SerialPort = new SerialPort();
            
            m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);
            m_SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialErrorReceived);
        }
        
        public void Open(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            if (m_Simulate)
            {
                MessageBox.Show("System run in Serial Port Simulation mode!!");
                return;
            }

            if (m_SerialPort != null)
            {
                m_SerialPort.PortName = portName;
                m_SerialPort.BaudRate = baudRate;
                m_SerialPort.Parity = parity;
                m_SerialPort.DataBits = dataBits;
                m_SerialPort.StopBits = stopBits;
                m_SerialPort.Open();
            }
        }

        public bool IsOpen()
        {
            return m_SerialPort.IsOpen;
        }

        public bool IsInQueue()
        {
            if (m_SerialPort.BytesToRead > 0) return true;
            else return false;
        }

        public void Close()
        {
            m_SerialPort.Close();
        }

        public void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                FireReceivedDataEvent(this);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                m_XCommcLog.TextOut(err.ToString());
                //MessageBox.Show(err.ToString());
            }
        }

        public void SerialErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
           // MessageBox.Show(e.ToString());
            m_XCommcLog.TextOut(e.ToString());
        }

        public int Read(ref char[] data, int length)
        {
            if (m_Simulate)
            {
                
            }

            try
            {
                length = m_SerialPort.Read(data, 0, length);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }

            return length;
        }

        public string ReadLine()
        {
            string data = null;

            if (m_Simulate)
            {

            }

            try
            {
				data = m_SerialPort.ReadLine();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                m_XCommcLog.TextOut(err.ToString());
                //MessageBox.Show(err.ToString());
            }

            return data;
        }

        public string ReadTo(string value)
        {
            string data = null;

            if (m_Simulate)
            {

            }

            try
            {
                data = m_SerialPort.ReadTo(value);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                m_XCommcLog.TextOut(err.ToString());
                //MessageBox.Show(err.ToString());
            }

            return data;
        }



		public string ReadExisting()
		{
			string data = null;

			if (m_Simulate)
			{

			}

			try
			{
				data = m_SerialPort.ReadExisting(); // 09.07.29 minhan
			}
			catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				m_XCommcLog.TextOut(err.ToString());
				//MessageBox.Show(err.ToString());
			}

			return data;
		}

		public int Read(ref byte[] buffer, int offset, int count) // 11.03.07 minhan
		{
			int data = 0;
			if (m_Simulate)
			{

			}

			try
			{
				data = m_SerialPort.Read(buffer, offset, count);
			}
			catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				m_XCommcLog.TextOut(err.ToString());
				//MessageBox.Show(err.ToString());
			}

			return data;
		}

        public bool Write(string msg)
        {
            try
            {
                if (!m_Simulate)
                {
                    m_SerialPort.Write(msg);
                }
            }
            catch// (Exception err)
            {
                return false;
            }
            return true;
        }

        public bool Write(char[] buffer, int offset, int count)
        {
            try
            {
                if (!m_Simulate)
                {
                    m_SerialPort.Write(buffer, offset, count);
                }
            }
            catch// (Exception err)
            {
                return false;
            }
            return true;
        }

		public bool Write(byte[] buffer, int offset, int count)
		{
			try
			{
				if (!m_Simulate)
				{
					m_SerialPort.Write(buffer, offset, count);
				}
			}
			catch// (Exception err)
			{
				return false;
			}
			return true;
		}

        public void FireReceivedDataEvent(object sender)
        {
            if (ReceivedData != null)
            {
                ReceivedData(sender);
            }
        }

        public void SetLog(string sLog)
        {
            m_XCommcLog.TextOut(sLog);
        }
    }
}
