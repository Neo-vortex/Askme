using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jflutter.Entities;

public class Presence
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  int _id { get; set; }
    public long LectureID { get; set; }
    public  User Student { get; set; }
}