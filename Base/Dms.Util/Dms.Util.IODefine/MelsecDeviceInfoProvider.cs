///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.05
// Author       : jemoon
// Description  : MelsecDeviceInfoProvider
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.IO;
using System.Windows.Forms;

namespace Dms.Util.IODefine
{
    public class MelsecDeviceInfoProvider
    {
        #region Fields
        private MelsecDeviceInfos m_DeviceInfos;    // parsing 한 data를 전부 add
        private MelsecDeviceInfos m_DiAddressMap;   // IoType별로 sort하여 add
        private MelsecDeviceInfos m_DoAddressMap;   // IoType별로 sort하여 add
        private MelsecDeviceInfos m_AiAddressMap;   // IoType별로 sort하여 add
        private MelsecDeviceInfos m_AoAddressMap;   // IoType별로 sort하여 add

        private static AppConfig m_AppConfig = AppConfig.Instance;
        private static int m_CheckPath = -1;
        private static string m_FolderPath = "";

        #endregion

        #region Properties
        public MelsecDeviceInfos DeviceInfos
        {
            get { return m_DeviceInfos; }
            set { m_DeviceInfos = value; }
        }
        public MelsecDeviceInfos DiAddressMap
        {
            get { return m_DiAddressMap; }
            set { m_DiAddressMap = value; }
        }
        public MelsecDeviceInfos DoAddressMap
        {
            get { return m_DoAddressMap; }
            set { m_DoAddressMap = value; }
        }
        public MelsecDeviceInfos AiAddressMap
        {
            get { return m_AiAddressMap; }
            set { m_AiAddressMap = value; }
        }
        public MelsecDeviceInfos AoAddressMap
        {
            get { return m_AoAddressMap; }
            set { m_AoAddressMap = value; }
        }
        #endregion

        #region Constructor
        public MelsecDeviceInfoProvider()
        {
        }
        #endregion

        #region Methods
        private void CheckFilePath(string path)
        {
            string folderPath = path;

            if (string.IsNullOrEmpty(folderPath))
            {
                m_AppConfig.ReadXml();
                folderPath = m_AppConfig.MelsecDeviceInfoPathName;
            }

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(folderPath);
            }

            if (Directory.Exists(folderPath) == false)
            {
                MessageBox.Show("MelsecDeviceInfo Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = Application.StartupPath;
                dlg.Description = "MelsecDeviceInfo Folder";
                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    folderPath = dlg.SelectedPath;
                    m_AppConfig.MelsecDeviceInfoPath.SelectedFolder = folderPath;
                    if (m_AppConfig.WriteXml())
                    {
                        m_FolderPath = folderPath;
                        m_CheckPath = 1;
                    }
                    else
                    {
                        m_CheckPath = 0;
                    }
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FolderPath = folderPath;
                m_CheckPath = 1;
            }
        }

        public bool ReadFromStorage()
        {
            return ReadFromStorage("");
        }

        public bool ReadFromStorage(string path)
        {
            // 1. 경로 처리
            // 1) default path option
            // 2) 지정경로는 폴더별로
            // 우선 default path 로 처리 합시다.
            // default path : ConfilgFiles/MelsecDeviceInfos/

            // 2.Parsing
            // 1) Read files : DeviceType별로(DevL.txt, DevB.txt...)
            // 2) Add to DataContainer : m_DeviceInfos

            // 3. MakeAddressMap
            // 1) Get item by IoType form DataContainer
            // 2) Add to correspond AddressMap

            if (m_CheckPath == -1) CheckFilePath(path); //1. 경로처리
            if (m_CheckPath == 1)
            {
                bool ok = true;
                ok &= Parsing();        //2. Parsing
                ok &= MakeAddressMap(); //3. MakeAddressMap
                return ok;
            }
            else
            {
                return false;
            }
        }

