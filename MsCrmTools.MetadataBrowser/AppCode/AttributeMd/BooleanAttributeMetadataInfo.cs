using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.BooleanOptionSetMd;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class BooleanAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly BooleanAttributeMetadata amd;

        public BooleanAttributeMetadataInfo(BooleanAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public bool DefaultValue => amd.DefaultValue.HasValue && amd.DefaultValue.Value;

        public string FormulaDefinition => amd.FormulaDefinition;
        public BooleanOptionSetMetadataInfo OptionSet => new BooleanOptionSetMetadataInfo(amd.OptionSet);
    }
}