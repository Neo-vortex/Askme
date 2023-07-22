using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jflutter.Entities;

public class Answer
{
    [Key]   
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  int _id { get; set; }
    public string Answear { get; set; }
}