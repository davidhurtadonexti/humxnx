using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auth.Domain.Entities
{
    [Table("Menus")]
    //example: scheme
    //[Table("Menus", Schema = "Admin")]
    public class Menus
    {
        [Key]
        public Guid? id { get; set; }
        public Guid parent_id { get; set; }
        //[ForeignKey("module_id")]
        public  Guid module_id { get; set; }

        [StringLength(50)]
        public  string name { get; set; }
        //url varchar(50) NOT NULL DEFAULT '#'
        [StringLength(50)]
        public  string url { get; set; } = "#"; // Adding a default value
        public  int order { get; set; }
        [EnumDataType(typeof(MenuStatus))]
        public MenuStatus status { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Menus")]
        public ICollection<Resources>? Resources { get; set; }
        // Propiedad de navegación inversa --- valid
        [JsonIgnore]
        [InverseProperty("Menus")]
        public Modules? Modules { get; set; }
    }
    public enum MenuStatus
    {
        [Display(Name = "active")]
        active,

        [Display(Name = "inactive")]
        inactive
    }
}

