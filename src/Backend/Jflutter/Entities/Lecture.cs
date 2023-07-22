using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace Jflutter.Entities;

public class Lecture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  int _id { get; set; }
    public  string LectureMaterial { get; set; }
    public  long SecretCode { get; set; }
    public DateTime LectureDate { get; set; } 
}