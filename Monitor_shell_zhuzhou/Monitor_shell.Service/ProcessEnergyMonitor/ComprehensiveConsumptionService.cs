using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SqlServerDataAdapter;
using Monitor_shell.Infrastructure.Configuration;
namespace Monitor_shell.Service.ProcessEnergyMonitor
{
    public class ComprehensiveConsumptionService
    {
        private const string Company = "Company";
        private const string Factory = "Factory";
        private const string ProductionLine = "ProductionLine";

        private static readonly SqlServerDataFactory _dataFactory = new SqlServerDataFactory(ConnectionStringFactory.NXJCConnectionString);
        public static Standard_GB16780_2012.Model_CaculateValue GetComprehensiveData(string myOrganizationId, string myVariableId)
        {
            Standard_GB16780_2012.Function_EnergyConsumption_V1 EnergyConsumption_V1 = new Standard_GB16780_2012.Function_EnergyConsumption_V1();
            Standard_GB16780_2012.Parameters_ComprehensiveData m_ComprehensiveData = AutoSetParameters.AutoSetParameters_V1.SetComprehensiveParametersEntity();

            DataTable table = Monitor_shell.Service.ProcessEnergyMonitor.ComprehensiveConsumptionService.GetEnergyValue(myOrganizationId);
            if (table != null)
            {
                EnergyConsumption_V1.LoadComprehensiveData(table, m_ComprehensiveData, "VariableId", "monthBalance");

                if (myVariableId == "clinker_ElectricityConsumption_Comprehensive")
                {
                    return EnergyConsumption_V1.GetClinkerPowerConsumptionWithFormula();
                }
                else if (myVariableId == "clinker_CoalConsumption_Comprehensive")
                {
                    return EnergyConsumption_V1.GetClinkerCoalConsumptionWithFormula();
                }
                else if (myVariableId == "cementmill_ElectricityConsumption_Comprehensive")
                {
                    decimal m_Clinker_ElectricityConsumption_Comprehensive = EnergyConsumption_V1.GetClinkerPowerConsumption();
                    return EnergyConsumption_V1.GetCementPowerConsumptionWithFormula(m_Clinker_ElectricityConsumption_Comprehensive);
                }
                else if (myVariableId == "cementmill_CoalConsumption_Comprehensive")
                {
                    decimal m_Cementmill_CoalConsumption_Comprehensive = EnergyConsumption_V1.GetClinkerCoalConsumption();
                    return EnergyConsumption_V1.GetCementCoalConsumptionWithFormula(m_Cementmill_CoalConsumption_Comprehensive);
                }
            }
            return null;
        }

