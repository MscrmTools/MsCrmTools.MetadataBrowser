using Microsoft.Xrm.Sdk.Metadata;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class StringAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly StringAttributeMetadata amd;

        public StringAttributeMetadataInfo(StringAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public string AutoNumberFormat => amd.AutoNumberFormat;
        public int? DataBaseLength => amd.DatabaseLength;
        public StringFormat Format => amd.Format ?? StringFormat.Text;

        public string FormatName => amd.FormatName != null ? amd.FormatName.Value : "";

        public string FormulaDefinition => amd.FormulaDefinition;
        public ImeMode ImeMode => amd.ImeMode ?? ImeMode.Auto;

        public bool IsLocalizable => amd.IsLocalizable.HasValue && amd.IsLocalizable.Value;
        public int MaxLength => amd.MaxLength ?? -1;
        public int SourceTypeMask => amd.SourceTypeMask ?? -1;
        public string YomiOf => amd.YomiOf;
    }
}