using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auth.Domain.Entities
{
    [Table("Profiles")]
    //example: scheme
    //[Table("Profiles", Schema = "Admin")]
    public class Profiles
    {
        [Key]
        public Guid? id { get; set; }
        //[ForeignKey("module_id")]
        public  Guid module_id { get; set; }
        [StringLength(50)]
        public string profile_name { get; set; }
        [EnumDataType(typeof(ProfileStatus))]
        public ProfileStatus status { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Profiles")]
        public ICollection<Users_Profiles>? Users_Profiles { get; set; }
        // Propiedad de navegación inversa  --- valid
        [JsonIgnore]
        [InverseProperty("Profiles")]
        public Modules? Modules { get; set; }
        // Propiedad de navegación inversa  --- valid
        [JsonIgnore]
        [InverseProperty("Profiles")]
        public Profile_ProtectedData? Profile_ProtectedData { get; set; }
        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Profiles")]
        public ICollection<Profiles_Resources>? Profiles_Resources { get; set; }
    }
    // table link
    [Table("Profiles_Resources")]
    //example: scheme
    //[Table("Profiles_Resources", Schema = "Admin")]
    public class Profiles_Resources
    {
        [Key]
        public Guid? id { get; set; }

        //[ForeignKey("profile_id")]
        public Guid profile_id { get; set; }
        [JsonIgnore]
        public Profiles? Profiles { get; set; }

        //[ForeignKey("resources_id")]
        public Guid resources_id { get; set; }
        [JsonIgnore]
        public Resources? Resources { get; set; }

        [StringLength(5)]
        public  string access { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
    }
    public enum ProfileStatus
    {
        [Display(Name = "active")]
        active,

        [Display(Name = "inactive")]
        inactive
    }
}

