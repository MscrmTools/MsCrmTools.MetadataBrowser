using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class DecimalAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly DecimalAttributeMetadata amd;

        public DecimalAttributeMetadataInfo(DecimalAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public string FormulaDefinition => amd.FormulaDefinition;
        public ImeMode ImeMode => amd.ImeMode.Value;

        public decimal MaxValue => amd.MaxValue.Value;

        public decimal MinValue => amd.MinValue.Value;

        public decimal Precision => amd.Precision.Value;
        public int SourceTypeMask => amd.SourceTypeMask ?? -1;
    }
}