using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace MongoDbConnection.BaseEntities
{
    public class Meta
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(null)]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedBy { get; set; }
    }
}