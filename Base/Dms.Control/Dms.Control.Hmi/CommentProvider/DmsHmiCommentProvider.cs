// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : class for HmiCommentProvider
//-------------------------------------------------------------------------
// Revison History
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Dms.Common;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace Dms.Control.Hmi
{
	public class XmlSerializableColor
	{
		public enum ColorFormat
		{
			NamedColor,
			ARGBColor
		}

		public static string FromColor(Color color)
		{
			if (color.IsNamedColor)
			{
				return string.Format("{0}:{1}", ColorFormat.NamedColor, color.Name);
			}
			else
			{
				return string.Format("{0}:{1}:{2}:{3}:{4}", ColorFormat.ARGBColor, color.A, color.R, color.G, color.B);
			}
		}

		public static Color ToColor(string color)
		{
			byte a, r, g, b;

			string[] para = color.Split(new char[] { ':' });

			ColorFormat colorType = (ColorFormat)Enum.Parse(typeof(ColorFormat), para[0], true);

			switch (colorType)
			{
				case ColorFormat.NamedColor:
						return Color.FromName(para[1]);
				case ColorFormat.ARGBColor:
					    a = byte.Parse(para[1]);
						r = byte.Parse(para[2]);
						g = byte.Parse(para[3]);
						b = byte.Parse(para[4]);

						return Color.FromArgb(a, r, g, b);
			}
			return Color.Empty;
		}
	}

	public class XmlSerializableFont
	{
		private string m_FontName;
		private Single m_FontSize;
		private FontStyle m_FontStyle;
		private GraphicsUnit m_FontUnit;
		private byte m_FontGdiCharSet;

		public string FontName
		{
			get { return m_FontName; }
			set { m_FontName = value; }
		}
		public Single FontSize
		{
			get { return m_FontSize; }
			set { m_FontSize = value; }
		}
		public FontStyle FontStyle
		{
			get { return m_FontStyle; }
			set { m_FontStyle = value; }
		}
		public GraphicsUnit FontUnit
		{
			get { return m_FontUnit; }
			set { m_FontUnit = value; }
		}
		public byte FontGdiCharSet
		{
			get { return m_FontGdiCharSet; }
			set { m_FontGdiCharSet = value; }
		}

		public static XmlSerializableFont FromFont(Font font)
		{
			XmlSerializableFont xmlFont = new XmlSerializableFont();
			xmlFont.FontName = font.Name;
			xmlFont.FontSize = font.Size;
			xmlFont.FontStyle = font.Style;
			xmlFont.FontUnit = font.Unit;
			xmlFont.FontGdiCharSet = font.GdiCharSet;
			return xmlFont;
		}

		public Font ToFont()
		{
			return XmlSerializableFont.ToFont(this);
		}

		public static Font ToFont(XmlSerializableFont xmlFont)
		{
			Font font = new Font(xmlFont.FontName, xmlFont.FontSize, xmlFont.FontStyle, xmlFont.FontUnit, xmlFont.FontGdiCharSet);
			return font;
		}
	}

	[Serializable()]
	public class HmiComment
	{
		#region Fields
		private ushort m_CommentNo = 1;
		private string[] m_Comments;
		private Font m_CommentFont;
		private Color m_CommentColor;
		private Color m_CommentBackColor;
		[XmlIgnore()]
		public static HmiComment DefaultComment = new HmiComment();
		#endregion

		#region Properties
		public ushort CommentNo
		{
			get { return m_CommentNo; }
			set { m_CommentNo = value; }
		}
		public string[] Comments
		{
			get { return m_Comments; }
			set { m_Comments = value; }
		}
		[XmlIgnore()]
		public Font CommentFont
		{
			get { return m_CommentFont; }
			set { m_CommentFont = value; }
		}
		[Browsable(false)]
		[XmlElement("CommentFont")]
		public XmlSerializableFont CommentFontXml
		{
			get { return XmlSerializableFont.FromFont(m_CommentFont); }
			set { m_CommentFont = value.ToFont(); }
		}
		[XmlIgnore()]
		public Color CommentColor
		{
			get { return m_CommentColor; }
			set { m_CommentColor = value; }
		}
		[Browsable(false)]
		[XmlElement("CommentColor")]
		public string CommentColorXml
		{
			get { return XmlSerializableColor.FromColor(m_CommentColor); }
			set { m_CommentColor = XmlSerializableColor.ToColor(value); }
		}
		[XmlIgnore()]
		public Color CommentBackColor
		{
			get { return m_CommentBackColor; }
			set { m_CommentBackColor = value; }
		}
		[Browsable(false)]
		[XmlElement("CommentBackColor")]
		public string CommentBackColorXml
		{
			get { return XmlSerializableColor.FromColor(m_CommentBackColor); }
			set { m_CommentBackColor = XmlSerializableColor.ToColor(value); }
		}
		[Browsable(false)]
		[XmlIgnore()]
		public Type ContainedType
		{
			get { return typeof(string); }
		}
		public string Text
		{
			get 
			{ 
				if (m_Comments != null && m_Comments.Length > (int)DmsHmiComponent.Language)
				{
					string comment = m_Comments[(int)DmsHmiComponent.Language];
					if (string.IsNullOrEmpty(comment))
					{
						comment= "Not Defined";
					}

					return comment; 
				}
				else
				{
					return "Not Defined";
				}
			}
		}
		#endregion

		#region Constructor
		public HmiComment()
		{
			m_Comments = new string[(int)HmiLanguage.Count];
			//m_Comments[(int)HmiLanguage.Default] = "정의되지 않음";
			//m_Comments[(int)HmiLanguage.Korean] = "정의되지 않음";
			//m_Comments[(int)HmiLanguage.English] = "Not Defined";

			for (int i = 0; i < (int)HmiLanguage.Count; i++)
			{
				m_Comments[i] = "Not Defined";
			}

			m_CommentFont = DmsHmiComponent.HmiDefaultFont;
			m_CommentColor = Color.Black;
			m_CommentBackColor = Color.White;
		}
		#endregion

		#region Override
		public override string ToString()
		{
			return string.Format("{0:d} : {1}", m_CommentNo, this.Text);
		}
		#endregion
	}

	public class HmiCommentInfo
	{
		private HmiComment m_Comment;
		private HmiCommentGroupInfo m_GroupInfo;

		public HmiComment Comment
		{
			get { return m_Comment; }
			set { m_Comment = value; }
		}
		public HmiCommentGroupInfo GroupInfo
		{
			get { return m_GroupInfo; }
			set { m_GroupInfo = value; }
		}

		public HmiCommentInfo()
		{
			m_Comment = new HmiComment();
		}

		#region Override
		public override string ToString()
		{
			return string.Format("{0:d} : {1}", m_Comment.CommentNo, m_Comment.Text);
		}
		#endregion
	}

	[Editor(typeof(UIEditorHmiCommentGroupInfoSelect), typeof(UITypeEditor))]
	public class HmiCommentGroupInfo
	{
		private ushort m_GroupNo = 0;
		private string m_GroupName = "";

		public ushort GroupNo
		{
			get { return m_GroupNo; }
			set { m_GroupNo = value; }
		}
		public string GroupName
		{
			get { return m_GroupName; }
			set { m_GroupName = value; }
		}

		public HmiCommentGroupInfo()
		{
		}

		#region Methods
		#endregion


		#region Override
		public override string ToString()
		{
			if (string.IsNullOrEmpty(m_GroupName))
			{
				return "Not defined";
			}
			else
			{
				return string.Format("{0:d} : {1}", m_GroupNo, m_GroupName);
			}
		}
		#endregion
	}

	[Serializable()]
	public class HmiCommentGroup
	{
		#region Fields
		private ushort m_GroupNo = 1;
		private string m_GroupName = "Group";
		private List<HmiComment> m_Items = new List<HmiComment>();
		#endregion

		#region Properties
		public ushort GroupNo
		{
			get { return m_GroupNo; }
			set { m_GroupNo = value; }
		}
		public string GroupName
		{
			get { return m_GroupName; }
			set { m_GroupName = value; }
		}
		public List<HmiComment> Items
		{
			get { return m_Items; }
			set { m_Items = value; }
		}

		public HmiComment this[int index]
		{
			get
			{
				if (index >= 0 && index < m_Items.Count)
				{
					return m_Items[index];
				}
				else
				{
					return HmiComment.DefaultComment;
				}
			}
			set
			{
				if (index >= 0 && index < m_Items.Count)
				{
					m_Items[index] = value;
				}
			}
		}
		#endregion

		#region Constructor
		public HmiCommentGroup() : this(0, "Group0")
		{
		}

		public HmiCommentGroup(ushort groupNo, string groupName)
		{
			m_GroupNo = groupNo;
			m_GroupName = groupName;
		}
		#endregion

		#region Override
		public override string ToString()
		{
			if (string.IsNullOrEmpty(m_GroupName))
			{
				return "Not defined";
			}
			else
			{
				return string.Format("{0:d} : {1}", m_GroupNo, m_GroupName);
			}
		}
		#endregion
	}

	[Serializable()]
	public class HmiCommentProvider
	{
		#region Singleton code...
		public static readonly HmiCommentProvider Instance = new HmiCommentProvider();
		#endregion

		#region Fields
		private List<HmiCommentGroup> m_Items = new List<HmiCommentGroup>();
		
		private static AppConfig m_AppConfig = AppConfig.Instance;
		private static int m_CheckPath = -1;
		private static string m_FileName = "";
		private static bool m_Initialized = false;
		#endregion

		#region Properties
		[Editor(typeof(UIEditorHmiCommentGroup), typeof(UITypeEditor))]
		public List<HmiCommentGroup> CommentGroups
		{
			get { return m_Items; }
			set { m_Items = value; }
		}

		//public HmiCommentGroup this[int index]
		//{
		//    get
		//    {
		//        if (index >= 0 && index < m_Items.Count)
		//        {
		//            return m_Items[index];
		//        }
		//        else
		//        {
		//            return null;
		//        }
		//    }
		//    set
		//    {
		//        if (index >= 0 && index < m_Items.Count)
		//        {
		//            m_Items[index] = value;
		//        }
		//    }
		//}

		public HmiCommentGroup this[string groupName]
		{
			get
			{
				return FindGroup(groupName);
			}
			set
			{
				HmiCommentGroup group = FindGroup(groupName);
				if (group != null)
				{
					group = value;
				}
			}
		}

		[Browsable(false)]
		public string FileName
		{
			get { return m_FileName; }
		}

		[Browsable(false)]
		public Type ContainedType
		{
			get { return  typeof(HmiCommentGroup); }
		}
		#endregion

		#region Constructor
		public HmiCommentProvider()
		{
			if (!m_Initialized)
			{
				m_Initialized = true;
				ReadFromStorage();
			}
		}
		#endregion

		#region Methods
		private static string m_FindGroupName = "";
		private static bool FindByGroupName(HmiCommentGroup group)
		{
			return m_FindGroupName == group.GroupName ? true : false;
		}

		private static ushort m_FindGroupNo = 0;
		private static bool FindByGroupNo(HmiCommentGroup group)
		{
			return m_FindGroupNo == group.GroupNo ? true : false;
		}

		public HmiCommentGroup FindGroup(string groupName)
		{
			m_FindGroupName = groupName;
			HmiCommentGroup group = m_Items.Find(FindByGroupName);
			return group;
		}

		public HmiCommentGroup FindGroup(ushort groupNo)
		{
			HmiCommentGroup group = m_Items.Find(FindByGroupNo);
			return group;
		}

		private bool IsExist(HmiCommentGroupInfo groupInfo)
		{ 
			bool exist = false;
			exist |= FindGroup(groupInfo.GroupName) != null;
			exist |= FindGroup(groupInfo.GroupNo) != null;

			return exist;
		}

		//0 보다 작음 : target의 값보다 작습니다. 
		//0 : target의 값과 같습니다. 
		//0 보다 큼 : target의 값보다 큽니다.
		private static int CompareByGroupNo(HmiCommentGroup source, HmiCommentGroup target)
		{
			if (source.GroupNo == target.GroupNo)
			{
				return 0;
			}
			else if (source.GroupNo < target.GroupNo)
			{
				return -1;
			}
			else
			{
				return 1;
			}
		}

		public static void SortByGroupNo(HmiCommentProvider source)
		{
			source.CommentGroups.Sort(CompareByGroupNo);
		}

		public ushort GetMaxGroupNo()
		{
			ushort max = 0;
			for (int i = 0; i < m_Items.Count; i++)
			{
				if (max < m_Items[i].GroupNo)
				{
					max = m_Items[i].GroupNo;
				}
			}
			return max;
		}

		public bool ReadFromStorage()
		{
			if (m_CheckPath == -1) CheckPath();
			if (m_CheckPath == 1)
			{
				return ReadFromStorage(m_FileName);
			}
			else return false;
		}

		private bool ReadFromStorage(string fileName)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(fileName);
				if (fileInfo.Exists)
				{
					m_FileName = fileName;
				}
				else
				{
					MessageBox.Show(this.GetType().Name + " File not found");
					OpenFileDialog dlg = new OpenFileDialog();
					dlg.Title = "Select XML file : " + this.GetType().Name;
					dlg.Filter = "XML files (*.xml)|*.xml|All files(*.*)|*.*";

					dlg.InitialDirectory = AppConfig.GetAppRootPath();

					if (DialogResult.OK == dlg.ShowDialog())
					{
						m_FileName = dlg.FileName;
					}
					else
					{
						if (DialogResult.Yes == MessageBox.Show("Do you want create new HmiCommentProvider file?", this.GetType().Name, MessageBoxButtons.YesNo))
						{
							FolderBrowserDialog dlg2 = new FolderBrowserDialog();
							dlg2.Description = "HmiCommentProvider File Folder Path to create";
							dlg2.SelectedPath = AppConfig.GetAppRootPath();
							if (DialogResult.OK == dlg2.ShowDialog())
							{
								m_FileName = string.Format("{0}\\{1}.xml", dlg2.SelectedPath, this.GetType().Name);
								this.WriteToStorage();
							}
						}

						return true;
					}
				}

				StreamReader sr = new StreamReader(m_FileName, Encoding.Default);
				XmlSerializer xmlSer = new XmlSerializer(this.GetType());

				HmiCommentProvider container = new HmiCommentProvider();
				container = xmlSer.Deserialize(sr) as HmiCommentProvider;
				sr.Close();

				m_Items.Clear();
				m_Items = container.CommentGroups;

				return true;
			}
			catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				MessageBox.Show(err.ToString());
				return false;
			}
		}

		public bool WriteToStorage()
		{
			if (m_CheckPath == -1) CheckPath();
			if (m_CheckPath == 1)
			{
				return WriteToStorage(m_FileName);
			}
			else return false;
		}

		private bool WriteToStorage(string fileName)
		{
			StreamWriter sw = null;
			XmlSerializer xmlSer = null;

			try
			{
				xmlSer = new XmlSerializer(this.GetType());

				// jemoon : 오류가 있는지 먼저 try
				sw = new StreamWriter(fileName + ".try", false, Encoding.Default);
				xmlSer.Serialize(sw, this);
				sw.Close();
				FileInfo file = new FileInfo(fileName + ".try");
				file.Delete();
			}
			catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				string msg = err.ToString();
				System.Windows.Forms.MessageBox.Show(err.ToString());

				if (sw != null) sw.Close();

				return false;
			}

			try
			{   // jemoon : 오류가 없으면 실제로 쓰자
				// jemoon : backup 본을 하나 만들고
				FileInfo file = new FileInfo(fileName);
				if (file.Exists)
				{
					file.CopyTo(fileName + ".old", true);
				}

				sw = new StreamWriter(fileName, false, Encoding.Default);
				xmlSer.Serialize(sw, this);
				sw.Close();

				m_FileName = fileName;

				return true;
			}
			catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				string msg = err.ToString();
				System.Windows.Forms.MessageBox.Show(err.ToString());

				if (sw != null) sw.Close();

				return false;
			}           
		}

		private void CheckPath()
		{
			m_AppConfig.ReadXml();
			string folderPath = m_AppConfig.DmsControlHmiConfigPathName;

			if (Directory.Exists(folderPath) == false)
			{
				MessageBox.Show("DmsControlHmi Config File Folder not found");
				FolderBrowserDialog dlg = new FolderBrowserDialog();
				dlg.SelectedPath = Application.StartupPath;
				dlg.Description = "DmsControlHmi Config File Folder";
				dlg.ShowNewFolderButton = true;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					folderPath = dlg.SelectedPath;
					m_AppConfig.DmsControlHmiConfigPath.SelectedFolder = folderPath;
					if (m_AppConfig.WriteXml())
					{
						m_FileName = string.Format("{0}\\{1}.xml", folderPath, this.GetType().Name);
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
				m_FileName = string.Format("{0}\\{1}.xml", folderPath, this.GetType().Name);
				m_CheckPath = 1;
			}
		}
		#endregion
	}
}
