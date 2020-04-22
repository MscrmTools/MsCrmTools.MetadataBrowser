using Microsoft.Xrm.Sdk.Metadata;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CascadeConfigurationInfo
    {
        private CascadeConfiguration configuration;

        public CascadeConfigurationInfo(CascadeConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public CascadeType Assign => configuration.Assign.HasValue ? configuration.Assign.Value : CascadeType.NoCascade;

        public CascadeType Delete => configuration.Delete.HasValue ? configuration.Delete.Value : CascadeType.NoCascade;

        public CascadeType Merge => configuration.Merge.HasValue ? configuration.Merge.Value : CascadeType.NoCascade;

        public CascadeType Reparent => configuration.Reparent.HasValue ? configuration.Reparent.Value : CascadeType.NoCascade;

        public CascadeType Share => configuration.Share.HasValue ? configuration.Share.Value : CascadeType.NoCascade;

        public CascadeType Unshare => configuration.Unshare.HasValue ? configuration.Unshare.Value : CascadeType.NoCascade;

        public override string ToString()
        {
            return "Expand to see properties";
        }
    }
}