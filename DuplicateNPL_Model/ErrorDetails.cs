using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateNPL_Model
{
    public class ErrorDetails
    {
        public int ID { get; set; }
        public int? ClientId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string UserMessage { get; set; }
        public string Severity { get; set; }
        public string NotifyTo { get; set; }
        public bool? IsActive { get; set; }
    }
}
