using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineAssessmentTool.Models
{
    public class Trainer
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int TrainerId { get; set; }

            [Required]
            public int UserId { get; set; } // Foreign key to Users table

            [ForeignKey("UserId")]
            public Users User { get; set; } // Navigation property to Users

            [Required]
            public DateTime JoinedOn { get; set; }

            [Required]
            public int RoleId { get; set; } // Foreign key to Role table

            [ForeignKey("RoleId")]
            public Role Role { get; set; } // Navigation property to Role
        }

    }
