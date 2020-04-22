using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode;
using MsCrmTools.MetadataBrowser.AppCode.AttributeMd;
using MsCrmTools.MetadataBrowser.AppCode.Excel;
using MsCrmTools.MetadataBrowser.AppCode.Keys;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using MsCrmTools.MetadataBrowser.AppCode.ManyToManyRelationship;
using MsCrmTools.MetadataBrowser.AppCode.OneToManyRelationship;
using MsCrmTools.MetadataBrowser.AppCode.SecurityPrivilege;
using MsCrmTools.MetadataBrowser.Forms;
using MsCrmTools.MetadataBrowser.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MsCrmTools.MetadataBrowser.UserControls
{
    public partial class EntityPropertiesControl : UserControl
    {
        private ConnectionDetail connectionDetail;
        private EntityMetadata emd;
        private ListViewColumnsSettings lvcSettings;
        private Thread searchThread;

        public EntityPropertiesControl(EntityMetadata emd, ListViewColumnsSettings lvcSettings, ConnectionDetail connectionDetail)
        {
            InitializeComponent();

            if (new Version(connectionDetail.OrganizationVersion) < new Version(7, 1))
            {
                // Hide Keys tab if under CRM 2015 Update 1
                tabControl1.TabPages.Remove(tabPage7);
            }

            this.emd = emd;
            this.connectionDetail = connectionDetail;
            this.lvcSettings = (ListViewColumnsSettings)lvcSettings.Clone();

            ListViewColumnHelper.AddColumnsHeader(attributeListView, typeof(AttributeMetadataInfo), ListViewColumnsSettings.AttributeFirstColumns, this.lvcSettings.AttributeSelectedAttributes, new string[] { });
            ListViewColumnHelper.AddColumnsHeader(OneToManyListView, typeof(OneToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, this.lvcSettings.OtmRelSelectedAttributes, new string[] { });
            ListViewColumnHelper.AddColumnsHeader(manyToOneListView, typeof(OneToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, this.lvcSettings.OtmRelSelectedAttributes, new string[] { });
            ListViewColumnHelper.AddColumnsHeader(manyToManyListView, typeof(ManyToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, this.lvcSettings.MtmRelSelectedAttributes, new string[] { });
            ListViewColumnHelper.AddColumnsHeader(privilegeListView, typeof(SecurityPrivilegeInfo), ListViewColumnsSettings.PrivFirstColumns, this.lvcSettings.PrivSelectedAttributes, new string[] { });
            ListViewColumnHelper.AddColumnsHeader(keyListView, typeof(KeyMetadataInfo), ListViewColumnsSettings.KeyFirstColumns, this.lvcSettings.KeySelectedAttributes, new string[] { });

            attributesSplitContainer.Panel2Collapsed = true;
            manyToManySplitContainer.Panel2Collapsed = true;
            manyToOneSplitContainer.Panel2Collapsed = true;
            oneToManySplitContainer.Panel2Collapsed = true;
            privilegeSplitContainer.Panel2Collapsed = true;
            keySplitContainer.Panel2Collapsed = true;

            RefreshContent(emd);
        }

        public event EventHandler<ColumnSettingsUpdatedEventArgs> OnColumnSettingsUpdated;

        public event EventHandler<EventArgs> OnSelectedTabChanged;

        public int SelectedTabIndex => tabControl1.SelectedIndex;

        public void RefreshColumns(ListViewColumnsSettings lvcUpdatedSettings)
        {
            if (lvcSettings.AttributeSelectedAttributes != lvcUpdatedSettings.AttributeSelectedAttributes)
            {
                lvcSettings.AttributeSelectedAttributes = (string[])lvcUpdatedSettings.AttributeSelectedAttributes.Clone();
                attributeListView.Columns.Clear();
                ListViewColumnHelper.AddColumnsHeader(attributeListView, typeof(AttributeMetadataInfo), ListViewColumnsSettings.AttributeFirstColumns, lvcSettings.AttributeSelectedAttributes, new string[] { });
                LoadAttributes(emd.Attributes);
            }

            if (lvcSettings.OtmRelSelectedAttributes != lvcUpdatedSettings.OtmRelSelectedAttributes)
            {
                lvcSettings.OtmRelSelectedAttributes = (string[])lvcUpdatedSettings.OtmRelSelectedAttributes.Clone();
                OneToManyListView.Columns.Clear();
                manyToOneListView.Columns.Clear();
                ListViewColumnHelper.AddColumnsHeader(OneToManyListView, typeof(OneToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, lvcSettings.OtmRelSelectedAttributes, new string[] { });
                ListViewColumnHelper.AddColumnsHeader(manyToOneListView, typeof(OneToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, lvcSettings.OtmRelSelectedAttributes, new string[] { });
                LoadOneToManyRelationships(emd.OneToManyRelationships);
                LoadManyToOneRelationships(emd.ManyToOneRelationships);
            }

            if (lvcSettings.MtmRelSelectedAttributes != lvcUpdatedSettings.MtmRelSelectedAttributes)
            {
                lvcSettings.MtmRelSelectedAttributes = (string[])lvcUpdatedSettings.MtmRelSelectedAttributes.Clone();
                manyToManyListView.Columns.Clear();
                ListViewColumnHelper.AddColumnsHeader(manyToManyListView, typeof(ManyToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, lvcSettings.MtmRelSelectedAttributes, new string[] { });
                LoadManyToManyRelationships(emd.ManyToManyRelationships);
            }

            if (lvcSettings.PrivSelectedAttributes != lvcUpdatedSettings.PrivSelectedAttributes)
            {
                lvcSettings.PrivSelectedAttributes = (string[])lvcUpdatedSettings.PrivSelectedAttributes.Clone();
                privilegeListView.Columns.Clear();
                ListViewColumnHelper.AddColumnsHeader(privilegeListView, typeof(SecurityPrivilegeInfo), ListViewColumnsSettings.PrivFirstColumns, lvcSettings.PrivSelectedAttributes, new string[] { });
                LoadPrivileges(emd.Privileges);
            }

            if (lvcSettings.KeySelectedAttributes != lvcUpdatedSettings.KeySelectedAttributes)
            {
                lvcSettings.KeySelectedAttributes = (string[])lvcUpdatedSettings.KeySelectedAttributes.Clone();
                keyListView.Columns.Clear();
                ListViewColumnHelper.AddColumnsHeader(keyListView, typeof(KeyMetadataInfo), ListViewColumnsSettings.KeyFirstColumns, lvcSettings.KeySelectedAttributes, new string[] { });
                LoadKeys(emd.Keys);
            }

            lvcSettings = lvcUpdatedSettings;
        }

        public void RefreshContent(EntityMetadata newEmd)
        {
            emd = newEmd;
            entityPropertyGrid.SelectedObject = new EntityMetadataInfo(emd);
            LoadAttributes(emd.Attributes);
            LoadOneToManyRelationships(emd.OneToManyRelationships);
            LoadManyToOneRelationships(emd.ManyToOneRelationships);
            LoadManyToManyRelationships(emd.ManyToManyRelationships);
            LoadPrivileges(emd.Privileges);
            LoadKeys(emd.Keys);
        }

        protected virtual void RaiseOnColumnSettingsUpdated(ColumnSettingsUpdatedEventArgs e)
        {
            EventHandler<ColumnSettingsUpdatedEventArgs> handler = OnColumnSettingsUpdated;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private static bool MatchAttributesByFilter(AttributeMetadata attribute, string filter)
        {
            // Demystified code for readability, knowing it can be made more compact/efficient -Jonas Rapp
            if (string.IsNullOrEmpty(filter))
            {
                return true;
            }
            if (attribute.LogicalName.Contains(filter))
            {
                return true;
            }
            if (attribute.DisplayName?.UserLocalizedLabel != null &&
                attribute.DisplayName.UserLocalizedLabel.Label.ToLower().Contains(filter))
            {
                return true;
            }
            if (attribute.MetadataId.ToString().ToLower().Contains(filter))
            {
                return true;
            }
            return false;
        }

        private void AddSecondarySubItems(Type type, string[] firstColumns, string[] selectedAttributes, object o, ListViewItem item)
        {
            if (selectedAttributes == null)
            {
                foreach (var prop in type.GetProperties().OrderBy(p => p.Name))
                {
                    if (firstColumns.Contains(prop.Name))
                        continue;

                    if (ListViewColumnsSettings.EntityAttributesToIgnore.Contains(prop.Name))
                        continue;

                    object value = null;

                    try
                    {
                        value = prop.GetValue(o, null);
                    }
                    catch
                    {
                        //MessageBox.Show(error.ToString());
                    }

                    var labelInfoValue = value as LabelInfo;
                    var managedPropertyInfoValue = value as BooleanManagedPropertyInfo;
                    var cascadeConfigurationInfoValue = value as CascadeConfigurationInfo;
                    var associatedMenuBehaviorInfoValue = value as AssociatedMenuConfigurationInfo;
                    var requiredLevelInfoValue = value as AttributeRequiredLevelManagedPropertyInfo;

                    if (labelInfoValue != null)
                    {
                        item.SubItems.Add(labelInfoValue.UserLocalizedLabel != null
                            ? labelInfoValue.UserLocalizedLabel.Label
                            : "N/A");
                    }
                    else if (managedPropertyInfoValue != null)
                    {
                        item.SubItems.Add(managedPropertyInfoValue.Value.ToString());
                    }
                    else if (requiredLevelInfoValue != null)
                    {
                        item.SubItems.Add(requiredLevelInfoValue.Value.ToString());
                    }
                    else if (cascadeConfigurationInfoValue != null || associatedMenuBehaviorInfoValue != null)
                    {
                        item.SubItems.Add("(Open row to see details)");
                    }
                    else
                    {
                        var stringArrayValue = value as String[];
                        if (stringArrayValue != null)
                        {
                            item.SubItems.Add(string.Join(",", stringArrayValue));
                            continue;
                        }

                        item.SubItems.Add(value == null ? "" : value.ToString());
                    }
                }
            }
            else
            {
                var properties = type.GetProperties();

                foreach (var attr in selectedAttributes)
                {
                    if (firstColumns.Contains(attr))
                        continue;

                    var prop = properties.First(p => p.Name == attr);

                    try
                    {
                        var value = prop.GetValue(o, null);
                        var labelInfoValue = value as LabelInfo;
                        var managedPropertyInfoValue = value as BooleanManagedPropertyInfo;
                        var cascadeConfigurationInfoValue = value as CascadeConfigurationInfo;
                        var associatedMenuBehaviorInfoValue = value as AssociatedMenuConfigurationInfo;
                        var requiredLevelInfoValue = value as AttributeRequiredLevelManagedPropertyInfo;
                        if (labelInfoValue != null)
                        {
                            item.SubItems.Add(labelInfoValue.UserLocalizedLabel != null
                                ? labelInfoValue.UserLocalizedLabel.Label
                                : "N/A");
                        }
                        else if (managedPropertyInfoValue != null)
                        {
                            item.SubItems.Add(managedPropertyInfoValue.Value.ToString());
                        }
                        else if (requiredLevelInfoValue != null)
                        {
                            item.SubItems.Add(requiredLevelInfoValue.Value.ToString());
                        }
                        else if (cascadeConfigurationInfoValue != null || associatedMenuBehaviorInfoValue != null)
                        {
                            item.SubItems.Add("(Open row to see details)");
                        }
                        else
                        {
                            item.SubItems.Add(value == null ? "" : value.ToString());
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void attributeListView_DoubleClick(object sender, EventArgs e)
        {
            if (attributeListView.SelectedItems.Count == 0)
                return;

            var amd = (AttributeMetadataInfo)attributeListView.SelectedItems[0].Tag;
            attributePropertyGrid.SelectedObject = amd;
            attributesSplitContainer.Panel1Collapsed = true;
            attributesSplitContainer.Panel2Collapsed = false;
            tsbHideAttributePanel.Visible = true;
            tsbAttributeColumns.Visible = false;
        }

        private void FilterAttributeList(object filter = null)
        {
            string filterText = filter?.ToString();
            if (filter == null)
            {
                return;
            }

            var action = new MethodInvoker(delegate
            {
                LoadAttributes(emd.Attributes, filterText.ToLower());
            });

            if (attributeListView.InvokeRequired)
            {
                attributeListView.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void keyListView_DoubleClick(object sender, EventArgs e)
        {
            if (keyListView.SelectedItems.Count == 0)
                return;

            var kmi = (KeyMetadataInfo)keyListView.SelectedItems[0].Tag;
            keyPropertyGrid.SelectedObject = kmi;
            keySplitContainer.Panel1Collapsed = true;
            keySplitContainer.Panel2Collapsed = false;
            tsbHideKeyPanel.Visible = true;
            tsbKeyColumns.Visible = false;
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var list = (ListView)sender;
            list.Sorting = list.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            list.ListViewItemSorter = new ListViewItemComparer(e.Column, list.Sorting);
        }

        private void LoadAttributes(IEnumerable<AttributeMetadata> attributes, string filter = null)
        {
            var items = new List<ListViewItem>();

            foreach (var attribute in attributes.ToList().OrderBy(a => a.LogicalName).Where(a => MatchAttributesByFilter(a, filter)
            ))
            {
                var amd = new AttributeMetadataInfo(attribute);
                var item = new ListViewItem(amd.LogicalName);
                item.SubItems.Add(amd.SchemaName);
                item.SubItems.Add(amd.AttributeType.ToString());
                AddSecondarySubItems(typeof(AttributeMetadataInfo), ListViewColumnsSettings.AttributeFirstColumns, lvcSettings.AttributeSelectedAttributes, amd, item);

                switch (attribute.AttributeType.Value)
                {
                    case AttributeTypeCode.Boolean:
                        {
                            item.Tag = new BooleanAttributeMetadataInfo((BooleanAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.BigInt:
                        {
                            item.Tag = new BigIntAttributeMetadataInfo((BigIntAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Lookup:
                    case AttributeTypeCode.Owner:
                        {
                            item.Tag = new LookupAttributeMetadataInfo((LookupAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.DateTime:
                        {
                            item.Tag = new DateTimeAttributeMetadataInfo((DateTimeAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Decimal:
                        {
                            item.Tag = new DecimalAttributeMetadataInfo((DecimalAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Double:
                        {
                            item.Tag = new DoubleAttributeMetadataInfo((DoubleAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.EntityName:
                        {
                            item.Tag = new AttributeMetadataInfo(attribute);
                        }
                        break;

                    case AttributeTypeCode.Integer:
                        {
                            item.Tag = new IntegerAttributeMetadataInfo((IntegerAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.ManagedProperty:
                        {
                            item.Tag =
                                new ManagedPropertyAttributeMetadataInfo((ManagedPropertyAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Memo:
                        {
                            item.Tag = new MemoAttributeMetadataInfo((MemoAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Money:
                        {
                            item.Tag = new MoneyAttributeMetadataInfo((MoneyAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Picklist:
                        {
                            item.Tag = new PicklistAttributeMetadataInfo((PicklistAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.State:
                        {
                            item.Tag = new StateAttributeMetadataInfo((StateAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.Status:
                        {
                            item.Tag = new StatusAttributeMetadataInfo((StatusAttributeMetadata)attribute);
                        }
                        break;

                    case AttributeTypeCode.String:
                        {
                            item.Tag = new StringAttributeMetadataInfo((StringAttributeMetadata)attribute);
                        }
                        break;

                    default:
                        {
                            if (attribute.AttributeTypeName == AttributeTypeDisplayName.ImageType)
                            {
                                item.Tag = new ImageAttributeMetadataInfo((ImageAttributeMetadata)attribute);
                            }
                            else if (attribute.AttributeTypeName == AttributeTypeDisplayName.FileType)
                            {
                                item.Tag = new FileAttributeMetadataInfo((FileAttributeMetadata)attribute);
                            }
                            else if (attribute is MultiSelectPicklistAttributeMetadata mspamd)
                            {
                                item.Tag = new MultiSelectPicklistAttributeMetadataInfo(mspamd);
                            }
                            else
                            {
                                item.Tag = new AttributeMetadataInfo(attribute);
                            }
                        }
                        break;
                }

                items.Add(item);
            }
            attributeListView.Items.Clear();
            attributeListView.Items.AddRange(items.ToArray());
        }

        private void LoadKeys(EntityKeyMetadata[] keys)
        {
            if (keys == null)
            {
                return;
            }

            var items = new List<ListViewItem>();

            foreach (var key in keys.ToList().OrderBy(a => a.SchemaName))
            {
                var kmi = new KeyMetadataInfo(key);

                var item = new ListViewItem(kmi.SchemaName) { Tag = kmi };
                AddSecondarySubItems(typeof(KeyMetadataInfo), ListViewColumnsSettings.KeyFirstColumns, lvcSettings.KeySelectedAttributes, kmi, item);

                items.Add(item);
            }

            keyListView.Items.Clear();
            keyListView.Items.AddRange(items.ToArray());
        }

        private void LoadManyToManyRelationships(IEnumerable<ManyToManyRelationshipMetadata> rels)
        {
            var items = new List<ListViewItem>();

            foreach (var rel in rels.ToList().OrderBy(a => a.Entity2LogicalName))
            {
                var rmd = new ManyToManyRelationshipMetadataInfo(rel);

                var item = new ListViewItem(rmd.SchemaName) { Tag = rmd };
                AddSecondarySubItems(typeof(ManyToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, lvcSettings.MtmRelSelectedAttributes, rmd, item);

                items.Add(item);
            }

            manyToManyListView.Items.Clear();
            manyToManyListView.Items.AddRange(items.ToArray());
        }

        private void LoadManyToOneRelationships(IEnumerable<OneToManyRelationshipMetadata> rels)
        {
            var items = new List<ListViewItem>();

            foreach (var rel in rels.ToList().OrderBy(a => a.ReferencedAttribute))
            {
                var rmd = new OneToManyRelationshipMetadataInfo(rel);

                var item = new ListViewItem(rmd.SchemaName) { Tag = rmd };
                AddSecondarySubItems(typeof(OneToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, lvcSettings.OtmRelSelectedAttributes, rmd, item);

                items.Add(item);
            }

            manyToOneListView.Items.Clear();
            manyToOneListView.Items.AddRange(items.ToArray());
        }

        private void LoadOneToManyRelationships(IEnumerable<OneToManyRelationshipMetadata> rels)
        {
            var items = new List<ListViewItem>();

            foreach (var rel in rels.ToList().OrderBy(a => a.ReferencingEntity))
            {
                var rmd = new OneToManyRelationshipMetadataInfo(rel);

                var item = new ListViewItem(rmd.SchemaName) { Tag = rmd };
                AddSecondarySubItems(typeof(OneToManyRelationshipMetadataInfo), ListViewColumnsSettings.RelFirstColumns, lvcSettings.OtmRelSelectedAttributes, rmd, item);

                items.Add(item);
            }

            OneToManyListView.Items.Clear();
            OneToManyListView.Items.AddRange(items.ToArray());
        }

        private void LoadPrivileges(IEnumerable<SecurityPrivilegeMetadata> privileges)
        {
            var items = new List<ListViewItem>();

            foreach (var privilege in privileges.ToList().OrderBy(a => a.Name))
            {
                var pmd = new SecurityPrivilegeInfo(privilege);

                var item = new ListViewItem(pmd.Name) { Tag = pmd };
                AddSecondarySubItems(typeof(SecurityPrivilegeInfo), ListViewColumnsSettings.PrivFirstColumns, lvcSettings.PrivSelectedAttributes, pmd, item);

                items.Add(item);
            }

            privilegeListView.Items.Clear();
            privilegeListView.Items.AddRange(items.ToArray());
        }

        private void manyToManyListView_DoubleClick(object sender, EventArgs e)
        {
            if (manyToManyListView.SelectedItems.Count == 0)
                return;

            var rmd = (ManyToManyRelationshipMetadataInfo)manyToManyListView.SelectedItems[0].Tag;
            manyToManyPropertyGrid.SelectedObject = rmd;
            manyToManySplitContainer.Panel1Collapsed = true;
            manyToManySplitContainer.Panel2Collapsed = false;
            tsbHideManyToManyPanel.Visible = true;
            tsbManyToManyColumns.Visible = false;
        }

        private void manyToOneListView_DoubleClick(object sender, EventArgs e)
        {
            if (manyToOneListView.SelectedItems.Count == 0)
                return;

            var rmd = (OneToManyRelationshipMetadataInfo)manyToOneListView.SelectedItems[0].Tag;
            manyToOnePropertyGrid.SelectedObject = rmd;
            manyToOneSplitContainer.Panel1Collapsed = true;
            manyToOneSplitContainer.Panel2Collapsed = false;
            tsbHideManyToOnePanel.Visible = true;
            tsbManyToOneColumns.Visible = false;
        }

        private void OneToManyListView_DoubleClick(object sender, EventArgs e)
        {
            if (OneToManyListView.SelectedItems.Count == 0)
                return;

            var rmd = (OneToManyRelationshipMetadataInfo)OneToManyListView.SelectedItems[0].Tag;
            OneToManyPropertyGrid.SelectedObject = rmd;
            oneToManySplitContainer.Panel1Collapsed = true;
            oneToManySplitContainer.Panel2Collapsed = false;
            tsbHideOneToManyPanel.Visible = true;
            tsbOneToManyColumns.Visible = false;
        }

        private void privilegeListView_DoubleClick(object sender, EventArgs e)
        {
            if (privilegeListView.SelectedItems.Count == 0)
                return;

            var rmd = (SecurityPrivilegeInfo)privilegeListView.SelectedItems[0].Tag;
            privilegePropertyGrid.SelectedObject = rmd;
            privilegeSplitContainer.Panel1Collapsed = true;
            privilegeSplitContainer.Panel2Collapsed = false;
            tsbHidePrivilegePanel.Visible = true;
            tsbPrivilegeColumns.Visible = false;
        }

        private void TabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            OnSelectedTabChanged?.Invoke(this, new EventArgs());
        }

        private void tsbColumns_Click(object sender, EventArgs e)
        {
            switch (((ToolStripButton)sender).Name)
            {
                case "tsbAttributeColumns":
                    {
                        var dialog = new ColumnSelector(typeof(AttributeMetadataInfo),
                            ListViewColumnsSettings.AttributeFirstColumns,
                            new string[] { },
                            lvcSettings.AttributeSelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.AttributeSelectedAttributes = dialog.UpdatedCurrentAttributes;
                            attributeListView.Columns.Clear();
                            attributeListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(attributeListView,
                                typeof(AttributeMetadataInfo),
                                ListViewColumnsSettings.AttributeFirstColumns,
                                lvcSettings.AttributeSelectedAttributes,
                                new string[] { });

                            LoadAttributes(emd.Attributes);
                        }
                    }
                    break;

                case "tsbOneToManyColumns":
                    {
                        var dialog = new ColumnSelector(typeof(OneToManyRelationshipMetadataInfo),
                            ListViewColumnsSettings.RelFirstColumns,
                            new string[] { },
                            lvcSettings.OtmRelSelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.OtmRelSelectedAttributes = dialog.UpdatedCurrentAttributes;
                            OneToManyListView.Columns.Clear();
                            OneToManyListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(OneToManyListView,
                                typeof(OneToManyRelationshipMetadataInfo),
                                ListViewColumnsSettings.RelFirstColumns,
                                lvcSettings.OtmRelSelectedAttributes,
                                new string[] { });

                            LoadOneToManyRelationships(emd.OneToManyRelationships);
                        }
                    }
                    break;

                case "tsbManyToOneColumns":
                    {
                        var dialog = new ColumnSelector(typeof(OneToManyRelationshipMetadataInfo),
                            ListViewColumnsSettings.RelFirstColumns,
                            new string[] { },
                            lvcSettings.OtmRelSelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.OtmRelSelectedAttributes = dialog.UpdatedCurrentAttributes;
                            manyToOneListView.Columns.Clear();
                            manyToOneListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(manyToOneListView,
                                typeof(OneToManyRelationshipMetadataInfo),
                                ListViewColumnsSettings.RelFirstColumns,
                                lvcSettings.OtmRelSelectedAttributes,
                                new string[] { });

                            LoadManyToOneRelationships(emd.ManyToOneRelationships);
                        }
                    }
                    break;

                case "tsbManyToManyColumns":
                    {
                        var dialog = new ColumnSelector(typeof(ManyToManyRelationshipMetadataInfo),
                            ListViewColumnsSettings.RelFirstColumns,
                            new string[] { },
                            lvcSettings.MtmRelSelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.MtmRelSelectedAttributes = dialog.UpdatedCurrentAttributes;
                            manyToManyListView.Columns.Clear();
                            manyToManyListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(manyToManyListView,
                                typeof(ManyToManyRelationshipMetadataInfo),
                                ListViewColumnsSettings.RelFirstColumns,
                                lvcSettings.MtmRelSelectedAttributes,
                                new string[] { });

                            LoadManyToManyRelationships(emd.ManyToManyRelationships);
                        }
                    }
                    break;

                case "tsbPrivilegeColumns":
                    {
                        var dialog = new ColumnSelector(typeof(SecurityPrivilegeInfo),
                            ListViewColumnsSettings.PrivFirstColumns,
                            new string[] { },
                            lvcSettings.PrivSelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.PrivSelectedAttributes = dialog.UpdatedCurrentAttributes;
                            privilegeListView.Columns.Clear();
                            privilegeListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(privilegeListView,
                                typeof(SecurityPrivilegeInfo),
                                ListViewColumnsSettings.PrivFirstColumns,
                                lvcSettings.PrivSelectedAttributes,
                                new string[] { });

                            LoadPrivileges(emd.Privileges);
                        }
                    }
                    break;

                case "tsbKeyColumns":
                    {
                        var dialog = new ColumnSelector(typeof(KeyMetadataInfo),
                            ListViewColumnsSettings.KeyFirstColumns,
                            new string[] { },
                            lvcSettings.KeySelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.KeySelectedAttributes = dialog.UpdatedCurrentAttributes;
                            keyListView.Columns.Clear();
                            keyListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(keyListView,
                                typeof(KeyMetadataInfo),
                                ListViewColumnsSettings.KeyFirstColumns,
                                lvcSettings.KeySelectedAttributes,
                                new string[] { });

                            LoadKeys(emd.Keys);
                        }
                    }
                    break;

                default:
                    {
                        MessageBox.Show(this, "Unexpected source for hiding panels");
                    }
                    break;
            }

            RaiseOnColumnSettingsUpdated(new ColumnSettingsUpdatedEventArgs { Settings = lvcSettings, Control = this });
        }

        private void tsbExportAttributesExcel_Click(object sender, EventArgs e)
        {
            if (attributeListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, attributeListView, $"{emd.SchemaName} attributes", this);
            }
        }

        private void tsbExportKeysExcel_Click(object sender, EventArgs e)
        {
            if (keyListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, keyListView, $"{emd.SchemaName} keys", this);
            }
        }

        private void tsbExportMmRelsExcel_Click(object sender, EventArgs e)
        {
            if (manyToManyListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, manyToManyListView, $"{emd.SchemaName} NN relationships", this);
            }
        }

        private void tsbExportMoRelsExcel_Click(object sender, EventArgs e)
        {
            if (manyToOneListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, manyToOneListView, $"{emd.SchemaName} N1 relationships", this);
            }
        }

        private void tsbExportOmRelsExcel_Click(object sender, EventArgs e)
        {
            if (OneToManyListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, OneToManyListView, $"{emd.SchemaName} 1N relationships", this);
            }
        }

        private void tsbExportPrivExcel_Click(object sender, EventArgs e)
        {
            if (privilegeListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, privilegeListView, $"{emd.SchemaName} privileges", this);
            }
        }

        private void tsbHidePanel_Click(object sender, EventArgs e)
        {
            switch (((ToolStripButton)sender).Name)
            {
                case "tsbHideEntityPanel":
                    {
                        var tabPage = (TabPage)Parent;
                        var tabPagesControl = (TabControl)tabPage.Parent;

                        tabPagesControl.TabPages.Remove(tabPage);
                    }
                    break;

                case "tsbHideAttributePanel":
                    {
                        attributesSplitContainer.Panel1Collapsed = false;
                        attributesSplitContainer.Panel2Collapsed = true;
                        tsbHideAttributePanel.Visible = false;
                        tsbAttributeColumns.Visible = true;
                    }
                    break;

                case "tsbHideOneToManyPanel":
                    {
                        oneToManySplitContainer.Panel1Collapsed = false;
                        oneToManySplitContainer.Panel2Collapsed = true;
                        tsbHideOneToManyPanel.Visible = false;
                        tsbOneToManyColumns.Visible = true;
                    }
                    break;

                case "tsbHideManyToOnePanel":
                    {
                        manyToOneSplitContainer.Panel1Collapsed = false;
                        manyToOneSplitContainer.Panel2Collapsed = true;
                        tsbHideManyToOnePanel.Visible = false;
                        tsbManyToOneColumns.Visible = true;
                    }
                    break;

                case "tsbHideManyToManyPanel":
                    {
                        manyToManySplitContainer.Panel1Collapsed = false;
                        manyToManySplitContainer.Panel2Collapsed = true;
                        tsbHideManyToManyPanel.Visible = false;
                        tsbManyToManyColumns.Visible = true;
                    }
                    break;

                case "tsbHidePrivilegePanel":
                    {
                        privilegeSplitContainer.Panel1Collapsed = false;
                        privilegeSplitContainer.Panel2Collapsed = true;
                        tsbHidePrivilegePanel.Visible = false;
                        tsbPrivilegeColumns.Visible = true;
                    }
                    break;

                case "tsbHideKeyPanel":
                    {
                        keySplitContainer.Panel1Collapsed = false;
                        keySplitContainer.Panel2Collapsed = true;
                        tsbHideKeyPanel.Visible = false;
                        tsbKeyColumns.Visible = true;
                    }
                    break;

                default:
                    {
                        MessageBox.Show(this, "Unexpected source for hiding panels");
                    }
                    break;
            }
        }

        private void tsbOpenInWebApp_Click(object sender, EventArgs e)
        {
            if (attributeListView.SelectedItems.Count != 1)
            {
                return;
            }

            var amd = (AttributeMetadataInfo)attributeListView.SelectedItems[0].Tag;

            Process.Start(
              $"{connectionDetail.WebApplicationUrl}/tools/systemcustomization/attributes/manageAttribute.aspx?appSolutionId=%7bfd140aaf-4df4-11dd-bd17-0019b9312238%7d&attributeId={amd.MetadataId}&entityId={emd.MetadataId.Value}");
        }

        private void tstxtSearch_Enter(object sender, EventArgs e)
        {
            var textBox = (ToolStripTextBox)sender;
            if (textBox.ForeColor == SystemColors.InactiveCaption)
            {
                textBox.TextChanged -= tstxtSearch_TextChanged;
                textBox.ForeColor = Color.Black;
                textBox.Text = string.Empty;
                textBox.TextChanged += tstxtSearch_TextChanged;
            }
        }

        private void tstxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (searchThread != null)
            {
                searchThread.Abort();
            }

            if (sender == tstxtSearchContact)
            {
                searchThread = new Thread(FilterAttributeList);
                searchThread.Start(((ToolStripTextBox)sender).Text);
            }
        }
    }
}