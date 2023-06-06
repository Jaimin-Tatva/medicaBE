using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Models
{
    public class DatabaseSettings
    {
        public Dictionary<string, string> CollectionNames { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; }
    }
}
