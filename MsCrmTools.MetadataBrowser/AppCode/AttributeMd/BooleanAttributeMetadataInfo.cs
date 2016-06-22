using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.BooleanOptionSetMd;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    public class BooleanAttributeMetadataInfo : AttributeMetadataInfo
    {
        private readonly BooleanAttributeMetadata amd;

        public BooleanAttributeMetadataInfo(BooleanAttributeMetadata amd)
            : base(amd)
        {
            this.amd = amd;
        }

        public bool DefaultValue
        {
            get { return amd.DefaultValue.HasValue && amd.DefaultValue.Value; }
        }

        public BooleanOptionSetMetadataInfo OptionSet
        {
            get { return new BooleanOptionSetMetadataInfo(amd.OptionSet); }
        }

        public string FormulaDefinition { get { return amd.FormulaDefinition; } }

        // https://msdn.microsoft.com/fr-fr/library/microsoft.xrm.sdk.metadata.picklistattributemetadata.sourcetypemask(v=crm.7).aspx
        //public object T { get { return amd.SourceTypeMask; } }
    }
}