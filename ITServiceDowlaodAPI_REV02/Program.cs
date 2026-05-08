using ITServiceDowlaodAPI_REV02;
using ITServiceDowlaodAPI_REV02.Class;
using ITServiceDowlaodAPI_REV02.Models;
using ITServiceDowlaodAPI_REV02.Models.Setting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

var builder = Host.CreateApplicationBuilder(args);

// 1. ชี้เป้าให้อ่านไฟล์ appsettings.json
var oConfig = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 2. โหลด Config เข้าคลาส Static โดยตรง
cConfig.oSettingConfig = oConfig.GetSection("oSettingConfig").Get<cmlSettings>() ?? new cmlSettings();
cConfig.oApiConfig = oConfig.GetSection("ApiConfig").Get<cmlApiConfig>() ?? new cmlApiConfig();
cConfig.aoC_Tasklist = oConfig.GetSection("TaskList").Get<List<cmlTaskList>>() ?? new List<cmlTaskList>();

// (ตรงนี้เช็คชื่อคลาสในโปรเจ็กต์คุณให้ดีนะครับ ว่าใช้ cmlConfigDB หรือ cmlConnectionConfig)
// ถ้าไฟล์เดิมของคุณคือ cmlConnectionConfig ให้แก้คำว่า cmlConfigDB ด้านล่างให้ตรงกัน
cConfig.oConfigDB = oConfig.GetSection("ConnectionConfig_Local").Get<cmlConfigDB>() ?? new cmlConfigDB();

// 3. ตั้งค่า IP
cCS.tCS_HostName = cConfig.C_GETtIpLocal(1);
cCS.tCS_IPLocal = cConfig.C_GETtIpLocal(2);

// 4. สั่งรัน Service (จบ ปิ๊ง!)
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();