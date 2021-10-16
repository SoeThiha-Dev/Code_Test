using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebAPI.Model
{
    public partial class CommonEntity
    {
        [Key]
        public int ID { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool Flag { get; set; }
    }
}
