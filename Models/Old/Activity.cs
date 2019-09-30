using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Old
{
    public class Activity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public List<ActivityEntry> ActivityEntries { get; set; }
    }
}