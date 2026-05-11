using ITServiceDowlaodAPI_REV02;
using ITServiceDowlaodAPI_REV02.Class;
using ITServiceDowlaodAPI_REV02.Models;
using ITServiceDowlaodAPI_REV02.Models.Setting;

var oBuilder = Host.CreateApplicationBuilder(args);

var oConfig = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

cConfig.oC_SettingConfig = oConfig.GetSection("oSettingConfig").Get<cmlSettings>() ?? new cmlSettings();
cConfig.oC_ApiConfig = oConfig.GetSection("ApiConfig").Get<cmlApiConfig>() ?? new cmlApiConfig();
cConfig.aoC_Tasklist = oConfig.GetSection("TaskList").Get<List<cmlTaskList>>() ?? new List<cmlTaskList>();

cConfig.oC_ConfigDB = oConfig.GetSection("ConnectionConfig_Local").Get<cmlConfigDB>() ?? new cmlConfigDB();
cCS.tCS_HostName = cConfig.C_PRCxGetIpLocal(1);
cCS.tCS_IPLocal = cConfig.C_PRCxGetIpLocal(2);

oBuilder.Services.AddHostedService<Worker>();

var host = oBuilder.Build();
host.Run();