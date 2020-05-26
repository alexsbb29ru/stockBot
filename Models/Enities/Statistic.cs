using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Enities
{
    public class Statistic
    {
        [Key]
        public Guid StatId { get; set; }
        public DateTime StatDate { get; set; }
        
        public Guid UserId { get; set; } 
    }
}