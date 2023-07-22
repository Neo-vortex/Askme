using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jflutter.Entities.Enums;

namespace Jflutter.Entities;

public class InvitationCode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  int _id { get; set; }
    public  long Code { get; set; }
    public  int ValidityCount { get; set; }
    public Rule Rule { get; set; }
}