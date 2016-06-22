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

        public bool CanBeChanged
        {
            get { return property?.CanBeChanged ?? false; }
        }

        public string ManagedPropertyLogicalName
        {
            get { return property?.ManagedPropertyLogicalName ?? "N/A"; }
        }

        public bool Value
        { 
            get { return property?.Value ?? false; }
        }

        public override string ToString()
        {
            return string.Format("Value: {0} / Can be changed: {1}", Value, CanBeChanged);
        }
    }
}