using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using MsCrmTools.MetadataBrowser.AppCode;
using MsCrmTools.MetadataBrowser.AppCode.Excel;
using MsCrmTools.MetadataBrowser.AppCode.LabelMd;
using MsCrmTools.MetadataBrowser.Forms;
using MsCrmTools.MetadataBrowser.Helpers;
using MsCrmTools.MetadataBrowser.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MsCrmTools.MetadataBrowser
{
    public partial class MainControl : PluginControlBase, IGitHubPlugin, IHelpPlugin
    {
        private List<EntityMetadata> currentAllMetadata;
        private bool initialized;
        private bool initialLoading = true;
        private ListViewColumnsSettings lvcSettings;
        private Thread searchThread;

        public MainControl()
        {
            InitializeComponent();
            lvcSettings = ListViewColumnsSettings.LoadSettings();

            if (initialLoading)
            {
                // Loads listview header column for entities
                ListViewColumnHelper.AddColumnsHeader(entityListView, typeof(EntityMetadataInfo),
                    ListViewColumnsSettings.EntityFirstColumns, lvcSettings.EntitySelectedAttributes,
                    ListViewColumnsSettings.EntityAttributesToIgnore);

                initialLoading = false;
            }

            this.Enter += MainControl_Enter;
        }

        public string HelpUrl => "https://github.com/MscrmTools/MsCrmTools.MetadataBrowser/wiki";

        public string RepositoryName => "MsCrmTools.MetadataBrowser";

        public string UserName => "MscrmTools";

        public void LoadEntities(bool fromSolution = false)
        {
            List<Entity> solutions = new List<Entity>();

            if (fromSolution)
            {
                var dialog = new SolutionPicker(Service);
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                solutions.AddRange(dialog.SelectedSolutions);
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities...",
                Work = (bw, e) =>
                {
                    // Search for all entities metadata

                    currentAllMetadata = GetEntities(solutions);
                    // return listview items
                    e.Result = BuildEntityItems(currentAllMetadata.ToList());
                },
                PostWorkCallBack = e =>
                {
                    entityListView.Items.Clear();
                    // Add listview items to listview
                    entityListView.Items.AddRange(((List<ListViewItem>)e.Result).ToArray());
                }
            });
        }

        private static bool MatchEntitiesByFilter(ListViewItem item, string filterText)
        {
            // Demystified code for readability, knowing it can be made more compact/efficient -Jonas Rapp
            var entity = (EntityMetadata)item.Tag;
            if (entity.LogicalName.Contains(filterText))
            {
                return true;
            }
            if (entity.DisplayName?.UserLocalizedLabel != null &&
                entity.DisplayName.UserLocalizedLabel.Label.ToLower().Contains(filterText))
            {
                return true;
            }
            if (entity.MetadataId.ToString().ToLower().Contains(filterText))
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
                    else if (value is Color)
                    {
                        var color = (Color)value;
                        item.SubItems.Add(color.Name);
                    }
                    else
                    {
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
                        if (labelInfoValue != null)
                        {
                            item.SubItems.Add(labelInfoValue.UserLocalizedLabel != null
                                ? labelInfoValue.UserLocalizedLabel.Label
                                : "N/A");
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

        private List<ListViewItem> BuildEntityItems(IEnumerable<EntityMetadata> emds)
        {
            if (emds == null) return new List<ListViewItem>();

            var items = new List<ListViewItem>();

            // Stores each property in a listviewitem
            foreach (var metadata in emds.OrderBy(m => m.LogicalName))
            {
                var emd = new EntityMetadataInfo(metadata);

                var item = new ListViewItem(emd.LogicalName) { Tag = metadata };
                item.SubItems.Add(emd.SchemaName);
                item.SubItems.Add(emd.ObjectTypeCode.ToString(CultureInfo.InvariantCulture));
                AddSecondarySubItems(typeof(EntityMetadataInfo), ListViewColumnsSettings.EntityFirstColumns, lvcSettings.EntitySelectedAttributes, emd, item);

                items.Add(item);
            }

            return items;
        }

        private void entityListView_DoubleClick(object sender, EventArgs e)
        {
            ExecuteMethod(LoadEntity);
        }

        private void entityListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsbOpenInWebApp.Enabled = entityListView.SelectedItems.Count > 0;
        }

        private void epc_OnColumnSettingsUpdated(object sender, ColumnSettingsUpdatedEventArgs e)
        {
            lvcSettings = (ListViewColumnsSettings)e.Settings.Clone();
            lvcSettings.SaveSettings();

            foreach (TabPage page in mainTabControl.TabPages)
            {
                if (page.TabIndex == 0) continue;
                var ctrl = ((EntityPropertiesControl)page.Controls[0]);
                if (ctrl.Name != e.Control.Name)
                {
                    ctrl.RefreshColumns(lvcSettings);
                }
            }
        }

        private void FilterEntityList(object filter = null)
        {
            if (currentAllMetadata == null)
            {
                return;
            }

            string filterText = filter?.ToString()?.ToLower();
            if (filter == null)
            {
                return;
            }

            var action = new MethodInvoker(delegate
            {
                entityListView.Items.Clear();
                entityListView.Items.AddRange(
                    BuildEntityItems(currentAllMetadata
                        .ToList()
                        ).Where(item => MatchEntitiesByFilter(item, filterText))
                        .ToArray());
            });

            if (entityListView.InvokeRequired)
            {
                entityListView.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private List<EntityMetadata> GetEntities(List<Entity> solutions)
        {
            if (solutions.Count > 0)
            {
                var components = Service.RetrieveMultiple(new QueryExpression("solutioncomponent")
                {
                    ColumnSet = new ColumnSet("objectid"),
                    NoLock = true,
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("solutionid", ConditionOperator.In,
                                solutions.Select(s => s.Id).ToArray()),
                            new ConditionExpression("componenttype", ConditionOperator.Equal, 1)
                        }
                    }
                }).Entities;

                var list = components.Select(component => component.GetAttributeValue<Guid>("objectid"))
                    .ToList();

                if (list.Count > 0)
                {
                    EntityQueryExpression entityQueryExpression = new EntityQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or),
                        Properties = new MetadataPropertiesExpression
                        {
                            AllProperties = true
                        },
                        AttributeQuery = new AttributeQueryExpression
                        {
                            Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                            {
                                Conditions =
                                {
                                    new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                                }
                            }
                        },
                        KeyQuery = new EntityKeyQueryExpression
                        {
                            Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                            {
                                Conditions =
                                {
                                    new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                                }
                            }
                        },
                        RelationshipQuery = new RelationshipQueryExpression
                        {
                            Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                            {
                                Conditions =
                                {
                                    new MetadataConditionExpression("SchemaName", MetadataConditionOperator.Equals, "filterout"),
                                }
                            }
                        }
                    };

                    list.ForEach(id =>
                    {
                        entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression("MetadataId", MetadataConditionOperator.Equals, id));
                    });

                    RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
                    {
                        Query = entityQueryExpression,
                        ClientVersionStamp = null
                    };

                    var response = (RetrieveMetadataChangesResponse)Service.Execute(retrieveMetadataChangesRequest);

                    return response.EntityMetadata.ToList();
                }

                return new List<EntityMetadata>();
            }

            EntityQueryExpression entityQueryExpression2 = new EntityQueryExpression
            {
                Properties = new MetadataPropertiesExpression
                {
                    AllProperties = true
                },
                AttributeQuery = new AttributeQueryExpression
                {
                    Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                            {
                                new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                            }
                    }
                },
                KeyQuery = new EntityKeyQueryExpression
                {
                    Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                        }
                    }
                },
                RelationshipQuery = new RelationshipQueryExpression
                {
                    Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new MetadataConditionExpression("SchemaName", MetadataConditionOperator.Equals, "filterout"),
                        }
                    }
                }
            };

            RetrieveMetadataChangesRequest retrieveMetadataChangesRequest2 = new RetrieveMetadataChangesRequest
            {
                Query = entityQueryExpression2,
                ClientVersionStamp = null
            };

            var response2 = (RetrieveMetadataChangesResponse)Service.Execute(retrieveMetadataChangesRequest2);

            return response2.EntityMetadata.ToList();
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var list = (ListView)sender;
            list.Sorting = list.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            list.ListViewItemSorter = new ListViewItemComparer(e.Column, list.Sorting);
        }

        private void LoadEntity()
        {
            if (entityListView.SelectedItems.Count == 0)
                return;

            var emd = new EntityMetadataInfo((EntityMetadata)entityListView.SelectedItems[0].Tag);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entity...",
                AsyncArgument = emd,
                Work = (bw, e) =>
                {
                    var request = new RetrieveEntityRequest
                    {
                        EntityFilters = EntityFilters.All,
                        LogicalName = ((EntityMetadataInfo)e.Argument).LogicalName
                    };
                    var response = (RetrieveEntityResponse)Service.Execute(request);
                    e.Result = response.EntityMetadata;
                },
                PostWorkCallBack = e =>
                {
                    var emdFull = (EntityMetadata)e.Result;

                    TabPage tab;
                    if (mainTabControl.TabPages.ContainsKey(emd.SchemaName))
                    {
                        tab = mainTabControl.TabPages[emd.SchemaName];
                        ((EntityPropertiesControl)tab.Controls[0]).RefreshContent(emdFull);
                    }
                    else
                    {
                        mainTabControl.TabPages.Add(emd.SchemaName, emd.SchemaName);
                        tab = mainTabControl.TabPages[emd.SchemaName];

                        var epc = new EntityPropertiesControl(emdFull, lvcSettings, ConnectionDetail)
                        {
                            Dock = DockStyle.Fill,
                            Name = emdFull.SchemaName
                        };
                        epc.OnSelectedTabChanged += (s, evt2) => { mainTabControl_SelectedIndexChanged(mainTabControl, new EventArgs()); };
                        epc.OnColumnSettingsUpdated += epc_OnColumnSettingsUpdated;
                        tab.Controls.Add(epc);
                        mainTabControl.SelectTab(tab);
                    }
                }
            });
        }

        private void MainControl_Enter(object sender, EventArgs e)
        {
            if (sender is MainControl control)
            {
                if (control.Service != null && !initialized)
                {
                    ExecuteMethod(LoadEntities, false);
                    initialized = true;
                }
            }
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            tstxtFilter.Enabled = mainTabControl.SelectedIndex == 0 || ((EntityPropertiesControl)mainTabControl.SelectedTab.Controls[0]).SelectedTabIndex != 1;
            tsbOpenInWebApp.Enabled = mainTabControl.SelectedIndex == 0 || ((EntityPropertiesControl)mainTabControl.SelectedTab.Controls[0]).SelectedTabIndex == 0;
            toolStripSeparator5.Visible = mainTabControl.SelectedIndex == 0;
            tsbExportExcel.Visible = mainTabControl.SelectedIndex == 0;
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbColumns_Click(object sender, EventArgs e)
        {
            switch (((ToolStripButton)sender).Name)
            {
                case "tsbEntityColumns":
                    {
                        var dialog = new ColumnSelector(typeof(EntityMetadataInfo),
                            ListViewColumnsSettings.EntityFirstColumns,
                            ListViewColumnsSettings.EntityAttributesToIgnore,
                            lvcSettings.EntitySelectedAttributes);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            lvcSettings.EntitySelectedAttributes = dialog.UpdatedCurrentAttributes;
                            entityListView.Columns.Clear();
                            entityListView.Items.Clear();

                            ListViewColumnHelper.AddColumnsHeader(entityListView,
                                typeof(EntityMetadataInfo),
                                ListViewColumnsSettings.EntityFirstColumns,
                                lvcSettings.EntitySelectedAttributes,
                                ListViewColumnsSettings.EntityAttributesToIgnore);

                            entityListView.Items.AddRange(BuildEntityItems(currentAllMetadata).ToArray());
                        }
                    }
                    break;

                default:
                    {
                        MessageBox.Show(this, "Unexpected source for hiding panels");
                    }
                    break;
            }

            try
            {
                lvcSettings.SaveSettings();
                foreach (TabPage page in mainTabControl.TabPages)
                {
                    if (page.TabIndex == 0) continue;

                    ((EntityPropertiesControl)page.Controls[0]).RefreshColumns(lvcSettings);
                }
            }
            catch (UnauthorizedAccessException error)
            {
                MessageBox.Show(this, "An error occured while trying to save your settings: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbExportExcel_Click(object sender, EventArgs e)
        {
            if (entityListView.Items.Count == 0) return;

            var sfd = new SaveFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                var builder = new Builder();
                builder.BuildFile(sfd.FileName, entityListView, "Entities", this);
            }
        }

        private void tsbLoadEntities_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadEntities, false);
        }

        private void tsbOpenInWebApp_Click(object sender, EventArgs e)
        {
            if (entityListView.SelectedItems.Count != 1)
            {
                return;
            }

            var emd = (EntityMetadata)entityListView.SelectedItems[0].Tag;
            Process.Start(
                $"{ConnectionDetail.WebApplicationUrl}/tools/systemcustomization/Entities/manageEntity.aspx?appSolutionId=%7bfd140aaf-4df4-11dd-bd17-0019b9312238%7d&entityId=%7b{emd.MetadataId.Value}%7d");
        }

        private void tsmiLoadEntitiesFromSolution_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadEntities, true);
        }

        private void tssbLoadEntities_ButtonClick(object sender, EventArgs e)
        {
            ExecuteMethod(LoadEntities, false);
        }

        private void tstxtFilter_Enter(object sender, EventArgs e)
        {
            if (tstxtFilter.ForeColor == SystemColors.InactiveCaption)
            {
                tstxtFilter.TextChanged -= tstxtFilter_TextChanged;
                tstxtFilter.ForeColor = Color.Black;
                tstxtFilter.Text = string.Empty;
                tstxtFilter.TextChanged += tstxtFilter_TextChanged;
            }

            mainTabControl.SelectedIndex = 0;
        }

        private void tstxtFilter_TextChanged(object sender, EventArgs e)
        {
            if (searchThread != null)
            {
                searchThread.Abort();
            }

            searchThread = new Thread(FilterEntityList);
            searchThread.Start(tstxtFilter.Text);
        }
    }
}