using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using DBInstaller.Interface;

namespace DBInstallerGUI
{
    class ConfigurationSetupImport 
    {
        [Import(typeof(IDBInstaller))]
        public IDBInstaller ReaderUIInstDB { get; set; }
    }
}
