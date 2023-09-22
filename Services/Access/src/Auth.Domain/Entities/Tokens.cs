using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auth.Domain.Entities
{
    [Table("Tokens")]
    //example: scheme
    //[Table("Enterprises", Schema = "Admin")]
    public class Tokens
	{
        [Key]
        public Guid? id { get; set; }
        [StringLength(600, ErrorMessage = "The LastName field cannot exceed 600 characters.")]
        public string? access_token { get; set; }
        public string? expiration_token_time { get; set; }
        [StringLength(600, ErrorMessage = "The LastName field cannot exceed 600 characters.")]
        public string? refresh_token { get; set; }
        public string? expiration_refresh_token_time { get; set; }
        /// <summary>
        /// Status Token (0:active,1:inactive)
        /// </summary>
        [EnumDataType(typeof(TokenStatus))]
        public TokenStatus status { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        [JsonIgnore]
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public DateTime? deleted_at { get; set; }
        // Propiedad de navegación inversa
        [JsonIgnore]
        //[ForeignKey("token_id")]
        [InverseProperty("Tokens")]
        public Users? Users { get; set; }
    }
    public enum TokenStatus
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

