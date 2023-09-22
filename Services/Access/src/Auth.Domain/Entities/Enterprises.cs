using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auth.Domain.Entities
{
    [Table("Enterprises")]
    //example: scheme
    //[Table("Enterprises", Schema = "Admin")]
    public class Enterprises
    {
        [Key]
        public Guid? id { get; set; }
        [StringLength(50)]
        public string name { get; set; }
        /// <summary>
        /// Status enterprise (0:active,1:inactive)
        /// </summary>
        [EnumDataType(typeof(EnterpriseStatus))]
        public EnterpriseStatus status { get; set; }
        [StringLength(13)]
        public string identification { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Enterprises")]
        public ICollection<Users>? Users { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Enterprises")]
        public ICollection<Modules>? Modules { get; set; }
    }
    public enum EnterpriseStatus
    {
        /// <summary>
        /// Status enterprise (0:active,1:inactive)
        /// </summary>
        [Display(Name = "active")]
        active,

        /// <summary>
        /// Status enterprise (0:active,1:inactive)
        /// </summary>
        [Display(Name = "inactive")]
        inactive
    }
}

