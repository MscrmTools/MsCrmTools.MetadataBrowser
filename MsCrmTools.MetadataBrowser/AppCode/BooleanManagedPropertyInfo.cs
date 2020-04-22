using Microsoft.Xrm.Sdk;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BooleanManagedPropertyInfo
    {
        private readonly BooleanManagedProperty property;

        public BooleanManagedPropertyInfo(BooleanManagedProperty property)
        {
            this.property = property;
        }

        public bool CanBeChanged => property?.CanBeChanged ?? false;

        public string ManagedPropertyLogicalName => property?.ManagedPropertyLogicalName ?? "N/A";

        public bool Value => property?.Value ?? false;

        public override string ToString()
        {
            return string.Format("Value: {0} / Can be changed: {1}", Value, CanBeChanged);
        }
    }
}