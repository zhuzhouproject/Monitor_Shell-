using Monitor_shell.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Monitor_shell.Service.ProcessEnergyMonitor.TargetValue
{
    public class EnergyConsumptionTargetValue
    {
        public static IEnumerable<DataItem> TargetValueItems(string keyitem, string[] mvariableids) 
        {
            IList<DataItem> results = new List<DataItem>();
            string[] items = keyitem.Split(',');
            string organizationId=items[0];
            string variableId = items[1]; 
            string conString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory datafactory = new SqlServerDataFactory(conString);
            if (variableId.Equals("TargetOverall")) 
            {
                int length = mvariableids.Length;
                string mySql = @"";
                for (int i = 0; i < length;i++ )
                {
                    mySql = mySql + @"select A.OrganizationID,A.KeyID,B.VariableId,B.Target_Overall from [dbo].[tz_Formula] A,[dbo].[formula_FormulaDetail] B
                            where A.OrganizationId='" +organizationId+@"' and A.Type=2 and A.ENABLE=1 and A.State=0
                            and A.KeyID=B.KeyID and B.LevelType='Process'and B.VariableId='" + mvariableids[i]+ "' union ";
                }
                mySql= mySql.Remove(mySql.Length-7,7);
                DataTable table=datafactory.Query(mySql);
                 if (table.Rows.Count > 0)
                 {
                     foreach(DataRow dr in table.Rows){
                         DataItem dataItem = new DataItem()
                         {
                             ID = dr["OrganizationID"] + ">" + dr["VariableId"] + ">TargetOverall",
                             Value = dr["Target_Overall"] is DBNull ? "0" : Convert.ToDecimal(dr["Target_Overall"]).ToString().Trim()
                         };
                         results.Add(dataItem);                      
                     }                     
                 }
            }
            else if (variableId.Equals("ThreeTarget"))
            {
                string mySql = @"select A.OrganizationID,A.KeyID,B.VariableId,B.Target_Moment,B.Target_Class,B.Target_Day from [dbo].[tz_Formula] A,[dbo].[formula_FormulaDetail] B
                                    where A.OrganizationId=@organizationId and A.Type=2 and A.ENABLE=1 and A.State=0
                                    and A.KeyID=B.KeyID and B.LevelType='Process'and B.VariableId=@variableId";
                SqlParameter[] para = { new SqlParameter("@organizationId", organizationId), new SqlParameter("@variableId", mvariableids[0]) };
                DataTable table = datafactory.Query(mySql, para);
                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    DataItem itemMoment = new DataItem
                    {
                        ID = dr["OrganizationID"].ToString().Trim() + ">" + dr["VariableId"].ToString().Trim() + "_TargetMoment>ThreeTarget",
                        Value = dr["Target_Moment"] is DBNull ? "0" : Convert.ToDecimal(dr["Target_Moment"]).ToString().Trim()
                    };
                    DataItem itemClass = new DataItem
                    {
                        ID = dr["OrganizationID"].ToString().Trim() + ">" + dr["VariableId"].ToString().Trim() + "_TargetClass>ThreeTarget",
                        Value = dr["Target_Class"] is DBNull ? "0" : Convert.ToDecimal(dr["Target_Class"]).ToString().Trim()
                    };
                    DataItem itemDay = new DataItem
                    {
                        ID = dr["OrganizationID"].ToString().Trim() + ">" + dr["VariableId"].ToString().Trim() + "_TargetDay>ThreeTarget",
                        Value = dr["Target_Day"] is DBNull ? "0" : Convert.ToDecimal(dr["Target_Day"]).ToString().Trim()
                    };
                    results.Add(itemMoment);
                    results.Add(itemClass);
                    results.Add(itemDay);
                }
            }
            return results;
        }
    }
}
