using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class LookupAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly LookupAttributeMetadata amd;

        public LookupAttributeMetadataInfo(LookupAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public string Format => amd.Format.ToString();

        public string[] Targets => amd.Targets;
    }
}