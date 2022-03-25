using System.ComponentModel.DataAnnotations;

namespace identity.Models
{
    public class Enployees
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string lastname { get; set; }
        public string Gender { get; set; }
    }
}
