using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Models.Setting
{
    public class cmlConfigDB
    {
        public string? tServerDB { get; set; }
        public string? tNameDB { get; set; }
        public string? tUser { get; set; }
        public string? tPassword { get; set; }
        public bool bIntegratedSecurity { get; set; }
    }
}
