using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;

namespace Exurb1aBot.Model.ViewModel.WeatherModels {
    public class WeatherModel {
        public string Name { get; set; }
        public WeatherMainModel Main { get; set; }
        public IList<object> Weather { get; set; }
        public WeatherDescriptionModel WeatherDescriptionModel{get;set;}
        public WeatherSysModel Sys { get; set; }
        public WeatherWindModel Wind { get; set; }

        public void FixMapping() {
            var obj = Weather[0] as JObject;
            var x = obj.GetValue("icon");

            WeatherDescriptionModel = new WeatherDescriptionModel() {
                Icon = obj.GetValue("icon").ToString(),
                Description = obj.GetValue("description").ToString(),
            };
        }
    }
}
