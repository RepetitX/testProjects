using System;
using System.Web.Mvc;

namespace Common.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class CheckBoxesListAttribute : Attribute, IMetadataAware
    {
        public string SourcePropertyName { get; set; }

        public const string Key = "__checkBoxesListKey";

        public string Css { get; set; }

        public CheckBoxesListAttribute(string sourcePropertyName)
        {
            SourcePropertyName = sourcePropertyName;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[Key] = new Tuple<string, string>(SourcePropertyName, Css);
        }
    }
}
