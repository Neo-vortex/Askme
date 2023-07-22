using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Jflutter.Entities;

public class Module
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int _id { get; set; }
    public  long ModuleID { get; set; }
    public  string ModuleName { get; set; }
    public string ModuleDescription { get; set; }
    public  ICollection<Lecture> Lectures { get; set; }
    
    public  DateTime CreatedAt { get; set; }
    
    [JsonIgnore]
    public ICollection<User> Students { get; set; } 
}