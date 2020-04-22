using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AssociatedMenuConfigurationInfo
    {
        private readonly AssociatedMenuConfiguration configuration;

        public AssociatedMenuConfigurationInfo(AssociatedMenuConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AssociatedMenuBehavior Behavior => configuration.Behavior != null ? configuration.Behavior.Value : AssociatedMenuBehavior.UseCollectionName;

        public AssociatedMenuGroup Group => configuration.Group != null ? configuration.Group.Value : AssociatedMenuGroup.Details;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo Label => new LabelInfo(configuration.Label);

        public int Order => configuration.Order.HasValue ? configuration.Order.Value : -1;

        public override string ToString()
        {
            return "Expand to see properties";
        }
    }
}