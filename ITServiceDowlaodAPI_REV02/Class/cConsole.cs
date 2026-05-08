using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cConsole
    {
        public static void C_LogMonitorConfig()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("+++++++++++++++++++++++++++++");
            Console.WriteLine("Application Config");
            Console.WriteLine("  App Mode : " + cConfig.oSettingConfig.tMode);
            Console.WriteLine("  Database : " + cConfig.oConfigDB.tServerDB);
            Console.WriteLine("+++++++++++++++++++++++++++++\n");
            C_LogInfo("Load Config Success");
        }
        public static void C_LogProcess(string ptMsg) 
        { 
            Console.ForegroundColor = ConsoleColor.Green; 
            Console.WriteLine($"Log: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor(); 
        }
        public static void C_LogInfo(string ptMsg) 
        { 
            Console.ForegroundColor = ConsoleColor.DarkCyan; 
            Console.WriteLine($"Info: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor(); 
        }
        public static void C_LogWarning(string ptMsg) 
        { 
            Console.ForegroundColor = ConsoleColor.Yellow; 
            Console.WriteLine($"Warning: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor(); 
        }
        public static void C_LogEvent(string ptMsg) 
        { 
            Console.ForegroundColor = ConsoleColor.Blue; 
            
            Console.WriteLine($"Event: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor(); 
        }
        public static void C_LogError(string ptMsg) 
        { 
            Console.ForegroundColor = ConsoleColor.DarkRed; 
            Console.WriteLine($"Error: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor(); 
        }
    }
}
