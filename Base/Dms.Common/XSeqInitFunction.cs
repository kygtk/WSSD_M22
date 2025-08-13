using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Dms.Common
{
    public class XSeqInitFunction : XSeqFunction
    {
        protected GenericTags m_InitCheckItems = new GenericTags();

        public GenericTags InitCheckItems
        {
            get { return m_InitCheckItems; }
        }

        public XSeqInitFunction()
        {
            GenerateCheckItems();
        }

        protected void GenerateCheckItems()
        {
            try
            {
                FieldInfo[] fieldInfos = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo fieldInfo;
                int fieldCount = fieldInfos.Length;
                for (int i = 0; i < fieldCount; i++)
                {
                    fieldInfo = fieldInfos[i];
                    if (fieldInfo.FieldType == typeof(GenericTag))
                    {
                        GenericTag item = fieldInfo.GetValue(this) as GenericTag;
                        if (item != null)
                        {
                            m_InitCheckItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
