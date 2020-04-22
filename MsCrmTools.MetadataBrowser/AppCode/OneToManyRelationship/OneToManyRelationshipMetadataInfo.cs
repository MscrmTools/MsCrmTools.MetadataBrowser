using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.OneToManyRelationship
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OneToManyRelationshipMetadataInfo
    {
        private readonly OneToManyRelationshipMetadata otmmd;

        public OneToManyRelationshipMetadataInfo(OneToManyRelationshipMetadata otmmd)
        {
            this.otmmd = otmmd;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AssociatedMenuConfigurationInfo AssociatedMenuConfiguration => new AssociatedMenuConfigurationInfo(otmmd.AssociatedMenuConfiguration);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CascadeConfigurationInfo CascadeConfiguration => new CascadeConfigurationInfo(otmmd.CascadeConfiguration);

        public string ExtensionData => otmmd.ExtensionData != null ? otmmd.ExtensionData.ToString() : "";

        public bool HasChanged => otmmd.HasChanged.HasValue && otmmd.HasChanged.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsCustomizable => new BooleanManagedPropertyInfo(otmmd.IsCustomizable);

        public bool IsCustomRelationship => otmmd.IsCustomRelationship.HasValue && otmmd.IsCustomRelationship.Value;

        public bool IsHierarchical => otmmd.IsHierarchical.HasValue && otmmd.IsHierarchical.Value;

        public bool IsManaged => otmmd.IsManaged.HasValue && otmmd.IsManaged.Value;

        public bool IsValidForAdvancedFind => otmmd.IsValidForAdvancedFind.HasValue && otmmd.IsValidForAdvancedFind.Value;

        public Guid MetadataId => otmmd.MetadataId.Value;

        public string ReferencedAttribute => otmmd.ReferencedAttribute;

        public string ReferencedEntity => otmmd.ReferencedEntity;

        public string ReferencedEntityNavigationPropertyName => otmmd.ReferencedEntityNavigationPropertyName;

        public string ReferencingAttribute => otmmd.ReferencingAttribute;

        public string ReferencingEntity => otmmd.ReferencingEntity;

        public string ReferencingEntityNavigationPropertyName => otmmd.ReferencingEntityNavigationPropertyName;

        public RelationshipType RelationshipType => otmmd.RelationshipType;

        public string SchemaName => otmmd.SchemaName;

        public SecurityTypes SecurityTypes => otmmd.SecurityTypes.Value;

        public override string ToString()
        {
            return otmmd.SchemaName;
        }
    }
}