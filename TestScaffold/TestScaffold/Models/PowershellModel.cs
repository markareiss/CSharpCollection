using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Web;

namespace TestScaffold.Models
{
    public class PowershellModel
    {
        public Collection<PSObject> PSResults { get; set; }        
    }
}