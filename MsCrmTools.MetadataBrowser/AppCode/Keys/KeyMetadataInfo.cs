using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.Keys
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class KeyMetadataInfo
    {
        private readonly EntityKeyMetadata ekm;

        public KeyMetadataInfo(EntityKeyMetadata ekm)
        {
            this.ekm = ekm;
        }

        public string AsyncJob => ekm.AsyncJob != null ? ekm.AsyncJob.Id.ToString("B") : "N/A";

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo DisplayName => new LabelInfo(ekm.DisplayName);

        public string EntityKeyIndexStatus => ekm.EntityKeyIndexStatus.ToString();
        public string EntityLogicalName => ekm.EntityLogicalName;
        public string IntroducedVersion => ekm.IntroducedVersion;
        public bool IsCustomizable => ekm.IsCustomizable.Value;
        public bool IsManaged => ekm.IsManaged.HasValue && ekm.IsManaged.Value;
        public bool IsSynchronous => ekm.IsSynchronous.HasValue && ekm.IsSynchronous.Value;
        public string[] KeyAttributes => ekm.KeyAttributes;
        public string LogicalName => ekm.LogicalName;
        public string MetadataId => ekm.MetadataId.Value.ToString("B");
        public string SchemaName => ekm.SchemaName;

        public override string ToString()
        {
            return ekm.SchemaName;
        }
    }
}