using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.OptionSetMd;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class PicklistAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly PicklistAttributeMetadata amd;

        public PicklistAttributeMetadataInfo(PicklistAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public string[] ChildPicklistLogicalNames => amd.ChildPicklistLogicalNames?.ToArray();
        public int DefaultFormValue => amd.DefaultFormValue.Value;

        public string FormulaDefinition => amd.FormulaDefinition;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public OptionSetMetadataInfo OptionSet => new OptionSetMetadataInfo(amd.OptionSet);

        public string ParentPicklistLogicalName => amd.ParentPicklistLogicalName;
        public int SourceTypeMask => amd.SourceTypeMask ?? -1;
    }
}