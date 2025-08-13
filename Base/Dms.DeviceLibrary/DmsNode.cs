using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using Dms.Common;

namespace Dms.DeviceLibrary
{
    public delegate bool GetDmsNodeDirectoryPath(ref string path);

    public abstract class DmsNode
    {
        #region Fields
        protected DmsNode m_Parent = null;
        protected TreeNode m_Node = new TreeNode();
        protected int m_Order;
        protected string m_Name;
        protected string m_Genealogy = "";
        protected static bool m_IsConfigMode = false;
        protected static bool m_CreateMode = false;
        protected string m_OriginalName;
        protected bool m_NodeCreated = false;
        protected DmsSerializingService m_Config = new DmsSerializingService();
        protected GetDmsNodeDirectoryPath m_GetDmsNodeDirectory;
        #endregion

        #region properties
        [Category("DMS : Node Info")]
        [Browsable(false)]
        public string Genealogy
        {
            get { return m_Genealogy; }
        }
        [Browsable(false), XmlIgnore()]
        public bool NodeCreated
        {
            get { return m_NodeCreated; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Don't call this in the any constructor
        /// </summary>
        /// <returns></returns>
        public virtual bool Initialize()
        {
            m_NodeCreated = true;
            return true;
        }
        public TreeNode GetNode()
        {
            return m_Node;
        }

        /// <summary>
        /// RootNode로 부터 Genealogy를 검색하여 해당하는 Node를 Return 한다. 
        /// </summary>
        /// <param name="rootNode">Root node</param>
        /// <param name="genealogy">Genealogy를 넣어준다.</param>
        /// <returns>DmsNode</returns>
        public DmsNode GetNode(DmsNode rootNode, string genealogy)
        {
            TreeNode node = new TreeNode();
            node = rootNode.m_Node;
            string[] orders = genealogy.Split('.');
            int count = orders.Length;
            for (int i = 1; i < count; i++)
            {
                node = node.Nodes[Convert.ToInt32(orders[i])];
            }
            return (DmsNode)node.Tag;
        }

        public DmsNode AddNode(DmsNode parent, int order, string name)
        {
            m_Parent = parent;
            m_Order = order;
            m_Genealogy = GetMyGenealogy(order);
            m_Node.Tag = this;
            m_Node.Text = string.Format("{0}[{1}]{2}", this.GetType().Name, m_Genealogy, name);
            m_Name = m_Node.Text;
            if (parent != null)
            {
                m_Parent.m_Node.Nodes.Add(m_Node);
            }
            m_OriginalName = name;
            return this;
        }

        public void SetConfigMode(bool on)
        {
            m_IsConfigMode = on;
        }

        public void SetCreateMode(bool on)
        {
            m_CreateMode = on;
        }

        protected string GetMyGenealogy(int order)
        {
            DmsNode parent;
            parent = m_Parent;
            string gen = string.Format("{0}", order);
            while (parent != null)
            {
                gen = string.Format("{0}.{1}", parent.m_Order, gen);
                parent = parent.m_Parent;
            }
            return gen;
        }

        protected string GetPathName()
        {
            string dirPath = "";
            if (m_GetDmsNodeDirectory != null)
            {
                if (!m_GetDmsNodeDirectory(ref dirPath))
                {
                    dirPath = @"..\Configfiles\Configuration";
                }
            }
            else
            {
                dirPath = @"..\Configfiles\Configuration";
            }
            string path = string.Format("{0}\\{1}.xml", dirPath, m_Name);
            return path;
        }

        public string GetOriginalName()
        {
            return m_OriginalName;
        }

        public string GetParentName()
        {
            if (m_Parent == null) return "";
            else return m_Parent.GetOriginalName();
        }

        // Class전체를 Serialize하고 Deserialize할때 사용(Property)
        // 이 함수를 사용할때는 아래 ReadConfiguration(DmsSerializingService dss, string filename); 함수 원형만 구현하고 
        // Initialize()에 이 함수를 사용함.
        protected void ReadConfiguration(object obj)  
        {
            if (m_Config.ReadXml(ref obj, this.GetType(), GetPathName()))
            {
                PropertyInfo[] propertyInfos = this.GetType().GetProperties();
                foreach (PropertyInfo info in propertyInfos)
                {
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();

                    object value = getMethodInfo.Invoke(obj, null);
                    object[] parameters = new object[] { value };
                    if (setMethodInfo != null)
                        setMethodInfo.Invoke(this, parameters);
                }
            }
		}

        public virtual void GetDmsNodeNameList(TreeNode node, List<DmsNodeTag> list, Type type)
        {
            DmsNode tagNode = node.Tag as DmsNode;

            if (tagNode != null)
            {
                if (tagNode.GetType() == type)
                {
                    DmsNodeTag tag = new DmsNodeTag();
                    tag.Name = tagNode.GetOriginalName();
                    list.Add(tag);
                }
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                GetDmsNodeNameList(childNode, list, type);
            }
        }
        #endregion

        #region Abstract methods
        protected abstract void ReadConfiguration(DmsSerializingService dss, string filename);
        public abstract void WriteConfiguration();
        #endregion
    }
}
