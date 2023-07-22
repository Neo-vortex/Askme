using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Jflutter.Entities;

public class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int _id { get; set; }
    public string question { get; set; }
    public Lecture lecture { get; set; }
    public Module Module { get; set; }
    public  ICollection<Answer> answers { get; set; }
    
    [JsonIgnore]
    public ICollection<User> Users { get; set; }
}

