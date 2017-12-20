using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class DoubleAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly DoubleAttributeMetadata amd;

        public DoubleAttributeMetadataInfo(DoubleAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public ImeMode ImeMode => amd.ImeMode.Value;

        public double MaxValue => amd.MaxValue.Value;

        public double MinValue => amd.MinValue.Value;

        public decimal Precision => amd.Precision.Value;
    }
}