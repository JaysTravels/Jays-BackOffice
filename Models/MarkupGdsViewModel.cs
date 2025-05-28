using Microsoft.AspNetCore.Mvc;
using Jays_BackOffice.DB_Models;

namespace Jays_BackOffice.Models
{
    public class MarkupGdsViewModel
    {
        public int MarkupId { get; set; }
        public string MarkupName { get; set; }
        public int GdsId { get; set; }
        public string GdsName { get; set; }
        public bool IsSelected { get; set; }
    }

  
   
}
