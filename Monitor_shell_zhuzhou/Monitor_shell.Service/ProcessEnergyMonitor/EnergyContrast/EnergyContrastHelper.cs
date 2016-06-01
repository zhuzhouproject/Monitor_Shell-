using Monitor_shell.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Monitor_shell.Service.ProcessEnergyMonitor.EnergyContrast
{
    public class EnergyContrastHelper
    {
        private ISqlServerDataFactory _dataFactory;
        private string _type;
        public EnergyContrastHelper(string type)
        {
            _type = type;
        }

        /// <summary>
        /// 获得实时数据
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public IEnumerable<DataItem> GetRealtimeDatas(string organizationId, IList<string> variableNames)
        {
            IList<DataItem> result = new List<DataItem>();

            string ammeterConn = ConnectionStringFactory.GetAmmeterConnectionString(organizationId);
            //string ammeterConn = "Data Source=QH-20150320GFTA;Initial Catalog=zc_nxjc_byc_byf;Integrated Security=True;";
            _dataFactory = new SqlServerDataFactory(ammeterConn);

            IDictionary<string, List<FieldInformation>> fieldInformations = GetFeildInformation(organizationId, variableNames);

            DataTable table = GetDataItemTable(fieldInformations);
            string[] idList = GetTableColumnName(table);
            foreach (var item in idList)
            {
                string t_value="0";
                if (_type == "RunningState")
                {
                    t_value = table.Rows[0][item] is DBNull ? "0" : Convert.ToDecimal(table.Rows[0][item]).ToString().Trim();
                }
                else
                {
                    t_value = table.Rows[0][item] is DBNull ? "0" : Convert.ToDecimal(table.Rows[0][item]).ToString("#").Trim();
                }
                result.Add(new DataItem
                {
                    ID = organizationId + ">" + item + ">" + _type,
                    Value = t_value//table.Rows[0][item] is DBNull ? "0" : Convert.ToDecimal(table.Rows[0][item]).ToString("#").Trim()
                });
            }
            return result;
        }
        /// <summary>
        /// 获得FeildInformation
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private IDictionary<string, List<FieldInformation>> GetFeildInformation(string organizationId, IList<string> variableNames)
        {
            Dictionary<string, List<FieldInformation>> fieldInformations = new Dictionary<string, List<FieldInformation>>();
            if (variableNames.Count > 0)
            {
                StringBuilder queryString = new StringBuilder();
                List<SqlParameter> parameters = new List<SqlParameter>();
                queryString.Append("select DatabaseName,TableName,FieldName,VariableName from MonitorContrast ");
                queryString.Append("where OrganizationID=@organizationId and type=@type and Enabled=@enabled and (");
                parameters.Add(new SqlParameter("@enabled", 1));
                parameters.Add(new SqlParameter("@organizationId", organizationId));
                parameters.Add(new SqlParameter("@type", _type));
                foreach (var item in variableNames)
                {
                    queryString.Append("VariableName=@"+ item + " or ");
                    parameters.Add(new SqlParameter("@" + item, item));
                }
                queryString.Remove(queryString.Length - 4, 4).Append(")");
                DataTable table = _dataFactory.Query(queryString.ToString(), parameters.ToArray());
                foreach (DataRow row in table.Rows)
                {
                    FieldInformation fieldInfor = new FieldInformation();
                    string key = row["DatabaseName"].ToString().Trim() + ".dbo.Realtime_" + row["TableName"].ToString().Trim();
                    fieldInfor.FeildName = row["FieldName"].ToString().Trim();
                    fieldInfor.VariableName = row["VariableName"].ToString().Trim();
                    if (fieldInformations.Keys.Contains(key))
                    {
                        fieldInformations[key].Add(fieldInfor);
                    }
                    else
                    {
                        fieldInformations.Add(key, new List<FieldInformation>());
                        fieldInformations[key].Add(fieldInfor);
                    }
                }
            }
            return fieldInformations;
        }

        private DataTable GetDataItemTable(IDictionary<string, List<FieldInformation>> fieldInformations)
        {
            DataTable result = new DataTable();
            if (fieldInformations.Keys.Count() > 0)
            {
                string queryString = GetSqlString(fieldInformations);
                result = _dataFactory.Query(queryString);
            }
            return result;
        }

        /// <summary>
        /// 获得table的字段名
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string[] GetTableColumnName(DataTable dt)
        {
            IList<string> result = new List<string>();
            foreach (DataColumn item in dt.Columns)
            {
                result.Add(item.ColumnName);
            }
            return result.ToArray();
        }

        public static string GetSqlString(IDictionary<string, List<FieldInformation>> feildInfor)
        {
            StringBuilder result = new StringBuilder();
            result.Append("select ");
            StringBuilder feilds = new StringBuilder();
            StringBuilder tables = new StringBuilder();

            foreach (var key in feildInfor.Keys)
            {
                tables.Append(key + ",");
                foreach (var feild in feildInfor[key])
                {
                    string tempFeild = key + ".[" + feild.FeildName + "] as " + feild.VariableName;
                    feilds.Append(tempFeild).Append(",") ;
                }
            }
            tables.Remove(tables.Length - 1, 1);
            feilds.Remove(feilds.Length - 1, 1);
            result.Append(feilds.ToString()).Append(" from ").Append(tables.ToString());

            return result.ToString();
        }
    }
}
