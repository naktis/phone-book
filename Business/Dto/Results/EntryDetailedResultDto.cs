using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto.Results
{
    public class EntryDetailedResultDto : EntryResultDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; }
    }
}
