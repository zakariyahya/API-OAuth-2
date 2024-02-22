using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OAuth2.Models.Base
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedBy = "System";
            CreatedTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        [JsonProperty("createdTime")]
        public DateTime? CreatedTime { get; set; }
        [JsonProperty("lastModifiedBy")]
        public string LastModifiedBy { get; set; } = string.Empty;
        [JsonProperty("lastModifiedTime")]
        public DateTime? LastModifiedTime { get; set; }

    }
}
