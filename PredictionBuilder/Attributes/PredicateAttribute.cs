using System;
using System.Collections.Generic;
using System.Text;

namespace PredictionBuilder
{
    public class PredicateAttribute : Attribute
    {
        private PredicateOption option;
        private string attributeName;
        public virtual PredicateOption Option
        {
            get { return option; }
        }
        public virtual string AttributeName
        {
            get { return attributeName; }
        }
        public PredicateAttribute(PredicateOption option, string attributeName)
        {
            this.option = option;
            this.attributeName = attributeName;
        }
        public PredicateAttribute()
        {
            this.option = PredicateOption.Equal;
            this.attributeName = string.Empty;
        }
    }
    public enum PredicateOption
    {
        Equal,
        NotEqual,
        Contains,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual

    }
}
