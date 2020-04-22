using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.ManyToManyRelationship
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ManyToManyRelationshipMetadataInfo
    {
        private readonly ManyToManyRelationshipMetadata mtmmd;

        public ManyToManyRelationshipMetadataInfo(ManyToManyRelationshipMetadata mtmmd)
        {
            this.mtmmd = mtmmd;
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AssociatedMenuConfigurationInfo Entity1AssociatedMenuConfiguration => new AssociatedMenuConfigurationInfo(mtmmd.Entity1AssociatedMenuConfiguration);

        public string Entity1IntersectAttribute => mtmmd.Entity1IntersectAttribute;

        public string Entity1LogicalName => mtmmd.Entity1LogicalName;

        public string Entity1NavigationPropertyName => mtmmd.Entity1NavigationPropertyName;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AssociatedMenuConfigurationInfo Entity2AssociatedMenuConfiguration => new AssociatedMenuConfigurationInfo(mtmmd.Entity2AssociatedMenuConfiguration);

        public string Entity2IntersectAttribute => mtmmd.Entity2IntersectAttribute;

        public string Entity2LogicalName => mtmmd.Entity2LogicalName;

        public string Entity2NavigationPropertyName => mtmmd.Entity2NavigationPropertyName;

        public string ExtensionData => mtmmd.ExtensionData?.ToString() ?? "";

        public bool HasChanged => mtmmd.IsValidForAdvancedFind.HasValue && mtmmd.IsValidForAdvancedFind.Value;

        public string IntersectEntityName => mtmmd.IntersectEntityName;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsCustomizable => new BooleanManagedPropertyInfo(mtmmd.IsCustomizable);

        public bool IsCustomRelationship => mtmmd.IsCustomRelationship.HasValue && mtmmd.IsCustomRelationship.Value;

        public bool IsManaged => mtmmd.IsManaged.HasValue && mtmmd.IsManaged.Value;

        public bool IsValidForAdvancedFind => mtmmd.IsValidForAdvancedFind.HasValue && mtmmd.IsValidForAdvancedFind.Value;

        public Guid MetadataId => mtmmd.MetadataId ?? Guid.Empty;

        public RelationshipType RelationshipType => mtmmd.RelationshipType;

        public string SchemaName => mtmmd.SchemaName;

        public SecurityTypes SecurityTypes => mtmmd.SecurityTypes.Value;

        public override string ToString()
        {
            return mtmmd.SchemaName;
        }
    }
}