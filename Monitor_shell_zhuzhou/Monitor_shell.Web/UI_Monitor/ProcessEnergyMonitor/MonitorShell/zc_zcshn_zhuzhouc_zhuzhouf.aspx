<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="zc_zcshn_zhuzhouc_zhuzhouf.aspx.cs" Inherits="Monitor_shell.Web.UI_Monitor.ProcessEnergyMonitor.MonitorShell.zc_zcshn_zhuzhouc_zhuzhouf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
  <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <link rel="stylesheet" type="text/css" href="/UI_Monitor/css/common/mymonitor.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/js/common/jquery.utility.js" charset="utf-8"></script>
    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->

    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script src="/UI_Monitor/js/common/monitorjs.js"></script>
    <script src="/UI_Monitor/js/common/chartjs.js"></script>
    <script src="/UI_Monitor/js/common/SubMonitorAdapter.js"></script>
    <script src="/UI_Monitor/js/common/alarm.js"></script>
    <script src="/UI_Monitor/js/common/multiTagChart.js"></script>
    <script src="/UI_Monitor/js/common/RunningState.js"></script>
    <script src="/UI_Monitor/js/common/AmmeterStatistic.js"></script>
    <script src="/UI_Monitor/js/common/ComprehensiveStatistic.js" charset="utf-8"></script>
    <script>
        publicData.organizationId = "zc_zcshn_zhuzhouc_zhuzhouf";
        publicData.sceneName = "";
    </script>
</head>
<body id="SubMonitorBody" style="margin: 0px; padding: 0px; overflow: auto;">   
    <div id="SubMonitorLayout" style="position: relative; background-image: url('../../images/page/zc_zcshn_zhuzhouc_zhuzhouf.png'); width: 1350px; height: 740px; overflow: hidden;">
       <%--<div onclick="getAllCookie()">cookie测试</div>--%>
        <%--<div onclick="openWindow()">打开趋势窗口</div>--%>
         <table class="mytable" style="position: absolute; top: 90px; left: 27px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>rawMaterialsHomogenize_ElectricityQuantity>Class" class="mchart"></span></td>
            </tr>
        </table>
        <table class="mytable" style="position: absolute; top: 91px; left: 92px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>rawMaterialsHomogenize_ElectricityConsumption>Class" class="mchart"></span></td>
            </tr>
        </table>
        <table class="mytable" style="position: absolute; top: 205px; left: 215px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>clinker_MixtureMaterialsOutput>Class" class="mchart nodisplay"></span></td>
            </tr>
        </table>




        <%-- <table class="mytable" style="position: absolute; top: 613px; left: 86px; width:45px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>rawMaterialsPreparation>Target_Overall" style="width:45px" ></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>clinkerPreparation>Target_Overall"style="width:45px"></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation>Target_Overall"style="width:45px"></span></td>
            </tr>
        </table>
         <table class="mytable" style="position: absolute; top: 612px; left: 139px; width:45px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>rawMaterialsPreparation>Target_Overall" style="width:45px" ></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>clinkerPreparation>Target_Overall"style="width:45px"></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation>Target_Overall"style="width:45px"></span></td>
            </tr>
        </table>
         <table class="mytable" style="position: absolute; top: 611px; left: 192px; width:45px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>rawMaterialsPreparation>Target_Overall" style="width:45px" ></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>clinkerPreparation>Target_Overall"style="width:45px"></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation>Target_Overall"style="width:45px"></span></td>
            </tr>
        </table>--%>




     <table class="mytable" style="position: absolute; top: 620px; left: 276px; width:55px;">
            <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>rawMaterialsPreparation>TargetOverall" style="width:55px" ></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_clinker01>clinkerPreparation>TargetOverall"style="width:55px"></span></td>
            </tr>
          <tr>
                <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation>TargetOverall"style="width:55px"></span></td>
        </tr>
        </table>



          <table class="mytable" style="position: absolute; top: 621px; left: 114px; ">
                    <tr>
                       <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation_TargetMoment>ThreeTarget"></span></td>
            </tr>
                         <tr>
                       <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation_TargetClass>ThreeTarget"></span></td>
            </tr>

              <tr>
                       <td><span id="zc_zcshn_zhuzhouc_zhuzhouf_cementmill01>cementPreparation_TargetDay>ThreeTarget"></span></td>
            </tr>


   
        </table>
    </div>
    <div id="htmlContainer"></div>
</body>
</html>
