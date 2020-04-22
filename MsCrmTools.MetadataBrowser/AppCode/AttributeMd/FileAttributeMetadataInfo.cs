using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class FileAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly FileAttributeMetadata amd;

        public FileAttributeMetadataInfo(FileAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public int MaxSizeInKB => amd.MaxSizeInKB ?? -1;
    }
}