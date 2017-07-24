using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memefy.Model
{
    public class MemeCaptions
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }

        [JsonProperty(PropertyName = "angerVal")]
        public double AngerVal { get; set; }

        [JsonProperty(PropertyName = "contemptVal")]
        public double ContemptVal { get; set; }

        [JsonProperty(PropertyName = "disgustVal")]
        public double DisgustVal { get; set; }

        [JsonProperty(PropertyName = "fearVal")]
        public double FearVal { get; set; }

        [JsonProperty(PropertyName = "happinessVal")]
        public double HappinessVal { get; set; }

        [JsonProperty(PropertyName = "neutralVal")]
        public double NeutralVal { get; set; }

        [JsonProperty(PropertyName = "sadnessVal")]
        public double SadnessVal { get; set; }

        [JsonProperty(PropertyName = "surpriseVal")]
        public double SurpriseVal { get; set; }
    }
}
