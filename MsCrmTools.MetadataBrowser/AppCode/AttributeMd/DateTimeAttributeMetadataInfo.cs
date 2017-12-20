using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class DateTimeAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly DateTimeAttributeMetadata amd;

        public DateTimeAttributeMetadataInfo(DateTimeAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public DateTimeFormat Format => amd.Format.Value;

        public ImeMode ImeMode => amd.ImeMode.Value;

        public bool CanChangeDateTimeBehavior => amd.CanChangeDateTimeBehavior.Value;
        public string DateTimeBehavior => amd.DateTimeBehavior.Value;
        public string FormulaDefinition => amd.FormulaDefinition;
    }
}