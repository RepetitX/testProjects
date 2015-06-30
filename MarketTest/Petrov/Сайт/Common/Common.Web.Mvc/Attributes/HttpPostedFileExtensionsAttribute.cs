using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    public class HttpPostedFileExtensionsAttribute : DataTypeAttribute, IClientValidatable
    {
        private readonly FileExtensionsAttribute _innerAttribute = new FileExtensionsAttribute();

        public HttpPostedFileExtensionsAttribute(string extensions)
            : base(DataType.Upload)
        {
            _innerAttribute.Extensions = extensions;
            _innerAttribute.ErrorMessage = null;
            _innerAttribute.ErrorMessageResourceType = null;
            _innerAttribute.ErrorMessageResourceName = null;
        }

        public new string ErrorMessage
        {
            get
            {
                return _innerAttribute.ErrorMessage;
            }
            set
            {
                if (value != null)
                    _innerAttribute.ErrorMessage = value;
            }
        }

        public new Type ErrorMessageResourceType
        {
            get
            {
                return _innerAttribute.ErrorMessageResourceType;
            }
            set
            {
                if (value != null)
                    _innerAttribute.ErrorMessageResourceType = value;
            }
        }

        public new string ErrorMessageResourceName
        {
            get
            {
                return _innerAttribute.ErrorMessageResourceName;
            }
            set
            {
                if (value != null)
                    _innerAttribute.ErrorMessageResourceName = value;
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
                {
                    ValidationType = "accept",
                    ErrorMessage = ErrorMessage
                };

            rule.ValidationParameters["exts"] = _innerAttribute.Extensions;

            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return _innerAttribute.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file != null)
                return _innerAttribute.IsValid(file.FileName);

            var files = value as IEnumerable<HttpPostedFileBase>;

            if (files != null && files.Any(f => f != null))
                return files.Where(f => f != null).All(f => _innerAttribute.IsValid(f.FileName));

            return true;
        }
    }
}