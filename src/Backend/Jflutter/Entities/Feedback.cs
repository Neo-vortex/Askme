using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jflutter.Entities.Enums;
using MLcredibility;

namespace Jflutter.Entities;

public class Feedback
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  int _id { get; set; }
    public  string FeedbackString { get; set; }
    public  User Student { get; set; }
    public  User Teacher { get; set; }
    public  Module Module { get; set; }
    public  Flavour Flavour { get; set; }
    public Credibility Credibility { get; set; }
    public  string StudentInfo { get; set; }
    
}