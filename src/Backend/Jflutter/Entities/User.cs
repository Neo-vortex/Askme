using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jflutter.Entities.Enums;

namespace Jflutter.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int _id { get; set; }
    public long PersonalCode { get; set; }
    public Rule Rule { get; set; }
    public  string Password { get; set; }
    public  string Firstname { get; set; }
    public  string Lastname { get; set; }
    public  ICollection<Module> Modules { get; set; }
    public  ICollection<Question> Questions { get; set; }
    
}