        public static DataTable GetEnergyValue(string myOrganizationId)
        {
            DataTable m_OrganizationInfo = GetOrganizationInfo(myOrganizationId);
            if (m_OrganizationInfo != null && m_OrganizationInfo.Rows.Count > 0)
            {
                string m_OrganizationType = m_OrganizationInfo.Rows[0]["LevelType"].ToString();
                string m_LevelCode = m_OrganizationInfo.Rows[0]["LevelCode"].ToString();
                string dataString;
                if (m_OrganizationType == Company)           //公司级综合能耗
                {
                    dataString = @"select D.VariableId as VariableId,
                                        sum(D.monthBalance) as monthBalance
                                        from
                                        (select 
                                           B.OrganizationID as OrganizationID,
                                           (case when B.VariableId = 'clinker_ClinkerFactoryTransportInput' then 'clinker_ClinkerInput'
                                               when B.VariableId = 'clinker_ClinkerCompanyTransportInput' then 'clinker_ClinkerOutsourcingInput'
                                               else B.VariableId end) as VariableId,
                                           SUM(B.TotalPeakValleyFlatB) as monthBalance 
                                        from tz_Balance as A, balance_Energy as B, system_Organization as C 
                                        where A.BalanceId=B.KeyId
                                        and (B.ValueType = 'ElectricityQuantity' or B.ValueType = 'MaterialWeight')
                                        and A.TimeStamp like CONVERT(varchar(7),GETDATE(),20) + '%'
                                        and A.StaticsCycle = 'day'
                                        and B.OrganizationID = C.OrganizationID
                                        and C.LevelCode like @LevelCode + '%'
                                        and (C.Type = '熟料' or C.Type = '水泥磨')
                                        group by B.OrganizationID, B.VariableId
                                        union 
                                        select 
                                            A.OrganizationID as OrganizationID,
                                            (case when A.VariableId = 'clinker_ClinkerFactoryTransportInput' then 'clinker_ClinkerInput'
                                                 when A.VariableId = 'clinker_ClinkerCompanyTransportInput' then 'clinker_ClinkerOutsourcingInput'
                                                 else A.VariableId end) as VariableId,
                                            (case when CumulantDay is null then 0 else CumulantDay end) as monthBalance
                                        from RealtimeIncrementCumulant AS A, system_Organization B
                                        where 
                                        A.OrganizationID = B.OrganizationID
                                        and B.LevelCode like @LevelCode + '%'
                                        and (B.Type = '熟料' or B.Type = '水泥磨')
                                        ) D
                                        group by D.VariableId";
                }
                else if (m_OrganizationType == Factory || m_OrganizationType == ProductionLine)              //分厂级、产线级综合能耗
                {
                    dataString = @"select D.VariableId as VariableId,
                                        sum(D.monthBalance) as monthBalance
                                        from
                                        (select 
                                           B.OrganizationID as OrganizationID,
                                           (case when B.VariableId = 'clinker_ClinkerFactoryTransportInput' then 'clinker_ClinkerOutsourcingInput'
                                               when B.VariableId = 'clinker_ClinkerCompanyTransportInput' then 'clinker_ClinkerOutsourcingInput'
                                               else B.VariableId end) as VariableId,
                                           SUM(B.TotalPeakValleyFlatB) as monthBalance 
                                        from tz_Balance as A, balance_Energy as B, system_Organization as C 
                                        where A.BalanceId=B.KeyId
                                        and (B.ValueType = 'ElectricityQuantity' or B.ValueType = 'MaterialWeight')
                                        and A.TimeStamp like CONVERT(varchar(7),GETDATE(),20) + '%'
                                        and A.StaticsCycle = 'day'
                                        and B.OrganizationID = C.OrganizationID
                                        and C.LevelCode like @LevelCode + '%'
                                        and (C.Type = '熟料' or C.Type = '水泥磨')
                                        group by B.OrganizationID, B.VariableId
                                        union 
                                        select 
                                            A.OrganizationID as OrganizationID,
                                            (case when A.VariableId = 'clinker_ClinkerFactoryTransportInput' then 'clinker_ClinkerOutsourcingInput'
                                                 when A.VariableId = 'clinker_ClinkerCompanyTransportInput' then 'clinker_ClinkerOutsourcingInput'
                                                 else A.VariableId end) as VariableId,
                                            (case when CumulantDay is null then 0 else CumulantDay end) as monthBalance
                                        from RealtimeIncrementCumulant AS A, system_Organization B
                                        where 
                                        A.OrganizationID = B.OrganizationID
                                        and B.LevelCode like @LevelCode + '%'
                                        and (B.Type = '熟料' or B.Type = '水泥磨')
                                        ) D
                                        group by D.VariableId";
                }
                else       //其它为集团级综合能耗
                {
                    dataString = @"select D.VariableId as VariableId,
                                        sum(D.monthBalance) as monthBalance
                                        from
                                        (select 
                                           B.OrganizationID as OrganizationID,
                                           (case when B.VariableId = 'clinker_ClinkerFactoryTransportInput' then 'clinker_ClinkerInput'
                                               when B.VariableId = 'clinker_ClinkerCompanyTransportInput' then 'clinker_ClinkerInput'
                                               else B.VariableId end) as VariableId,
                                           SUM(B.TotalPeakValleyFlatB) as monthBalance 
                                        from tz_Balance as A, balance_Energy as B, system_Organization as C 
                                        where A.BalanceId=B.KeyId
                                        and (B.ValueType = 'ElectricityQuantity' or B.ValueType = 'MaterialWeight')
                                        and A.TimeStamp like CONVERT(varchar(7),GETDATE(),20) + '%'
                                        and A.StaticsCycle = 'day'
                                        and B.OrganizationID = C.OrganizationID
                                        and C.LevelCode like @LevelCode + '%'
                                        and (C.Type = '熟料' or C.Type = '水泥磨')
                                        group by B.OrganizationID, B.VariableId
                                        union 
                                        select 
                                            A.OrganizationID as OrganizationID,
                                            (case when A.VariableId = 'clinker_ClinkerFactoryTransportInput' then 'clinker_ClinkerInput'
                                                 when A.VariableId = 'clinker_ClinkerCompanyTransportInput' then 'clinker_ClinkerInput'
                                                 else A.VariableId end) as VariableId,
                                            (case when CumulantDay is null then 0 else CumulantDay end) as monthBalance
                                        from RealtimeIncrementCumulant AS A, system_Organization B
                                        where 
                                        A.OrganizationID = B.OrganizationID
                                        and B.LevelCode like @LevelCode + '%'
                                        and (B.Type = '熟料' or B.Type = '水泥磨')
                                        ) D
                                        group by D.VariableId";
                }
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LevelCode", m_LevelCode));
                try
                {
                    DataTable table = _dataFactory.Query(dataString, parameters.ToArray());
                    return table;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private static DataTable GetOrganizationInfo(string myOrganizationId)
        {
            string m_Sql = @"select A.LevelCode as LevelCode, A.LevelType as LevelType from system_Organization A
                     where A.OrganizationID = @OrganizationID";
            List<SqlParameter> m_Parameters = new List<SqlParameter>();
            m_Parameters.Add(new SqlParameter("@OrganizationID", myOrganizationId));
            DataTable table = _dataFactory.Query(m_Sql, m_Parameters.ToArray());
            return table;
        }
    }
}
