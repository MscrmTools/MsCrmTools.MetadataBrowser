using Microsoft.Xrm.Sdk;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.LocalizedLabelMd
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LocalizedLabelInfo
    {
        private readonly LocalizedLabel amd;

        public LocalizedLabelInfo(LocalizedLabel amd)
        {
            this.amd = amd;
        }

        public string ExtensionData => amd != null && amd.ExtensionData != null ? amd.ExtensionData.ToString() : "";

        public bool HasChanged => amd != null && amd.HasChanged.HasValue && amd.HasChanged.Value;

        public bool IsManaged => amd != null && amd.IsManaged.HasValue && amd.IsManaged.Value;

        public string Label => amd != null ? amd.Label : "N/A";

        public int LanguageCode => amd != null ? amd.LanguageCode : -1;

        public override string ToString()
        {
            return amd != null ? amd.Label : "N/A";
        }
    }
}