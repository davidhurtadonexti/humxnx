using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auth.Domain.Entities
{
    [Table("Modules")]
    //example: scheme
    //[Table("Modules", Schema = "Admin")]
    public class Modules
    {
        [Key]
        public Guid? id { get; set; }
        //[ForeignKey("enterprise_id")]
        public Guid enterprise_id { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(50)]
        public string url { get; set; }
        //public enum status { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Modules")]
        public ICollection<Profiles>? Profiles { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Modules")]
        public ICollection<Menus>? Menus { get; set; }
        // Propiedad de navegación inversa --- valid
        [JsonIgnore]
        [InverseProperty("Modules")]
        public Enterprises? Enterprises { get; set; }
    }
}

