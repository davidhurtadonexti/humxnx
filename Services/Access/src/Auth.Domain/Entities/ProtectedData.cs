using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auth.Domain.Entities
{
    [Table("ProtectedData")]
    //example: scheme
    //[Table("ProtectedData", Schema = "Admin")]
    public class ProtectedData
    {
        [Key]
        public Guid? id { get; set; }
        [StringLength(50)]
        public  string tabla_name { get; set; }
        [StringLength(200)]
        public string fields { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("ProtectedData")]
        public ICollection<Profile_ProtectedData>? Profile_ProtectedData { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
    }
    // table link
    [Table("Profile_ProtectedData")]
    //example: scheme
    //[Table("Profiles_Resources", Schema = "Admin")]
    public class Profile_ProtectedData
    {
        [Key]
        public Guid? id { get; set; }

        //[ForeignKey("protected_id")]
        public Guid protected_id { get; set; }
        [JsonIgnore]
        public ProtectedData? ProtectedData { get; set; }

        //[ForeignKey("profile_resource_id")]
        public Guid profile_id { get; set; }
        [JsonIgnore]
        [ForeignKey("profile_id")]
        [InverseProperty("Profile_ProtectedData")]
        public Profiles? Profiles { get; set; }

        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
    }
}

