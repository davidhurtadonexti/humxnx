using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Access.Auth.Domain.Entities
{
    [Table("Resources")]
    //example: scheme
    //[Table("ProtectedData", Schema = "Admin")]
    public class Resources
    {
        [Key]
        public Guid? id { get; set; }
        //[ForeignKey("menu_id")]
        public Guid menu_id { get; set; }
        [StringLength(50)]
        public string resource { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Propiedad de navegación inversa
        [JsonIgnore]
        [InverseProperty("Resources")]
        public Menus? Menus { get; set; }

        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Resources")]
        public ICollection<Profiles_Resources>? Profiles_Resources { get; set; }
    }
}

