using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.AttributeMd
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AttributeMetadataInfo
    {
        private readonly AttributeMetadata amd;

        public AttributeMetadataInfo(AttributeMetadata amd)
        {
            this.amd = amd;
        }

        public string AttributeOf => amd.AttributeOf;

        public AttributeTypeCode AttributeType
        {
            get
            {
                if (amd is MultiSelectPicklistAttributeMetadata)
                {
                    return AttributeTypeCode.Picklist;
                }
                return amd.AttributeType.Value;
            }
        }

        public string AttributeTypeName
        {
            get
            {
                if (amd is MultiSelectPicklistAttributeMetadata)
                {
                    return "MultiSelect Picklist";
                }
                return amd.AttributeTypeName != null ? amd.AttributeTypeName.Value : "";
            }
        }

        public bool CanBeSecuredForCreate => amd.CanBeSecuredForCreate.HasValue && amd.CanBeSecuredForCreate.Value;

        public bool CanBeSecuredForRead => amd.CanBeSecuredForRead.HasValue && amd.CanBeSecuredForRead.Value;

        public bool CanBeSecuredForUpdate => amd.CanBeSecuredForUpdate.HasValue && amd.CanBeSecuredForUpdate.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo CanModifyAdditionalSettings => new BooleanManagedPropertyInfo(amd.CanModifyAdditionalSettings);

        public int ColumnNumber => amd.ColumnNumber.Value;

        public string DeprecatedVersion => amd.DeprecatedVersion;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo Description => new LabelInfo(amd.Description);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo DisplayName => new LabelInfo(amd.DisplayName);

        public string EntityLogicalName => amd.EntityLogicalName;

        public string ExtensionData => amd.ExtensionData.ToString();

        public bool HasChanged => amd.HasChanged.HasValue && amd.HasChanged.Value;

        public string InheritsFrom => amd.InheritsFrom;

        public string IntroducedVersion => amd.IntroducedVersion;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsAuditEnabled => new BooleanManagedPropertyInfo(amd.IsAuditEnabled);

        public bool IsCustomAttribute => amd.IsCustomAttribute.HasValue && amd.IsCustomAttribute.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsCustomizable => new BooleanManagedPropertyInfo(amd.IsCustomizable);

        public bool IsDataSourceSecret => amd.IsDataSourceSecret.HasValue && amd.IsDataSourceSecret.Value;
        public bool IsFilterable => amd.IsFilterable.HasValue && amd.IsFilterable.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsGlobalFilterEnabled => new BooleanManagedPropertyInfo(amd.IsGlobalFilterEnabled);

        public bool IsLogical => amd.IsLogical.HasValue && amd.IsLogical.Value;

        public bool IsManaged => amd.IsManaged.HasValue && amd.IsManaged.Value;

        public bool IsPrimaryId => amd.IsPrimaryId.HasValue && amd.IsPrimaryId.Value;

        public bool IsPrimaryName => amd.IsPrimaryName.HasValue && amd.IsPrimaryName.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsRenameable => new BooleanManagedPropertyInfo(amd.IsRenameable);

        public bool IsRequiredForForm => amd.IsRequiredForForm.HasValue && amd.IsRequiredForForm.Value;

        public bool IsRetrievable => amd.IsRetrievable.HasValue && amd.IsRetrievable.Value;

        public bool IsSearchable => amd.IsSearchable.HasValue && amd.IsSearchable.Value;

        public bool IsSecured => amd.IsSecured.HasValue && amd.IsSecured.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsSortableEnabled => new BooleanManagedPropertyInfo(amd.IsSortableEnabled);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsValidForAdvancedFind => new BooleanManagedPropertyInfo(amd.IsValidForAdvancedFind);

        public bool IsValidForCreate => amd.IsValidForCreate.HasValue && amd.IsValidForCreate.Value;

        public bool IsValidForForm => amd.IsValidForForm.HasValue && amd.IsValidForForm.Value;
        public bool IsValidForGrid => amd.IsValidForGrid.HasValue && amd.IsValidForGrid.Value;
        public bool IsValidForRead => amd.IsValidForRead.HasValue && amd.IsValidForRead.Value;

        public bool IsValidForUpdate => amd.IsValidForUpdate.HasValue && amd.IsValidForUpdate.Value;

        public string LinkedAttributeId => amd.LinkedAttributeId.HasValue ? amd.LinkedAttributeId.Value.ToString("B") : "";

        public string LogicalName => amd.LogicalName;

        public string MetadataId => amd.MetadataId.HasValue ? amd.MetadataId.Value.ToString("B") : "";

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AttributeRequiredLevelManagedPropertyInfo RequiredLevel => new AttributeRequiredLevelManagedPropertyInfo(amd.RequiredLevel);

        public string SchemaName => amd.SchemaName;

        public int SourceType => amd.SourceType.HasValue ? amd.SourceType.Value : -1;

        public override string ToString()
        {
            if (amd is MultiSelectPicklistAttributeMetadata)
            {
                return "MultiSelect Picklist";
            }
            return amd.AttributeType.Value.ToString();
        }
    }
}