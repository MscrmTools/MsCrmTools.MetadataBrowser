using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.ComponentModel;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;

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

        public string AsyncJob { get { return ekm.AsyncJob != null ? ekm.AsyncJob.Id.ToString("B") : "N/A"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo DisplayName { get { return new LabelInfo(ekm.DisplayName); } }
        public string EntityKeyIndexStatus { get { return ekm.EntityKeyIndexStatus.ToString(); } }
        public string EntityLogicalName { get { return ekm.EntityLogicalName; } }
        public string IntroducedVersion { get { return ekm.IntroducedVersion; } }
        public bool IsCustomizable { get { return ekm.IsCustomizable.Value; } }
        public bool IsManaged { get { return ekm.IsManaged.HasValue && ekm.IsManaged.Value; } }
        public string[] KeyAttributes { get { return ekm.KeyAttributes; } }
        public string LogicalName { get { return ekm.LogicalName; } }
        public string SchemaName { get { return ekm.SchemaName; } }
        public string MetadataId { get { return ekm.MetadataId.Value.ToString("B"); } }

        public override string ToString()
        {
            return ekm.SchemaName;
        }
    }
}