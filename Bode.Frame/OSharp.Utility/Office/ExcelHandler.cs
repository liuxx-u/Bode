using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace OSharp.Utility.Office
{
    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHandler
    {
        private string _filePath, _connectionString;

        public ExcelHandler(string filePath) 
        {
            _filePath = filePath;
            if (filePath.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties='Excel 12.0';";
            }
            else
            {
                _connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filePath + "';Extended Properties='Excel 8.0';";
            }
        }

        #region 数据导出至Excel文件

        /// <summary>
        /// 将数据导出至Excel文件
        /// </summary>
        /// <param name="table">DataTable对象</param>
        public bool OutputToExcel(DataTable table)
        {
            if (File.Exists(_filePath))
            {
                throw new Exception("该文件已经存在！");
            }

            if ((table.TableName.Trim().Length == 0) || (table.TableName.ToLower() == "table"))
            {
                table.TableName = "Sheet1";
            }

            //数据表的列数
            int colCount = table.Columns.Count;

            //用于记数，实例化参数时的序号
            int i = 0;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[colCount];

            //创建表结构的SQL语句
            string tableStructStr = @"Create Table " + table.TableName + "(";

            //连接字符串
            OleDbConnection objConn = new OleDbConnection(_connectionString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList dataTypeList = new ArrayList();
            dataTypeList.Add("System.Decimal");
            dataTypeList.Add("System.Double");
            dataTypeList.Add("System.Int16");
            dataTypeList.Add("System.Int32");
            dataTypeList.Add("System.Int64");
            dataTypeList.Add("System.Single");

            //遍历数据表的所有列，用于创建表结构
            foreach (DataColumn col in table.Columns)
            {
                //如果列属于数字列，则设置该列的数据类型为double
                if (dataTypeList.IndexOf(col.DataType.ToString()) >= 0)
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.Double);
                    objCmd.Parameters.Add(para[i]);

                    //如果是最后一列
                    if (i + 1 == colCount)
                    {
                        tableStructStr += col.ColumnName + " double)";
                    }
                    else
                    {
                        tableStructStr += col.ColumnName + " double,";
                    }
                }
                else
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.VarChar);
                    objCmd.Parameters.Add(para[i]);

                    //如果是最后一列
                    if (i + 1 == colCount)
                    {
                        tableStructStr += col.ColumnName + " varchar)";
                    }
                    else
                    {
                        tableStructStr += col.ColumnName + " varchar,";
                    }
                }
                i++;
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = tableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string insertSql1 = "Insert into " + table.TableName + " (";
            string insertSql2 = " Values (";
            string insertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colId = 0; colId < colCount; colId++)
            {
                if (colId + 1 == colCount)  //最后一列
                {
                    insertSql1 += table.Columns[colId].ColumnName + ")";
                    insertSql2 += "@" + table.Columns[colId].ColumnName + ")";
                }
                else
                {
                    insertSql1 += table.Columns[colId].ColumnName + ",";
                    insertSql2 += "@" + table.Columns[colId].ColumnName + ",";
                }
            }

            insertSql = insertSql1 + insertSql2;

            //遍历数据表的所有数据行
            for (int rowId = 0; rowId < table.Rows.Count; rowId++)
            {
                for (int colId = 0; colId < colCount; colId++)
                {
                    if (para[colId].DbType == DbType.Double && table.Rows[rowId][colId].ToString().Trim() == "")
                    {
                        para[colId].Value = 0;
                    }
                    else
                    {
                        para[colId].Value = table.Rows[rowId][colId].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = insertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }

        /// <summary>
        /// 将数据导出至Excel文件
        /// </summary>
        /// <param name="table">DataTable对象</param>
        /// <param name="columns">要导出的数据列集合</param>
        public bool OutputToExcel(DataTable table, ArrayList columns)
        {
            if (File.Exists(_filePath))
            {
                throw new Exception("该文件已经存在！");
            }

            //如果数据列数大于表的列数，取数据表的所有列
            if (columns.Count > table.Columns.Count)
            {
                for (int s = table.Columns.Count + 1; s <= columns.Count; s++)
                {
                    columns.RemoveAt(s);   //移除数据表列数后的所有列
                }
            }

            //遍历所有的数据列，如果有数据列的数据类型不是 DataColumn，则将它移除
            DataColumn column = new DataColumn();
            for (int j = 0; j < columns.Count; j++)
            {
                try
                {
                    column = (DataColumn)columns[j];
                }
                catch (Exception)
                {
                    columns.RemoveAt(j);
                }
            }
            if ((table.TableName.Trim().Length == 0) || (table.TableName.ToLower() == "table"))
            {
                table.TableName = "Sheet1";
            }

            //数据表的列数
            int colCount = columns.Count;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[colCount];

            //创建表结构的SQL语句
            string tableStructStr = @"Create Table " + table.TableName + "(";

            //连接字符串
            OleDbConnection objConn = new OleDbConnection(_connectionString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList dataTypeList = new ArrayList();
            dataTypeList.Add("System.Decimal");
            dataTypeList.Add("System.Double");
            dataTypeList.Add("System.Int16");
            dataTypeList.Add("System.Int32");
            dataTypeList.Add("System.Int64");
            dataTypeList.Add("System.Single");

            //遍历数据表的所有列，用于创建表结构
            for (int k = 0; k < colCount; k++)
            {
                var col = (DataColumn)columns[k];

                //列的数据类型是数字型
                if (dataTypeList.IndexOf(col.DataType.ToString().Trim()) >= 0)
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.Double);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == colCount)
                    {
                        tableStructStr += col.Caption.Trim() + " Double)";
                    }
                    else
                    {
                        tableStructStr += col.Caption.Trim() + " Double,";
                    }
                }
                else
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.VarChar);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == colCount)
                    {
                        tableStructStr += col.Caption.Trim() + " VarChar)";
                    }
                    else
                    {
                        tableStructStr += col.Caption.Trim() + " VarChar,";
                    }
                }
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = tableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string insertSql1 = "Insert into " + table.TableName + " (";
            string insertSql2 = " Values (";
            string insertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colID = 0; colID < colCount; colID++)
            {
                if (colID + 1 == colCount)  //最后一列
                {
                    insertSql1 += columns[colID].ToString().Trim() + ")";
                    insertSql2 += "@" + columns[colID].ToString().Trim() + ")";
                }
                else
                {
                    insertSql1 += columns[colID].ToString().Trim() + ",";
                    insertSql2 += "@" + columns[colID].ToString().Trim() + ",";
                }
            }

            insertSql = insertSql1 + insertSql2;

            //遍历数据表的所有数据行
            DataColumn dataCol;
            for (int rowId = 0; rowId < table.Rows.Count; rowId++)
            {
                for (int colID = 0; colID < colCount; colID++)
                {
                    //因为列不连续，所以在取得单元格时不能用行列编号，列需得用列的名称
                    dataCol = (DataColumn)columns[colID];
                    if (para[colID].DbType == DbType.Double && table.Rows[rowId][dataCol.Caption].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = table.Rows[rowId][dataCol.Caption].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = insertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 获取Excel文件数据表列表
        /// </summary>
        public ArrayList GetExcelTables()
        {
            ArrayList tablesList = new ArrayList();
            if (File.Exists(_filePath))
            {
                using (OleDbConnection conn = new OleDbConnection(_connectionString))
                {
                    DataTable dt;
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //获取数据表个数
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (tablesList.IndexOf(tablename) < 0)
                        {
                            tablesList.Add(tablename);
                        }
                    }
                }
            }
            return tablesList;
        }

        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// </summary>
        /// <param name="tableName">数据表名，如果数据表名错误，默认为第一个数据表名</param>
        public DataTable InputFromExcel(string tableName = "Sheet1$")
        {
            if (!File.Exists(_filePath))
            {
                throw new Exception("Excel文件不存在！");
            }

            DataTable table = new DataTable();

            OleDbConnection dbcon = new OleDbConnection(_connectionString);
            OleDbCommand cmd = new OleDbCommand("select * from [" + tableName + "]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// 获取Excel文件指定数据表的数据列表
        /// </summary>
        /// <param name="tableName">数据表名</param>
        public ArrayList GetExcelTableColumns(string tableName)
        {
            ArrayList colsList = new ArrayList();
            if (File.Exists(_filePath))
            {
                using (OleDbConnection conn = new OleDbConnection(_connectionString))
                {
                    conn.Open();
                    var dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });

                    //获取列个数
                    int colcount = dt.Rows.Count;
                    for (int i = 0; i < colcount; i++)
                    {
                        string colname = dt.Rows[i]["Column_Name"].ToString().Trim();
                        colsList.Add(colname);
                    }
                }
            }
            return colsList;
        }
    }
}