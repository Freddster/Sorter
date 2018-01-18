using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sorter.Models
{
    [Table("AnnouncementChannel")]
    public class AnnouncementChannelModel
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }

        [Required]
        public String Value { get; set; }
        [Required]
        public String Guild { get; set; }
    }
}