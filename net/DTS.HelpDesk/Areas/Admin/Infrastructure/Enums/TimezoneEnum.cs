using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Enums
{
    public enum TimeZoneEnum
    {
        [Display(Name = "Eastern Standard Time")]
        EasternStandardTime,
        [Display(Name = "Central Standard Time")]
        CentralStandardTime,
        [Display(Name = "Mountain Standard Time")]
        Mountain_Standard_Time,
        [Display(Name = "US Mountain Standard Time")]
        USMountainStandardTime, // Arizona
        [Display(Name = "Pacific Standard Time")]
        PacificStandardTime,
        [Display(Name = "Hawaiian Standard Time")]
        HawaiianStandardTime,
        [Display(Name = "Atlantic Standard Time")]
        AtlanticStandardTime,
        [Display(Name = "West Pacific Standard Time")]
        WestPacificStandardTime, // Guam, Northern Mariana Islands
        [Display(Name = "Samoa Standard Time")]
        SamoaStandardTime // American Samoa



    }
}