using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.OptionMd
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OptionMetadataInfo
    {
        private readonly OptionMetadata amd;

        public OptionMetadataInfo(OptionMetadata amd)
        {
            this.amd = amd;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo Description => new LabelInfo(amd.Description);

        public string ExtensionData => amd.ExtensionData?.ToString() ?? "";

        public bool HasChanged => amd.HasChanged.HasValue && amd.HasChanged.Value;

        public bool IsManaged => amd.IsManaged.HasValue && amd.IsManaged.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo Label => new LabelInfo(amd.Label);

        public string MetadataId => amd.MetadataId != null ? amd.MetadataId.Value.ToString() : "";

        public int Value => amd.Value.Value;

        public string State
        {
            get
            {
                var somd = amd as StatusOptionMetadata;
                if (somd != null)
                {
                    return (somd.State ?? -1).ToString();
                }

                return "Not relevant";
            }
        }

        public string DefaultStatus
        {
            get
            {
                var somd = amd as StateOptionMetadata;
                if (somd != null)
                {
                    return (somd.DefaultStatus ?? -1).ToString();
                }

                return "Not relevant";
            }
        }

        public string InvariantName
        {
            get
            {
                var somd = amd as StateOptionMetadata;
                if (somd != null)
                {
                    return somd.InvariantName;
                }

                return "Not relevant";
            }
        }

        public override string ToString()
        {
            return amd.Label.UserLocalizedLabel != null ? amd.Label.UserLocalizedLabel.Label : "N/A";
        }
    }
}