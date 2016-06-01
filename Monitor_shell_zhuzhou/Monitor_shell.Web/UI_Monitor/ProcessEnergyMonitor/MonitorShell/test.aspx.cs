using Monitor_shell.Infrastructure.Configuration;
using Monitor_shell.Service.Formula;
using Monitor_shell.Service.MeterStatistics;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Monitor_shell.Web.UI_Monitor.ProcessEnergyMonitor.MonitorShell
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //FormulaHelper formulaService = new FormulaHelper();
            //formulaService.Claculate("zc_nxjc_byc_byf_cementmill01", "P0102");
            //string nxjcConn = ConnectionStringFactory.NXJCConnectionString;
            //ISqlServerDataFactory nxjcFactory = new SqlServerDataFactory(nxjcConn);
            //string ammeterConn = ConnectionStringFactory.GetAmmeterConnectionString("zc_nxjc_byc_byf_cementmill01");
            //ISqlServerDataFactory ammeterFactory = new SqlServerDataFactory(ammeterConn);
            //MeterStatisticsHelper helper = new MeterStatisticsHelper(nxjcFactory, ammeterFactory);
           // helper.GetMeterStatictisticsData("zc_nxjc_byc_byf_cementmill01", "hybridMaterialsPreparation", 10);

           // formulaService.GetDenominatorFormulaJson("zc_nxjc_byc_byf_cementmill01", "hybridMaterialsPreparation");
            //MeterStatisticsHelper.GetEquipmentInfo("", "");
        } 
    }
}