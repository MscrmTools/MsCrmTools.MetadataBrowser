using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode.AttributeMd;
using MsCrmTools.MetadataBrowser.AppCode.Keys;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using MsCrmTools.MetadataBrowser.AppCode.ManyToManyRelationship;
using MsCrmTools.MetadataBrowser.AppCode.OneToManyRelationship;
using MsCrmTools.MetadataBrowser.AppCode.SecurityPrivilege;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    public class EntityMetadataInfo
    {
        private readonly EntityMetadata emd;

        public EntityMetadataInfo(EntityMetadata emd)
        {
            this.emd = emd;
        }

        public int ActivityTypeMask => emd.ActivityTypeMask.Value;

        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(AttributeMetadataCollectionConverter))]
        public AttributeMetadataCollection Attributes
        {
            get
            {
                var collection = new AttributeMetadataCollection();
                if (emd.Attributes == null)
                {
                    return collection;
                }

                foreach (AttributeMetadata rmd in emd.Attributes.OrderBy(r => r.SchemaName))
                {
                    switch (rmd.AttributeType.Value)
                    {
                        case AttributeTypeCode.Boolean:
                            {
                                collection.Add(new BooleanAttributeMetadataInfo((BooleanAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.BigInt:
                            {
                                collection.Add(new BigIntAttributeMetadataInfo((BigIntAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Customer:
                        case AttributeTypeCode.Lookup:
                        case AttributeTypeCode.Owner:
                            {
                                collection.Add(new LookupAttributeMetadataInfo((LookupAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.DateTime:
                            {
                                collection.Add(new DateTimeAttributeMetadataInfo((DateTimeAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Decimal:
                            {
                                collection.Add(new DecimalAttributeMetadataInfo((DecimalAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Double:
                            {
                                collection.Add(new DoubleAttributeMetadataInfo((DoubleAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.EntityName:
                            {
                                collection.Add(new AttributeMetadataInfo(rmd));
                            }
                            break;

                        case AttributeTypeCode.Integer:
                            {
                                collection.Add(new IntegerAttributeMetadataInfo((IntegerAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.ManagedProperty:
                            {
                                collection.Add(
                                    new ManagedPropertyAttributeMetadataInfo((ManagedPropertyAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Memo:
                            {
                                collection.Add(new MemoAttributeMetadataInfo((MemoAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Money:
                            {
                                collection.Add(new MoneyAttributeMetadataInfo((MoneyAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Picklist:
                            {
                                var pamd = rmd as PicklistAttributeMetadata;
                                if (pamd != null)
                                {
                                    collection.Add(new PicklistAttributeMetadataInfo(pamd));
                                }
                            }
                            break;

                        case AttributeTypeCode.State:
                            {
                                collection.Add(new StateAttributeMetadataInfo((StateAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.Status:
                            {
                                collection.Add(new StatusAttributeMetadataInfo((StatusAttributeMetadata)rmd));
                            }
                            break;

                        case AttributeTypeCode.String:
                            {
                                collection.Add(new StringAttributeMetadataInfo((StringAttributeMetadata)rmd));
                            }
                            break;

                        default:
                            {
                                if (rmd is MultiSelectPicklistAttributeMetadata mpamd)
                                {
                                    collection.Add(new MultiSelectPicklistAttributeMetadataInfo(mpamd));
                                }
                                else if (rmd is FileAttributeMetadata famd)
                                {
                                    collection.Add(new FileAttributeMetadataInfo(famd));
                                }
                                else if (rmd is ImageAttributeMetadata iamd)
                                {
                                    collection.Add(new ImageAttributeMetadataInfo(iamd));
                                }
                                else
                                {
                                    collection.Add(new AttributeMetadataInfo(rmd));
                                }
                            }
                            break;
                    }
                }

                return collection;
            }
        }

        public bool AutoCreateAccessTeams => emd.AutoCreateAccessTeams.HasValue && emd.AutoCreateAccessTeams.Value;

        public bool AutoRouteToOwnerQueue => emd.AutoRouteToOwnerQueue.HasValue && emd.AutoRouteToOwnerQueue.Value;

        public bool CanBeInCustomEntityAssociation => emd.CanBeInCustomEntityAssociation.Value;
        public bool CanBeInManyToMany => emd.CanBeInManyToMany.Value;

        public bool CanBePrimaryEntityInRelationship => emd.CanBePrimaryEntityInRelationship.Value;

        public bool CanBeRelatedEntityInRelationship => emd.CanBeRelatedEntityInRelationship.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo CanChangeHierarchicalRelationship => new BooleanManagedPropertyInfo(emd.CanChangeHierarchicalRelationship);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo CanChangeTrackingBeEnabled => new BooleanManagedPropertyInfo(emd.CanChangeTrackingBeEnabled);

        public bool CanCreateAttributes => emd.CanCreateAttributes.Value;

        public bool CanCreateCharts => emd.CanCreateCharts.Value;

        public bool CanCreateForms => emd.CanCreateForms.Value;

        public bool CanCreateViews => emd.CanCreateViews.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo CanEnableSyncToExternalSearchIndex => new BooleanManagedPropertyInfo(emd.CanEnableSyncToExternalSearchIndex);

        public bool CanModifyAdditionalSettings => emd.CanModifyAdditionalSettings.Value;

        public bool CanTriggerWorkflow => emd.CanTriggerWorkflow.HasValue && emd.CanTriggerWorkflow.Value;

        public bool ChangeTrackingEnabled => emd.ChangeTrackingEnabled.HasValue && emd.ChangeTrackingEnabled.Value;

        public string CollectionSchemaName => emd.CollectionSchemaName;

        public Guid? DataProviderId => emd.DataProviderId;
        public Guid? DataSourceId => emd.DataSourceId;

        public int DaysSinceRecordLastModified => emd.DaysSinceRecordLastModified ?? -1;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo Description => new LabelInfo(emd.Description);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo DisplayCollectionName => new LabelInfo(emd.DisplayCollectionName);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LabelInfo DisplayName => new LabelInfo(emd.DisplayName);

        public bool EnforceStateTransitions => emd.EnforceStateTransitions.HasValue && emd.EnforceStateTransitions.Value;
        public Color EntityColor => ColorTranslator.FromHtml(emd.EntityColor);
        public string EntityHelpUrl => emd.EntityHelpUrl;
        public bool EntityHelpUrlEnabled => emd.EntityHelpUrlEnabled.HasValue && emd.EntityHelpUrlEnabled.Value;
        public string EntitySetName => emd.EntitySetName;
        public string ExtensionData => emd.ExtensionData?.ToString() ?? "";
        public string ExternalCollectionName => emd.ExternalCollectionName;
        public bool HasActivities => emd.HasActivities.HasValue && emd.HasActivities.Value;
        public bool HasChanged => emd.HasChanged.HasValue && emd.HasChanged.Value;
        public bool HasFeedback => emd.HasFeedback.HasValue && emd.HasFeedback.Value;
        public bool HasNotes => emd.HasNotes.HasValue && emd.HasNotes.Value;

        public string IconLargeName => emd.IconLargeName;

        public string IconMediumName => emd.IconMediumName;

        public string IconSmallName => emd.IconSmallName;

        public string IconVectorName => emd.IconVectorName;

        public string IntroducedVersion => emd.IntroducedVersion;

        public bool IsActivity => emd.IsActivity.HasValue && emd.IsActivity.Value;
        public bool IsActivityParty => emd.IsActivityParty.HasValue && emd.IsActivityParty.Value;
        public bool IsAIRUpdated => emd.IsAIRUpdated.HasValue && emd.IsAIRUpdated.Value;
        public bool IsAuditEnabled => emd.IsAuditEnabled.Value;

        public bool IsAvailableOffline => emd.IsAvailableOffline.HasValue && emd.IsAvailableOffline.Value;

        public bool IsBPFEntity => emd.IsBPFEntity.HasValue && emd.IsBPFEntity.Value;

        public bool IsBusinessProcessEnabled => emd.IsBusinessProcessEnabled.HasValue && emd.IsBusinessProcessEnabled.Value;

        public bool IsChildEntity => emd.IsChildEntity.HasValue && emd.IsChildEntity.Value;

        public bool IsConnectionsEnabled => emd.IsConnectionsEnabled.Value;

        public bool IsCustomEntity => emd.IsCustomEntity.HasValue && emd.IsCustomEntity.Value;

        public bool IsCustomizable => emd.IsCustomizable.Value;

        public bool IsDocumentManagementEnabled => emd.IsDocumentManagementEnabled.HasValue && emd.IsDocumentManagementEnabled.Value;

        public bool IsDocumentRecommendationsEnabled => emd.IsDocumentRecommendationsEnabled.HasValue && emd.IsDocumentRecommendationsEnabled.Value;

        public bool IsDuplicateDetectionEnabled => emd.IsDuplicateDetectionEnabled.Value;

        public bool IsEnabledForCharts => emd.IsEnabledForCharts.HasValue && emd.IsEnabledForCharts.Value;

        public bool IsEnabledForExternalChannels => emd.IsEnabledForExternalChannels.HasValue && emd.IsEnabledForExternalChannels.Value;
        public bool IsEnabledForTrace => emd.IsEnabledForTrace.HasValue && emd.IsEnabledForTrace.Value;

        public bool IsImportable => emd.IsImportable.HasValue && emd.IsImportable.Value;

        public bool IsInteractionCentricEnabled => emd.IsInteractionCentricEnabled.HasValue && emd.IsInteractionCentricEnabled.Value;

        public bool IsIntersect => emd.IsIntersect.HasValue && emd.IsIntersect.Value;

        public bool IsKnowledgeManagementEnabled => emd.IsKnowledgeManagementEnabled.HasValue && emd.IsKnowledgeManagementEnabled.Value;

        public bool IsLogicalEntity => emd.IsLogicalEntity.HasValue && emd.IsLogicalEntity.Value;

        public bool IsMailMergeEnabled => emd.IsMailMergeEnabled.Value;
        public bool IsManaged => emd.IsManaged.HasValue && emd.IsManaged.Value;
        public bool IsMappable => emd.IsMappable.Value;
        public bool IsMSTeamsIntegrationEnabled => emd.IsMSTeamsIntegrationEnabled ?? false;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsOfflineInMobileClient => new BooleanManagedPropertyInfo(emd.IsOfflineInMobileClient);

        public bool IsOneNoteIntegrationEnabled => emd.IsOneNoteIntegrationEnabled.HasValue && emd.IsOneNoteIntegrationEnabled.Value;
        public bool IsOptimisticConcurrencyEnabled => emd.IsOptimisticConcurrencyEnabled.HasValue && emd.IsOptimisticConcurrencyEnabled.Value;
        public bool IsPrivate => emd.IsPrivate.HasValue && emd.IsPrivate.Value;
        public bool IsQuickCreateEnabled => emd.IsQuickCreateEnabled.HasValue && emd.IsQuickCreateEnabled.Value;

        public bool IsReadingPaneEnabled => emd.IsReadingPaneEnabled.HasValue && emd.IsReadingPaneEnabled.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsReadOnlyInMobileClient => new BooleanManagedPropertyInfo(emd.IsReadOnlyInMobileClient);

        public bool IsRenameable => emd.IsRenameable.Value;

        public bool IsSLAEnabled => emd.IsSLAEnabled.HasValue && emd.IsSLAEnabled.Value;
        public bool IsStateModelAware => emd.IsStateModelAware.HasValue && emd.IsStateModelAware.Value;

        public bool IsValidForAdvancedFind => emd.IsValidForAdvancedFind.HasValue && emd.IsValidForAdvancedFind.Value;

        public bool IsValidForQueue => emd.IsValidForQueue.Value;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsVisibleInMobile => new BooleanManagedPropertyInfo(emd.IsVisibleInMobile);

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BooleanManagedPropertyInfo IsVisibleInMobileClient => new BooleanManagedPropertyInfo(emd.IsVisibleInMobileClient);

        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeyCollectionConverter))]
        public KeyCollection Keys
        {
            get
            {
                var collection = new KeyCollection();
                if (emd.Keys == null)
                {
                    return collection;
                }

                foreach (EntityKeyMetadata ekm in emd.Keys.OrderBy(r => r.SchemaName))
                {
                    collection.Add(new KeyMetadataInfo(ekm));
                }

                return collection;
            }
        }

        public string LogicalCollectionName => emd.LogicalCollectionName;

        public string LogicalName => emd.LogicalName;

        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ManyToManyRelationshipCollectionConverter))]
        public ManyToManyRelationshipCollection ManyToManyRelationships
        {
            get
            {
                var collection = new ManyToManyRelationshipCollection();
                if (emd.ManyToManyRelationships == null)
                {
                    return collection;
                }

                foreach (ManyToManyRelationshipMetadata rmd in emd.ManyToManyRelationships.OrderBy(r => r.SchemaName))
                {
                    collection.Add(new ManyToManyRelationshipMetadataInfo(rmd));
                }

                return collection;
            }
        }

        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(OneToManyRelationshipCollectionConverter))]
        public OneToManyRelationshipCollection ManyToOneRelationships
        {
            get
            {
                var collection = new OneToManyRelationshipCollection();
                if (emd.ManyToOneRelationships == null)
                {
                    return collection;
                }

                foreach (OneToManyRelationshipMetadata rmd in emd.ManyToOneRelationships.OrderBy(r => r.SchemaName))
                {
                    collection.Add(new OneToManyRelationshipMetadataInfo(rmd));
                }

                return collection;
            }
        }

        public Guid MetadataId => emd.MetadataId.Value;

        public string MobileOfflineFilters => emd.MobileOfflineFilters;

        public int ObjectTypeCode => emd.ObjectTypeCode.Value;

        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(OneToManyRelationshipCollectionConverter))]
        public OneToManyRelationshipCollection OneToManyRelationships
        {
            get
            {
                var collection = new OneToManyRelationshipCollection();
                if (emd.OneToManyRelationships == null)
                {
                    return collection;
                }

                foreach (OneToManyRelationshipMetadata rmd in emd.OneToManyRelationships.OrderBy(r => r.SchemaName))
                {
                    collection.Add(new OneToManyRelationshipMetadataInfo(rmd));
                }

                return collection;
            }
        }

        public OwnershipTypes OwnershipType => emd.OwnershipType.Value;

        public string PrimaryIdAttribute => emd.PrimaryIdAttribute;

        public string PrimaryImageAttribute => emd.PrimaryImageAttribute;

        public string PrimaryNameAttribute => emd.PrimaryNameAttribute;

        [Editor(typeof(CustomCollectionEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(SecurityPrivilegeCollectionConverter))]
        public SecurityPrivilegeCollection Privileges
        {
            get
            {
                var collection = new SecurityPrivilegeCollection();
                if (emd.Privileges == null)
                {
                    return collection;
                }

                foreach (SecurityPrivilegeMetadata rmd in emd.Privileges.OrderBy(r => r.Name))
                {
                    collection.Add(new SecurityPrivilegeInfo(rmd));
                }

                return collection;
            }
        }

        public string RecurrenceBaseEntityLogicalName => emd.RecurrenceBaseEntityLogicalName;

        public string ReportViewName => emd.ReportViewName;

        public string SchemaName => emd.SchemaName;

        public bool SyncToExternalSearchIndex => emd.SyncToExternalSearchIndex.HasValue && emd.SyncToExternalSearchIndex.Value;

        public bool UsesBusinessDataLabelTable => emd.UsesBusinessDataLabelTable.HasValue &&
                                                  emd.UsesBusinessDataLabelTable.Value;
    }
}