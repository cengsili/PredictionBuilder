using System;
using System.Collections.Generic;
using System.Text;

namespace PredictionBuilder
{
    public class SampleFilter
    {
       [PredicateAttribute(PredicateOption.Equal, "Param1")]
        public int? Param1 { get; set; }
        [PredicateAttribute(PredicateOption.Contains, "Param3")]
        public List<int?> Param3 { get; set; }

        [PredicateAttribute(PredicateOption.Equal, "Param2")]
        public int? Param2 { get; set; }

    }
   
}
