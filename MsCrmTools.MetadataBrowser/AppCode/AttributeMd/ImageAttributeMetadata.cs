using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class ImageAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly ImageAttributeMetadata amd;

        public ImageAttributeMetadataInfo(ImageAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public bool CanStoreFullImage => amd.CanStoreFullImage ?? false;

        public bool IsPrimaryImage => amd.IsPrimaryImage.HasValue && amd.IsPrimaryImage.Value;

        public int MaxHeight => amd.MaxHeight ?? -1;

        public int MaxSizeInKB => amd.MaxSizeInKB ?? -1;

        public int MaxWidth => amd.MaxWidth ?? -1;
    }
}