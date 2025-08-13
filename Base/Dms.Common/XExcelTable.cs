///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.11.16
// Author       : Kim Youngsik
// Description  : XExcelTable
///////////////////////////////////////////////////////////////////////////
// * Revision history
// * 
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace Dms.Common
{
    public enum SheetType
    {
        sheetVertical,
        sheetHorizontal,
    }

    public class XExcelTable
    {
        #region Fields
        public static object m_Lock = new object();

        private static int MAX_COLS = 255;
        private static int MAX_ROWS = 65535;

        //private StreamWriter m_StreamWriter = null;
        private string m_ExportString = "";
        private Color m_HeaderColor;
        private Color m_DefaultHeaderColor = Color.LightCyan;
        #endregion

        #region Singleton
        public static readonly XExcelTable Instance = new XExcelTable();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public XExcelTable()
        {

        }
        #endregion

        #region Methods
        /****************************************************************************************************
         * public bool CreateExcel(string path, string fileName, System.Data.DataTable table, options...)
         * 
         * string path : save path
         * string fileName : save fileName
         * System.Data.DataTable : data table
         * options
         *    ==> sheetType : Vertical Type과 Horizontal Type (dafault Vertical Type)
         *    ==> headerColor : Header의 Color 지정 (dafault LightCyan Color)
         * ****************************************************************************************************/

        //1. SheetType과 headerColor 모두 사용자 지정 생성
        public bool CreateExcel(string path, string fileName, DataTable table, SheetType type, Color headerColor)
        {
            m_HeaderColor = headerColor;

            return MakeFile(path, fileName, table, type);
        }

        //2. headerColor만 사용자 지정, SheetType은 Vertical
        public bool CreateExcel(string path, string fileName, DataTable table, Color headerColor)
        {
            m_HeaderColor = headerColor;

            return MakeFile(path, fileName, table, SheetType.sheetVertical);
        }

        //3. SheetType만 사용자 지정, headerColor는 default color 사용
        public bool CreateExcel(string path, string fileName, DataTable table, SheetType type)
        {
            m_HeaderColor = m_DefaultHeaderColor;

            return MakeFile(path, fileName, table, type);
        }

        //4. 모든 option을 default값 사용.
        public bool CreateExcel(string path, string fileName, DataTable table)
        {
            m_HeaderColor = m_DefaultHeaderColor;

            return MakeFile(path, fileName, table, SheetType.sheetVertical);
        }

        /****************************************************************************************************
         * public bool MakeFile(string path, string fileName, System.Data.DataTable table, SheetType sheetType)
         * 2010.11.11 Kim Youngsik
         * 
         * string path : save path
         * string fileName : save fileName
         * System.Data.DataTable : data table
         * SheetType sheetType : Sheet Type (Vertical or Horizontal)
        ****************************************************************************************************/
        private bool MakeFile(string path, string fileName, DataTable table, SheetType sheetType)
        {
            lock (m_Lock)
            {
                bool retVal = false;

                if (CheckDirectory(path) == false)
                {
                    return retVal;
                }

                //일단 Table의 Row와 Column이 모두 Max Size를 넘기면 일단 만들지 말자...
                if ((((sheetType == SheetType.sheetVertical) && (table.Columns.Count > MAX_COLS)) ||
                    ((sheetType == SheetType.sheetHorizontal) && (table.Rows.Count > MAX_COLS))) &&
                    (((sheetType == SheetType.sheetVertical) && (table.Rows.Count > 65535)) ||
                    ((sheetType == SheetType.sheetHorizontal) && (table.Columns.Count > 65535))))
                {
                    return retVal;
                }
                //만들어야 하는 Excel File이 Max Column(255)를 넘기면 File 분할
                else if (((sheetType == SheetType.sheetVertical) && (table.Columns.Count > MAX_COLS)) ||
                        ((sheetType == SheetType.sheetHorizontal) && (table.Rows.Count > MAX_COLS)))
                {
                    int fileNo = 1;

                    if (sheetType == SheetType.sheetVertical)
                    {
                        int startCol = 0;
                        int endCol = 0;

                        while (startCol < table.Columns.Count)
                        {
                            string excelName = string.Format("{0}{1}-{2}.xls", path, fileName, fileNo);

                            endCol = ((startCol + MAX_COLS) < table.Columns.Count) ? startCol + MAX_COLS : table.Columns.Count;

                            DataTable dataTable = new DataTable("temp");

                            for (int i = startCol; i < endCol; i++)
                            {
                                DataColumn column = new DataColumn();
                                column.ColumnName = table.Columns[i].ColumnName;
                                column.DataType = table.Columns[i].DataType;
                                column.Caption = table.Columns[i].Caption;

                                dataTable.Columns.Add(column);
                            }

                            int rowCount = table.Rows.Count;

                            for (int i = 0; i < rowCount; i++)
                            {
                                string[] rowValue = new string[endCol - startCol];

                                for (int j = startCol; j < endCol; j++)
                                {
                                    rowValue[j - startCol] = (string)table.Rows[i][j];
                                }

                                dataTable.Rows.Add(rowValue);
                            }

                            StreamWriter streamWriter = new StreamWriter(excelName, false, System.Text.Encoding.Default);

                            retVal = CreateExcelFile(streamWriter, dataTable, sheetType);

                            streamWriter.Close();
                            streamWriter = null;

                            fileNo++;

                            startCol += MAX_COLS;

                            if (retVal == false)
                            {
                                return retVal;
                            }
                        }
                    }
                    else
                    {
                        int startRow = 0;
                        int endRow = 0;

                        while (startRow < table.Rows.Count)
                        {
                            string excelName = string.Format("{0}{1}-{2}.xls", path, fileName, fileNo);

                            endRow = ((startRow + MAX_COLS) < table.Rows.Count) ? startRow + MAX_COLS : table.Rows.Count;

                            DataTable dataTable = new DataTable("temp");

                            int colCount = table.Columns.Count;

                            for (int i = 0; i < colCount; i++)
                            {
                                DataColumn column = new DataColumn();
                                column.ColumnName = table.Columns[i].ColumnName;
                                column.DataType = table.Columns[i].DataType;
                                column.Caption = table.Columns[i].Caption;

                                dataTable.Columns.Add(column);
                            }

                            int rowCount = (endRow - startRow);

                            for (int i = 0; i < rowCount; i++)
                            {
                                string[] rowValue = new string[colCount];

                                for (int j = 0; j < colCount; j++)
                                {
                                    rowValue[j] = (string)table.Rows[i + startRow][j];
                                }

                                dataTable.Rows.Add(rowValue);
                            }

                           StreamWriter streamWriter = new StreamWriter(excelName, false, System.Text.Encoding.Default);

                           retVal = CreateExcelFile(streamWriter, dataTable, sheetType);

                           streamWriter.Close();
                           streamWriter = null;

                            fileNo++;

                            startRow += MAX_COLS;

                            if (retVal == false)
                            {
                                return retVal;
                            }
                        }
                    }
                }
                //만들어야 하는 Excel File이 Max Row(65535)를 넘기면 File 분할
                else if (((sheetType == SheetType.sheetVertical) && (table.Rows.Count > MAX_ROWS)) ||
                        ((sheetType == SheetType.sheetHorizontal) && (table.Columns.Count > MAX_ROWS)))   //Excel의 Max Row가 65536이기 때문에 Table을 Check함.
                {
                    int fileNo = 1;

                    if (sheetType == SheetType.sheetVertical)
                    {
                        int startRow = 0;
                        int endRow = 0;

                        while (startRow < table.Rows.Count)
                        {
                            string excelName = string.Format("{0}{1}-{2}.xls", path, fileName, fileNo);

                            endRow = ((startRow + MAX_ROWS) < table.Rows.Count) ? startRow + MAX_ROWS : table.Rows.Count;

                            DataTable dataTable = new DataTable("temp");

                            int colCount = table.Columns.Count;

                            for (int i = 0; i < colCount; i++)
                            {
                                DataColumn column = new DataColumn();
                                column.ColumnName = table.Columns[i].ColumnName;
                                column.DataType = table.Columns[i].DataType;
                                column.Caption = table.Columns[i].Caption;

                                dataTable.Columns.Add(column);
                            }

                            for (int i = startRow; i < endRow; i++)
                            {
                                string[] rowValue = new string[table.Columns.Count];
                                table.Rows[i].ItemArray.CopyTo(rowValue, 0);

                                dataTable.Rows.Add(rowValue);
                            }

                            StreamWriter streamWriter = new StreamWriter(excelName, false, System.Text.Encoding.Default);

                            retVal = CreateExcelFile(streamWriter, dataTable, sheetType);

                            streamWriter.Close();
                            streamWriter = null;

                            fileNo++;

                            startRow += MAX_ROWS;

                            if (retVal == false)
                            {
                                return retVal;
                            }
                        }
                    }
                    else
                    {
                        int startCol = 0;
                        int endCol = 0;

                        while (startCol < table.Columns.Count)
                        {
                            string excelName = string.Format("{0}{1}-{2}.xls", path, fileName, fileNo);

                            endCol = ((startCol + MAX_ROWS) < table.Columns.Count) ? startCol + MAX_ROWS : table.Columns.Count;

                            DataTable dataTable = new DataTable("temp");

                            int colCount = table.Columns.Count;

                            for (int i = startCol; i < endCol; i++)
                            {
                                DataColumn column = new DataColumn();
                                column.ColumnName = table.Columns[i].ColumnName;
                                column.DataType = table.Columns[i].DataType;
                                column.Caption = table.Columns[i].Caption;

                                dataTable.Columns.Add(column);
                            }

                            int rowCount = table.Rows.Count;

                            for (int i = 0; i < rowCount; i++)
                            {
                                string[] rowValue = new string[colCount];

                                for (int j = startCol; j < endCol; j++)
                                {
                                    rowValue[j - startCol] = (string)table.Rows[i][j];
                                }

                                dataTable.Rows.Add(rowValue);
                            }

                            StreamWriter streamWriter = new StreamWriter(excelName, false, System.Text.Encoding.Default);

                            retVal = CreateExcelFile(streamWriter, dataTable, sheetType);

                            streamWriter.Close();
                            streamWriter = null;

                            fileNo++;

                            startCol += MAX_ROWS;

                            if (retVal == false)
                            {
                                return retVal;
                            }
                        }
                    }
                }
                else
                {
                    string excelName = path + fileName + ".xls";

                    StreamWriter streamWriter = new StreamWriter(excelName, false, System.Text.Encoding.Default);

                    retVal = CreateExcelFile(streamWriter, table, sheetType);

                    streamWriter.Close();
                    streamWriter = null;
                }

                TerminateExcelProcess();

                return retVal;
            }
        }

        private bool CheckDirectory(string path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                if (dirInfo.Exists == false)
                {
                    Directory.CreateDirectory(path);
                }

                dirInfo = null;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //StreamWriter를 이용하여 xls로 저장...
        private bool CreateExcelFile(StreamWriter streamWriter, DataTable table, SheetType sheetType)
        {
            try
            {
                string headerColor = string.Format("{0:X2}{1:X2}{2:X2}", m_HeaderColor.R, m_HeaderColor.G, m_HeaderColor.B);
                //Excel File Header...
                m_ExportString = "";
                m_ExportString += "<htmp xmlns:o=\"urn:schmas-microsoft-com:";
                m_ExportString += "office:office\"xmlns:x=\"urn:schemas-microsoft-com:office:excel\"";
                m_ExportString += "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"xmlns=\"http://www.w3.org/TR/REC-html40\">";
                m_ExportString += "<head><meta http-equiv=Content-Type content='text/html; charset=euc-kr'>";
                m_ExportString += "<style>.xl24{mso-number-format:\"\\@\";}.xl25{mso-style-parent:style0;mso-number-format:\"\\@\";}</style></head>";
                m_ExportString += "<table border=1>";

                if (sheetType == SheetType.sheetVertical)
                {
                    //첫번 째 Row에 DataTable Column Name을 넣는다...
                    int rowCount = table.Rows.Count + 1;
                    int colCount = table.Columns.Count;

                    for (int row = 0; row < rowCount; row++)
                    {
                        m_ExportString += "<tr>";
                        for (int col = 0; col < colCount; col++)
                        {
                            m_ExportString += "<td";
                            if (row == 0)
                            {
                                m_ExportString += " bgcolor=\"#" + headerColor + "\" align=center>" + table.Columns[col].ColumnName;
                            }
                            else
                            {
                                m_ExportString += " align=center>" + table.Rows[row - 1][col];
                            }
                            m_ExportString += "</td>";
                        }
                        m_ExportString += "</tr>";

                        streamWriter.Write(m_ExportString);
                        m_ExportString = "";
                    }

                    streamWriter.Write("</table></html>");
                }
                else
                {
                    //1번째 Column에 Data Table의 Column Name을 넣는다...
                    int rowCount = table.Columns.Count;
                    int colCount = table.Rows.Count + 1;

                    for (int row = 0; row < rowCount; row++)
                    {
                        m_ExportString += "<tr>";
                        for (int col = 0; col < colCount; col++)
                        {
                            m_ExportString += "<td";
                            if (col == 0)
                            {
                                m_ExportString += " bgcolor=\"#" + headerColor + "\" align=center>" + table.Columns[row].ColumnName;
                            }
                            else
                            {
                                m_ExportString += " align=center>" + table.Rows[col - 1][row];
                            }
                            m_ExportString += "</td>";
                        }
                        m_ExportString += "</tr>";

                        streamWriter.Write(m_ExportString);
                        m_ExportString = "";
                    }

                    streamWriter.Write("</table></html>");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void TerminateExcelProcess()
        {
            System.Diagnostics.Process[] pProcess;
            pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");

            int count = pProcess.Length;

            for (int i = 0; i < count; i++)
            {
                pProcess[i].Kill();
            }
        }
        #endregion
    }
}
