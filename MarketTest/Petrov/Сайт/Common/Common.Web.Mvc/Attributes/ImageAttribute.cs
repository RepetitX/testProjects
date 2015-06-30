using System;
using System.Web.Mvc;

namespace Common.Web.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ImageAttribute : Attribute, IMetadataAware
    {
        public string ImageType { get; set; }

        public bool ShowRemoveButton { get; set; }

        public string RemoveAction { get; set; }

        public string RemoveController { get; set; }

        public ImageAttribute(string imageType)
        {
            ImageType = imageType;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues.Add("ImageType", ImageType);
            metadata.AdditionalValues.Add("ShowRemoveButton", ShowRemoveButton);
            metadata.AdditionalValues.Add("RemoveAction", RemoveAction);
            metadata.AdditionalValues.Add("RemoveController", RemoveController);
        }
    }
}
