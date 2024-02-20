using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Signee.Domain.Entities.Display;

public class View
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = string.Empty;
    
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string imgUrl { get; set; } // TEMPORARY SOLUTION FOR MVP
}