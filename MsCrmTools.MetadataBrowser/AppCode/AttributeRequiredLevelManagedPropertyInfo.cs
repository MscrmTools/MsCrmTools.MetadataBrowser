using Microsoft.Xrm.Sdk.Metadata;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AttributeRequiredLevelManagedPropertyInfo
    {
        private readonly AttributeRequiredLevelManagedProperty property;

        public AttributeRequiredLevelManagedPropertyInfo(AttributeRequiredLevelManagedProperty property)
        {
            this.property = property;
        }

        public bool CanBeChanged => property.CanBeChanged;

        public string ManagedPropertyLogicalName => property.ManagedPropertyLogicalName;

        public AttributeRequiredLevel Value => property.Value;

        public override string ToString()
        {
            return string.Format("Value: {0} / Can be changed: {1}", Value, CanBeChanged);
        }
    }
}