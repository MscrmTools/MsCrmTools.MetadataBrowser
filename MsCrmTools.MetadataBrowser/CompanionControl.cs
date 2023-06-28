using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using MsCrmTools.MetadataBrowser.AppCode;
using MsCrmTools.MetadataBrowser.AppCode.AttributeMd;
using MsCrmTools.MetadataBrowser.AppCode.Keys;
using MsCrmTools.MetadataBrowser.AppCode.ManyToManyRelationship;
using MsCrmTools.MetadataBrowser.AppCode.OneToManyRelationship;
using MsCrmTools.MetadataBrowser.AppCode.OptionMd;
using MsCrmTools.MetadataBrowser.AppCode.OptionSetMd;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MsCrmTools.MetadataBrowser
{
    public partial class CompanionControl : PluginControlBase, ICompanion, IMessageBusHost
    {
        private EntityMetadata[] _emds;

        private Thread searchThread;
        private int? sortColumn;

        public CompanionControl()
        {
            InitializeComponent();

            var tt = new ToolTip();
            tt.SetToolTip(btnClearSearch, "Clear search");
            tt.SetToolTip(btnRefresh, "Refresh metadata from connected environment. It might take a while");
        }

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

        public RightOrLeft GetPosition()
        {
            return RightOrLeft.Right;
        }

        public void OnIncomingMessage(MessageBusEventArgs message)
        {
            if (message.TargetArgument != null)
            {
                txtSearch.Text = message.TargetArgument.ToString();
            }
        }

        public void SearchMetadata(object term)
        {
            Thread.Sleep(300);

            var sTerm = term.ToString();

            if (sTerm.Length == 0)
            {
                Invoke(new Action(() =>
                {
                    lvSearchResult.Items.Clear();
                    scProperties.Visible = false;
                }));
                return;
            }

            var matchingEmds = _emds.Where(e => chkEntities.Checked && e.Matches(sTerm, _emds)).OrderBy(e => e.SchemaName).ToList();
            var matchingAttrs = _emds.SelectMany(e => e.Attributes).Where(e => chkColumns.Checked && e.Matches(sTerm, _emds)).OrderBy(e => e.SchemaName).ToList();
            var matchingOneToManyRels = _emds.SelectMany(e => e.OneToManyRelationships).Where(e => chkRels.Checked && e.Matches(sTerm, _emds)).OrderBy(e => e.SchemaName).ToList();
            var matchingManyToOneRels = _emds.SelectMany(e => e.ManyToOneRelationships).Where(e => chkRels.Checked && e.Matches(sTerm, _emds)).OrderBy(e => e.SchemaName).ToList();
            var matchingManyToManyRels = _emds.SelectMany(e => e.ManyToManyRelationships).Where(e => chkRels.Checked && e.Matches(sTerm, _emds)).OrderBy(e => e.SchemaName).ToList();
            var matchingKeys = _emds.SelectMany(e => e.Keys).Where(e => chkKeys.Checked && (e.Matches(sTerm, _emds) || e.KeyAttributes.Any(k => k.Matches(sTerm)))).OrderBy(e => e.SchemaName).ToList();

            var list = new List<ListViewItem>();
            foreach (var matchingEmd in matchingEmds)
            {
                list.Add(new ListViewItem(matchingEmd.LogicalName) { Tag = matchingEmd, ImageIndex = 0, SubItems = { new ListViewItem.ListViewSubItem { Text = matchingEmd.LogicalName } } }); ;
            }
            foreach (var matchingAttr in matchingAttrs)
            {
                list.Add(new ListViewItem(matchingAttr.LogicalName) { Tag = matchingAttr, ImageIndex = 1, SubItems = { new ListViewItem.ListViewSubItem { Text = matchingAttr.EntityLogicalName } } });
            }
            foreach (var matchingOneToManyRel in matchingOneToManyRels)
            {
                list.Add(new ListViewItem(matchingOneToManyRel.SchemaName) { Tag = matchingOneToManyRel, ImageIndex = 2, SubItems = { new ListViewItem.ListViewSubItem { Text = matchingOneToManyRel.ReferencedEntity } } });
            }
            foreach (var matchingManyToOneRel in matchingManyToOneRels)
            {
                list.Add(new ListViewItem(matchingManyToOneRel.SchemaName) { Tag = matchingManyToOneRel, ImageIndex = 2, SubItems = { new ListViewItem.ListViewSubItem { Text = matchingManyToOneRel.ReferencingEntity } } });
            }
            foreach (var matchingManyToManyRel in matchingManyToManyRels)
            {
                list.Add(new ListViewItem(matchingManyToManyRel.SchemaName) { Tag = matchingManyToManyRel, ImageIndex = 3, SubItems = { new ListViewItem.ListViewSubItem { Text = $"{matchingManyToManyRel.Entity1LogicalName}/{matchingManyToManyRel.Entity2LogicalName}" } } });
            }
            foreach (var matchingKey in matchingKeys)
            {
                list.Add(new ListViewItem(matchingKey.LogicalName) { Tag = matchingKey, ImageIndex = 4, SubItems = { new ListViewItem.ListViewSubItem { Text = matchingKey.EntityLogicalName } } });
            }

            Invoke(new Action(() =>
            {
                lvSearchResult.Items.Clear();
                lvSearchResult.Items.AddRange(list.ToArray());
            }));
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            LoadMetadataCache(false);
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            if (lvSearchResult.Items.Count == 0) return;

            using (var sfd = new SaveFileDialog
            {
                Title = "Choose a file destination",
                Filter = "CSV File (*.csv)|*.csv"
            })
            {
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(sfd.FileName, false, Encoding.Default))
                    {
                        writer.WriteLine(string.Join(",", lvSearchResult.Columns.Cast<ColumnHeader>().Select(ch => ch.Text)));

                        foreach (ListViewItem item in lvSearchResult.Items)
                        {
                            string type = "";
                            switch (item.ImageIndex)
                            {
                                case 0:
                                    type = "Table";
                                    break;

                                case 1:
                                    type = "Column";
                                    break;

                                case 2:
                                    type = "Relationship";
                                    break;

                                case 3:
                                    type = "NN Relationship";
                                    break;

                                case 4:
                                    type = "Key";
                                    break;
                            }

                            writer.WriteLine($"{type},{string.Join(",", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(si => si.Text))}");
                        }
                    }

                    var result = MessageBox.Show(this, $"File saved to {sfd.FileName}\r\n\r\nWould you like to open it now?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        Process.Start($"\"{sfd.FileName}\"");
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMetadataCache(true);
        }

        private void chkEntities_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearch_TextChanged(sender, new EventArgs());
        }

        private void cmsMetadata_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Clipboard.SetText(e.ClickedItem.ToolTipText);
        }

        private void cmsPicklist_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == copyValueToolStripMenuItem)
            {
                Clipboard.SetText(pgOptionSetValues.SelectedGridItem.Label);
            }
            else if (e.ClickedItem == copyDisplayNameToolStripMenuItem)
            {
                Clipboard.SetText(((OptionMetadataInfo)pgOptionSetValues.SelectedGridItem.Value).Label.UserLocalizedLabel.Label);
            }
        }

        private void LoadMetadataCache(bool force)
        {
            SetWorkingState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading metadata...",
                MessageWidth = 300,
                Work = (bw, evt) =>
                {
                    if (force || (ConnectionDetail.MetadataCache?.Length ?? 0) == 0)
                    {
                        ConnectionDetail.UpdateMetadataCache(true).GetAwaiter().GetResult();
                    }
                    _emds = ConnectionDetail.MetadataCacheLoader.GetAwaiter().GetResult().EntityMetadata;
                },
                PostWorkCallBack = evt =>
                {
                    SetWorkingState(false);

                    if (evt.Error != null)
                    {
                        MessageBox.Show(this, "An error occured when loading metadata: " + evt.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
        }

        private void lvSearchResult_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var lv = (ListView)sender;

            lv.Sorting = e.Column == (sortColumn ?? -1) ? (lv.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending) : SortOrder.Ascending;
            lv.ListViewItemSorter = new ListViewItemComparer(e.Column, lv.Sorting);

            sortColumn = e.Column;
        }

        private void lvSearchResult_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) cmsMetadata.Hide();

            var lv = (ListView)sender;

            if (lv.SelectedItems.Count == 0)
            {
                cmsMetadata.Hide();
                return;
            }

            // Hide all menus
            tsmiColumnCopyLogicalName.Visible = false;
            tsmiColumnCopySchemaName.Visible = false;
            tsmiColumnCopyWabApiLookupName.Visible = false;
            tsmiMenuColumn.Visible = false;

            tsmiMenuRel.Visible = false;
            tsmiRelCopyChildNavigation.Visible = false;
            tsmiRelCopyParentNavigation.Visible = false;
            tsmiRelCopyParentNavigationWithBinding.Visible = false;
            tsmiRelCopySchemaName.Visible = false;

            tsmiMenuTable.Visible = false;
            tsmiTableCopyLogicalCollectionName.Visible = false;
            tsmiTableCopyLogicalName.Visible = false;
            tsmiTableCopySchemaName.Visible = false;

            var metadata = lv.SelectedItems[0].Tag;

            // Show menus based on metadata type
            if (metadata is EntityMetadata emd)
            {
                tsmiMenuTable.Visible = true;
                tsmiTableCopyLogicalCollectionName.Visible = true;
                tsmiTableCopyLogicalName.Visible = true;
                tsmiTableCopySchemaName.Visible = true;

                tsmiTableCopyLogicalCollectionName.ToolTipText = emd.LogicalCollectionName;
                tsmiTableCopyLogicalName.ToolTipText = emd.LogicalName;
                tsmiTableCopySchemaName.ToolTipText = emd.SchemaName;
            }
            else if (metadata is AttributeMetadata amd)
            {
                tsmiColumnCopyLogicalName.Visible = true;
                tsmiColumnCopySchemaName.Visible = true;

                tsmiColumnCopyLogicalName.ToolTipText = amd.LogicalName;
                tsmiColumnCopySchemaName.ToolTipText = amd.SchemaName;

                if (metadata is LookupAttributeMetadata lamd)
                {
                    tsmiColumnCopyWabApiLookupName.Visible = true;
                    tsmiColumnCopyWabApiLookupName.ToolTipText = $"_{lamd.LogicalName}_value";
                }
                tsmiMenuColumn.Visible = true;
            }
            else if (metadata is OneToManyRelationshipMetadata || metadata is ManyToManyRelationshipMetadata)
            {
                tsmiMenuRel.Visible = true;
                tsmiRelCopyChildNavigation.Visible = true;
                tsmiRelCopyParentNavigation.Visible = true;
                tsmiRelCopySchemaName.Visible = true;

                if (metadata is OneToManyRelationshipMetadata otmmd)
                {
                    tsmiRelCopyParentNavigationWithBinding.Visible = true;
                    tsmiRelCopyChildNavigation.ToolTipText = otmmd.ReferencedEntityNavigationPropertyName;
                    tsmiRelCopyParentNavigation.ToolTipText = otmmd.ReferencingEntityNavigationPropertyName;
                    tsmiRelCopyParentNavigationWithBinding.ToolTipText = $"{otmmd.ReferencingEntityNavigationPropertyName}@odata.bind";
                    tsmiRelCopySchemaName.ToolTipText = otmmd.SchemaName;
                }
                else if (metadata is ManyToManyRelationshipMetadata mtmmd)
                {
                    tsmiRelCopyChildNavigation.ToolTipText = mtmmd.Entity1NavigationPropertyName;
                    tsmiRelCopyParentNavigation.ToolTipText = mtmmd.Entity2NavigationPropertyName;
                    tsmiRelCopySchemaName.ToolTipText = mtmmd.SchemaName;
                }
            }
        }

        private void lvSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = (ListView)sender;

            if (lv.SelectedItems.Count == 0)
            {
                scProperties.Visible = false;
                return;
            }
            pgOptionSetValues.Visible = false;
            var md = lv.SelectedItems[0].Tag;
            if (md is EntityMetadata emd)
            {
                propertyGrid1.SelectedObject = new EntityMetadataInfo(emd);
            }
            else if (md is AttributeMetadata amd)
            {
                propertyGrid1.SelectedObject = new AttributeMetadataInfo(amd);

                if (amd is PicklistAttributeMetadata plmd)
                {
                    pgOptionSetValues.SelectedObject = new OptionSetMetadataInfo(plmd.OptionSet).Options;
                    pgOptionSetValues.Visible = true;
                }
                else if (amd is MultiSelectPicklistAttributeMetadata mplmd)
                {
                    pgOptionSetValues.SelectedObject = new OptionSetMetadataInfo(mplmd.OptionSet).Options;
                    pgOptionSetValues.Visible = true;
                }
                else if (amd is StateAttributeMetadata smd)
                {
                    pgOptionSetValues.SelectedObject = new OptionSetMetadataInfo(smd.OptionSet).Options;
                    pgOptionSetValues.Visible = true;
                }
                else if (amd is StatusAttributeMetadata smd2)
                {
                    pgOptionSetValues.SelectedObject = new OptionSetMetadataInfo(smd2.OptionSet).Options;
                    pgOptionSetValues.Visible = true;
                }
            }
            else if (md is OneToManyRelationshipMetadata otmd1)
            {
                propertyGrid1.SelectedObject = new OneToManyRelationshipMetadataInfo(otmd1);
            }
            else if (md is ManyToManyRelationshipMetadata mtmmd)
            {
                propertyGrid1.SelectedObject = new ManyToManyRelationshipMetadataInfo(mtmmd);
            }
            else if (md is EntityKeyMetadata ekmd)
            {
                propertyGrid1.SelectedObject = new KeyMetadataInfo(ekmd);
            }

            scProperties.Visible = true;
        }

        private void SetWorkingState(bool working)
        {
            gbSearch.Enabled = !working;
            gbSearchResult.Enabled = !working;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchThread?.Abort();
            searchThread = new Thread(SearchMetadata);
            searchThread.Start(txtSearch.Text);
        }
    }
}