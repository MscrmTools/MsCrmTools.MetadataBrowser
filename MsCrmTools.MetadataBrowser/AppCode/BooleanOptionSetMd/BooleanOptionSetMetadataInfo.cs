using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using MsCrmTools.MetadataBrowser.AppCode.OptionMd;
using System;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.BooleanOptionSetMd
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BooleanOptionSetMetadataInfo
    {
        private readonly BooleanOptionSetMetadata amd;

        public BooleanOptionSetMetadataInfo(BooleanOptionSetMetadata amd)
        {
            this.amd = amd;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo Description => new LabelInfo(amd.Description);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo DisplayName => new LabelInfo(amd.DisplayName);

        public string ExtensionData => amd.ExtensionData != null ? amd.ExtensionData.ToString() : "";

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public OptionMetadataInfo FalseOption => new OptionMetadataInfo(amd.FalseOption);

        public string IntroducedVersion => amd.IntroducedVersion;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsCustomizable => new BooleanManagedPropertyInfo(amd.IsCustomizable);

        public bool IsCustomOptionSet => amd.IsCustomOptionSet.HasValue && amd.IsCustomOptionSet.Value;

        public bool IsGlobal => amd.IsGlobal.HasValue && amd.IsGlobal.Value;

        public bool IsManaged => amd.IsManaged.HasValue && amd.IsManaged.Value;

        public Guid MetadataId => amd.MetadataId.Value;

        public string Name => amd.Name;

        public OptionSetType OptionSetType => amd.OptionSetType.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public OptionMetadataInfo TrueOption => new OptionMetadataInfo(amd.TrueOption);

        public override string ToString()
        {
            return amd.Name;
        }
    }
}