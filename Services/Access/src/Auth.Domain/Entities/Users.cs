using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Access.Auth.Domain.Entities
{
    [Table("Users")]
    //example: scheme
    //[Table("Users", Schema = "Admin")]
    public class Users
    {
        //[ForeignKey("id")]
        [Key]
        public Guid? id { get; set; }
        // Relación uno a muchos
        //[ForeignKey("enterprise_id")]
        public Guid enterprise_id { get; set; }
        [StringLength(50)]
        public string username { get; set; }
        [Column("protected")]
        [StringLength(128, ErrorMessage = "The Protected field cannot exceed 128 characters.")]
        public string Protected { get; set; }

        //[ForeignKey("token_id")]
        public Guid token_id { get; set; }

        //public enum status { get; set; }
        public bool logged { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Propiedad de navegación inversa
        [JsonIgnore]
        [InverseProperty("Users")]
        public Enterprises? Enterprises { get; set; }

        // Relación uno a uno
        [JsonIgnore]
        //[ForeignKey("token_id")]
        [InverseProperty("Users")]
        public Tokens? Tokens { get; set; }

        // Relación uno a muchos
        [JsonIgnore]
        [InverseProperty("Users")]
        public ICollection<Users_Profiles>? Users_Profiles { get; set; }

    }
    // table link
    [Table("Users_Profiles")]
    //example: scheme
    //[Table("ProtectedData", Schema = "Admin")]
    public class Users_Profiles
    {
        [Key]
        public Guid? id { get; set; }

        //[ForeignKey("user_id")]
        public Guid user_id { get; set; }

        //[ForeignKey("profile_id")]
        public Guid profile_id { get; set; }

        // Use the correct navigation property name within the InverseProperty attribute
        [JsonIgnore]
        [ForeignKey("user_id")]
        [InverseProperty("Users_Profiles")]
        public Users? Users { get; set; }

        // Use the correct navigation property name within the InverseProperty attribute
        [JsonIgnore]
        [ForeignKey("profile_id")]
        [InverseProperty("Users_Profiles")]
        public Profiles? Profiles { get; set; }

        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
    }
}

