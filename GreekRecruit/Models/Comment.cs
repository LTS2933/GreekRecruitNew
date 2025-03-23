﻿using System.ComponentModel.DataAnnotations;

namespace GreekRecruit.Models
{
    public class Comment
    {
       [Key]
        public int comment_id { get; set; }
       public int pnm_id { get; set; }
       public string comment_text { get; set; }
       public string comment_type { get; set; }
       public DateTime comment_dt { get; set; }
    }
}
