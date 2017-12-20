using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class IntegerAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly IntegerAttributeMetadata amd;

        public IntegerAttributeMetadataInfo(IntegerAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public IntegerFormat Format => amd.Format.Value;

        public double MaxValue => amd.MaxValue.Value;

        public double MinValue => amd.MinValue.Value;

        public string FormulaDefinition => amd.FormulaDefinition;
    }
}