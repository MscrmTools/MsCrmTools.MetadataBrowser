using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class BigIntAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly BigIntAttributeMetadata amd;

        public BigIntAttributeMetadataInfo(BigIntAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public long MaxValue => amd.MaxValue.Value;

        public long MinValue => amd.MinValue.Value;
    }
}