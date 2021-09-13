namespace MsCrmTools.MetadataBrowser
{
    partial class MainControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbLoadEntities = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiLoadEntitiesFromSolution = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbEntityColumns = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tslSearch = new System.Windows.Forms.ToolStripLabel();
            this.tstxtFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOpenInWebApp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportExcel = new System.Windows.Forms.ToolStripButton();
            this.entityListView = new System.Windows.Forms.ListView();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tbEntitiesList = new System.Windows.Forms.TabPage();
            this.cmsEntity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiEntityCopyLogicalName = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEntityCopyLogicalCollectionName = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEntityCopySchemaName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.tbEntitiesList.SuspendLayout();
            this.cmsEntity.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.toolStripSeparator1,
            this.tssbLoadEntities,
            this.toolStripSeparator2,
            this.tsbEntityColumns,
            this.toolStripSeparator3,
            this.tslSearch,
            this.tstxtFilter,
            this.toolStripSeparator4,
            this.tsbOpenInWebApp,
            this.toolStripSeparator5,
            this.tsbExportExcel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1394, 46);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(34, 41);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // tssbLoadEntities
            // 
            this.tssbLoadEntities.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLoadEntitiesFromSolution});
            this.tssbLoadEntities.Image = global::MsCrmTools.MetadataBrowser.Properties.Resources.ico_16_9801;
            this.tssbLoadEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbLoadEntities.Name = "tssbLoadEntities";
            this.tssbLoadEntities.Size = new System.Drawing.Size(157, 29);
            this.tssbLoadEntities.Text = "Load Entities";
            this.tssbLoadEntities.ToolTipText = "Load all entities from the connected organization";
            this.tssbLoadEntities.ButtonClick += new System.EventHandler(this.tssbLoadEntities_ButtonClick);
            // 
            // tsmiLoadEntitiesFromSolution
            // 
            this.tsmiLoadEntitiesFromSolution.Name = "tsmiLoadEntitiesFromSolution";
            this.tsmiLoadEntitiesFromSolution.Size = new System.Drawing.Size(345, 34);
            this.tsmiLoadEntitiesFromSolution.Text = "Load Entities from solution(s)";
            this.tsmiLoadEntitiesFromSolution.Click += new System.EventHandler(this.tsmiLoadEntitiesFromSolution_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbEntityColumns
            // 
            this.tsbEntityColumns.Image = ((System.Drawing.Image)(resources.GetObject("tsbEntityColumns.Image")));
            this.tsbEntityColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEntityColumns.Name = "tsbEntityColumns";
            this.tsbEntityColumns.Size = new System.Drawing.Size(122, 29);
            this.tsbEntityColumns.Text = "Columns...";
            this.tsbEntityColumns.Click += new System.EventHandler(this.tsbColumns_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 34);
            // 
            // tslSearch
            // 
            this.tslSearch.Name = "tslSearch";
            this.tslSearch.Size = new System.Drawing.Size(64, 29);
            this.tslSearch.Text = "Search";
            // 
            // tstxtFilter
            // 
            this.tstxtFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstxtFilter.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.tstxtFilter.Name = "tstxtFilter";
            this.tstxtFilter.Size = new System.Drawing.Size(298, 34);
            this.tstxtFilter.Text = "Logical/Display Name or GUID";
            this.tstxtFilter.Enter += new System.EventHandler(this.tstxtFilter_Enter);
            this.tstxtFilter.TextChanged += new System.EventHandler(this.tstxtFilter_TextChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbOpenInWebApp
            // 
            this.tsbOpenInWebApp.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenInWebApp.Image")));
            this.tsbOpenInWebApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenInWebApp.Name = "tsbOpenInWebApp";
            this.tsbOpenInWebApp.Size = new System.Drawing.Size(177, 29);
            this.tsbOpenInWebApp.Text = "Open in web app";
            this.tsbOpenInWebApp.Click += new System.EventHandler(this.tsbOpenInWebApp_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbExportExcel
            // 
            this.tsbExportExcel.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportExcel.Image")));
            this.tsbExportExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportExcel.Name = "tsbExportExcel";
            this.tsbExportExcel.Size = new System.Drawing.Size(156, 29);
            this.tsbExportExcel.Text = "Export to Excel";
            this.tsbExportExcel.Click += new System.EventHandler(this.tsbExportExcel_Click);
            // 
            // entityListView
            // 
            this.entityListView.ContextMenuStrip = this.cmsEntity;
            this.entityListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityListView.FullRowSelect = true;
            this.entityListView.GridLines = true;
            this.entityListView.HideSelection = false;
            this.entityListView.Location = new System.Drawing.Point(3, 3);
            this.entityListView.Name = "entityListView";
            this.entityListView.Size = new System.Drawing.Size(1380, 800);
            this.entityListView.TabIndex = 0;
            this.entityListView.UseCompatibleStateImageBehavior = false;
            this.entityListView.View = System.Windows.Forms.View.Details;
            this.entityListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.entityListView.SelectedIndexChanged += new System.EventHandler(this.entityListView_SelectedIndexChanged);
            this.entityListView.DoubleClick += new System.EventHandler(this.entityListView_DoubleClick);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tbEntitiesList);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 55);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1394, 839);
            this.mainTabControl.TabIndex = 7;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            // 
            // tbEntitiesList
            // 
            this.tbEntitiesList.Controls.Add(this.entityListView);
            this.tbEntitiesList.Location = new System.Drawing.Point(4, 29);
            this.tbEntitiesList.Name = "tbEntitiesList";
            this.tbEntitiesList.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tbEntitiesList.Size = new System.Drawing.Size(1386, 806);
            this.tbEntitiesList.TabIndex = 0;
            this.tbEntitiesList.Text = "Tables";
            this.tbEntitiesList.UseVisualStyleBackColor = true;
            // 
            // cmsEntity
            // 
            this.cmsEntity.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEntityCopyLogicalName,
            this.tsmiEntityCopyLogicalCollectionName,
            this.tsmiEntityCopySchemaName});
            this.cmsEntity.Name = "cmsAttributes";
            this.cmsEntity.Size = new System.Drawing.Size(319, 100);
            this.cmsEntity.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsEntity_ItemClicked);
            // 
            // tsmiEntityCopyLogicalName
            // 
            this.tsmiEntityCopyLogicalName.Name = "tsmiEntityCopyLogicalName";
            this.tsmiEntityCopyLogicalName.Size = new System.Drawing.Size(318, 32);
            this.tsmiEntityCopyLogicalName.Text = "Copy Logical name";
            // 
            // tsmiEntityCopyLogicalCollectionName
            // 
            this.tsmiEntityCopyLogicalCollectionName.Name = "tsmiEntityCopyLogicalCollectionName";
            this.tsmiEntityCopyLogicalCollectionName.Size = new System.Drawing.Size(318, 32);
            this.tsmiEntityCopyLogicalCollectionName.Text = "Copy Logical Collection name";
            // 
            // tsmiEntityCopySchemaName
            // 
            this.tsmiEntityCopySchemaName.Name = "tsmiEntityCopySchemaName";
            this.tsmiEntityCopySchemaName.Size = new System.Drawing.Size(318, 32);
            this.tsmiEntityCopySchemaName.Text = "Copy Schema name";
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(1162, 745);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.tbEntitiesList.ResumeLayout(false);
            this.cmsEntity.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListView entityListView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbEntityColumns;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tbEntitiesList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel tslSearch;
        private System.Windows.Forms.ToolStripTextBox tstxtFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbOpenInWebApp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbExportExcel;
        private System.Windows.Forms.ToolStripSplitButton tssbLoadEntities;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadEntitiesFromSolution;
        private System.Windows.Forms.ContextMenuStrip cmsEntity;
        private System.Windows.Forms.ToolStripMenuItem tsmiEntityCopyLogicalName;
        private System.Windows.Forms.ToolStripMenuItem tsmiEntityCopyLogicalCollectionName;
        private System.Windows.Forms.ToolStripMenuItem tsmiEntityCopySchemaName;
    }
}
