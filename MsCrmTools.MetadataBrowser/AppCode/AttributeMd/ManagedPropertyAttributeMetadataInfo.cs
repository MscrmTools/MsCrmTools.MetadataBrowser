using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class ManagedPropertyAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly ManagedPropertyAttributeMetadata amd;

        public ManagedPropertyAttributeMetadataInfo(ManagedPropertyAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public string ManagedPropertyLogicalName => amd.ManagedPropertyLogicalName;

        public string ParentAttributeName => amd.ParentAttributeName;

        public int ParentComponentType => amd.ParentComponentType.Value;

        public string ValueAttributeTypeCode => amd.ValueAttributeTypeCode.ToString();
    }
}