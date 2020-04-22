using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class MemoAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly MemoAttributeMetadata amd;

        public MemoAttributeMetadataInfo(MemoAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public StringFormat Format => amd.Format.Value;

        public ImeMode ImeMode => amd.ImeMode.Value;

        public bool IsLocalizable => amd.IsLocalizable.HasValue && amd.IsLocalizable.Value;
        public int MaxLength => amd.MaxLength.Value;
    }
}