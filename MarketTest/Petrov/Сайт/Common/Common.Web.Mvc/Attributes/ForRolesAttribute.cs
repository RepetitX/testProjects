using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ForRolesAttribute : Attribute, IMetadataAware
    {
        public const string Key = "__forRolesKey";

        public ForRolesAttribute(params string[] roles)
        {
            AlowedRoles = roles.Any() ? roles.ToList() : new List<string>();
        }

        public List<string> AlowedRoles { get; private set; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[Key] = AlowedRoles;
        }
    }
}