using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sorter.Models
{
    [Table("Configuration")]
    public class KeyValueModel
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }

        [Required]
        public String Value { get; set; }
        [Required]
        public String Key { get; set; }
    }
}