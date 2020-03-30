using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Google
{
    public class GoogleDriveFiles
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public long? Size { get; set; }
        public long? Version { get; set; }
        public DateTime? CreatedTime { get; set; }

    }
}
