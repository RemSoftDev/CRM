namespace CRM.Report
{
	partial class Report1
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for telerik Reporting designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
			Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
			this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
			this.textBox1 = new Telerik.Reporting.TextBox();
			this.detailSection1 = new Telerik.Reporting.DetailSection();
			this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
			this.textBox2 = new Telerik.Reporting.TextBox();
			this.textBox3 = new Telerik.Reporting.TextBox();
			this.LeadsDataSource_Object = new Telerik.Reporting.ObjectDataSource();
			this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
			this.objectDataSource2 = new Telerik.Reporting.ObjectDataSource();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeaderSection1
			// 
			this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
			this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1});
			this.pageHeaderSection1.Name = "pageHeaderSection1";
			// 
			// textBox1
			// 
			this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.39999997615814209D));
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox1.Value = "Hello world";
			// 
			// detailSection1
			// 
			this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(2D);
			this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox3});
			this.detailSection1.Name = "detailSection1";
			// 
			// pageFooterSection1
			// 
			this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
			this.pageFooterSection1.Name = "pageFooterSection1";
			// 
			// textBox2
			// 
			this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox2.Value = "= Fields.Name";
			// 
			// textBox3
			// 
			this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.2000792026519775D), Telerik.Reporting.Drawing.Unit.Inch(0D));
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox3.Value = "= Fields.Email";
			// 
			// LeadsDataSource_Object
			// 
			this.LeadsDataSource_Object.Name = "LeadsDataSource_Object";
			// 
			// objectDataSource1
			// 
			this.objectDataSource1.Name = "objectDataSource1";
			// 
			// objectDataSource2
			// 
			this.objectDataSource2.DataMember = "GetLeads";
			this.objectDataSource2.DataSource = typeof(CRM.Report.LeadsSource);
			this.objectDataSource2.Name = "objectDataSource2";
			// 
			// Report1
			// 
			this.DataSource = this.objectDataSource2;
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1});
			this.Name = "Report1";
			this.PageSettings.ColumnCount = 1;
			this.PageSettings.ColumnSpacing = Telerik.Reporting.Drawing.Unit.Inch(0D);
			this.PageSettings.Landscape = false;
			this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
			styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
			styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
			styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
			styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
			styleRule2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
			styleRule2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
			this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2});
			this.Width = Telerik.Reporting.Drawing.Unit.Inch(6.5D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion

		private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
		private Telerik.Reporting.TextBox textBox1;
		private Telerik.Reporting.DetailSection detailSection1;
		private Telerik.Reporting.PageFooterSection pageFooterSection1;
		private Telerik.Reporting.TextBox textBox2;
		private Telerik.Reporting.TextBox textBox3;
		private Telerik.Reporting.ObjectDataSource LeadsDataSource_Object;
		private Telerik.Reporting.ObjectDataSource objectDataSource1;
		private Telerik.Reporting.ObjectDataSource objectDataSource2;
	}
}