        public void GetIoItems(IoDefines ioDefines)
        {
            ioDefines.ClearCollection();

            foreach (MelsecDeviceInfo dev in m_DiAddressMap.Items)
            {
                IoItemDI io = new IoItemDI();
                io.IoType = IoType.DI;
                io.Id = dev.ChannelId;
                io.Name = dev.Name;
                ioDefines.AddIOCollection(io);
            }

            foreach (MelsecDeviceInfo dev in m_DoAddressMap.Items)
            {
                IoItem io = new IoItem();
                io.IoType = IoType.DO;
                io.Id = dev.ChannelId;
                io.Name = dev.Name;
                ioDefines.AddIOCollection(io);
            }

            foreach (MelsecDeviceInfo dev in m_AiAddressMap.Items)
            {
                IoItem io = new IoItem();
                io.IoType = IoType.AI;
                io.Id = dev.ChannelId;
                io.Name = dev.Name;
                ioDefines.AddIOCollection(io);
            }

            foreach (MelsecDeviceInfo dev in m_AoAddressMap.Items)
            {
                IoItem io = new IoItem();
                io.IoType = IoType.AO;
                io.Id = dev.ChannelId;
                io.Name = dev.Name;
                ioDefines.AddIOCollection(io);
            }
        }

        // order : InoutType / devType / Address / Name
        private enum ParserItems { Inout = 0, DevType, Address, Name, Count };
        private bool Parsing()
        {
            m_DeviceInfos = new MelsecDeviceInfos();

            try
            {
                string[] files = Directory.GetFiles(m_FolderPath);
                string[] lineValues;
                int fileCount = files.Length;

                if (fileCount == 0) return true;

                string line = "";
                FileStream fileStream = null;
                StreamReader streamReader = null;
                MelsecDeviceInfo info = null;

                for (int i = 0; i < fileCount; i++)
                {
                    if (File.Exists(files[i]))
                    {
                        fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        streamReader = new StreamReader(fileStream, Encoding.Default);

                        while ((line = streamReader.ReadLine()) != null)
                        {
                            lineValues = line.Split('\t');

                            if (lineValues.Length < (int)ParserItems.Count) continue;
                            if (!Enum.IsDefined(typeof(IoInOutType), lineValues[(int)ParserItems.Inout])) continue;
                            if (!Enum.IsDefined(typeof(devTYPE), lineValues[(int)ParserItems.DevType])) continue;
                            if (string.IsNullOrEmpty(lineValues[(int)ParserItems.Address])) continue;
                            if (string.IsNullOrEmpty(lineValues[(int)ParserItems.Name])) continue;

                            info = new MelsecDeviceInfo();
                            info.InOutType = (IoInOutType)Enum.Parse(typeof(IoInOutType), lineValues[(int)ParserItems.Inout]);
                            info.DeviceType = (devTYPE)Enum.Parse(typeof(devTYPE), lineValues[(int)ParserItems.DevType]);
                            info.AddressStr = lineValues[(int)ParserItems.Address];
                            info.Name = lineValues[(int)ParserItems.Name];

                            m_DeviceInfos.Items.Add(info);
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool MakeAddressMap()
        {
            m_DiAddressMap = new MelsecDeviceInfos();
            m_DoAddressMap = new MelsecDeviceInfos();
            m_AiAddressMap = new MelsecDeviceInfos();
            m_AoAddressMap = new MelsecDeviceInfos();

            try
            {
                m_DiAddressMap = MelsecDeviceInfos.GetItemsBy(m_DeviceInfos, IoType.DI);
                m_DiAddressMap.UpdateChannelId();
                m_DoAddressMap = MelsecDeviceInfos.GetItemsBy(m_DeviceInfos, IoType.DO);
                m_DoAddressMap.UpdateChannelId();
                m_AiAddressMap = MelsecDeviceInfos.GetItemsBy(m_DeviceInfos, IoType.AI);
                m_AiAddressMap.UpdateChannelId();
                m_AoAddressMap = MelsecDeviceInfos.GetItemsBy(m_DeviceInfos, IoType.AO);
                m_AoAddressMap.UpdateChannelId();
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}

