namespace CRM.Report
{
	partial class LeadsReport
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
			this.textBox4 = new Telerik.Reporting.TextBox();
			this.textBox5 = new Telerik.Reporting.TextBox();
			this.detailSection1 = new Telerik.Reporting.DetailSection();
			this.textBox2 = new Telerik.Reporting.TextBox();
			this.textBox3 = new Telerik.Reporting.TextBox();
			this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
			this.LeadSource = new Telerik.Reporting.ObjectDataSource();
			this.textBox6 = new Telerik.Reporting.TextBox();
			this.textBox7 = new Telerik.Reporting.TextBox();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// pageHeaderSection1
			// 
			this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.0999999046325684D);
			this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox4,
            this.textBox5,
            this.textBox6});
			this.pageHeaderSection1.Name = "pageHeaderSection1";
			// 
			// textBox1
			// 
			this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.49999994039535522D));
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox1.Value = "Leads Report";
			// 
			// textBox4
			// 
			this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.79992127418518066D));
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox4.Value = "Id";
			// 
			// textBox5
			// 
			this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.2000792026519775D), Telerik.Reporting.Drawing.Unit.Inch(0.79992127418518066D));
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox5.Value = "Name";
			// 
			// detailSection1
			// 
			this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.29999998211860657D);
			this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox3,
            this.textBox7});
			this.detailSection1.Name = "detailSection1";
			// 
			// textBox2
			// 
			this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox2.Value = "= Fields.Id";
			// 
			// textBox3
			// 
			this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.2000792026519775D), Telerik.Reporting.Drawing.Unit.Inch(0D));
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox3.Value = "= Fields.Name";
			// 
			// pageFooterSection1
			// 
			this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1D);
			this.pageFooterSection1.Name = "pageFooterSection1";
			// 
			// LeadSource
			// 
			this.LeadSource.DataMember = "GetLeads";
			this.LeadSource.DataSource = typeof(CRM.Report.LeadsSource);
			this.LeadSource.Name = "LeadSource";
			// 
			// textBox6
			// 
			this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.79992127418518066D));
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox6.Value = "Email";
			// 
			// textBox7
			// 
			this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0D));
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
			this.textBox7.Value = "= Fields.Email";
			// 
			// LeadsReport
			// 
			this.DataSource = this.LeadSource;
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detailSection1,
            this.pageFooterSection1});
			this.Name = "LeadsReport";
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
		private Telerik.Reporting.ObjectDataSource LeadSource;
		private Telerik.Reporting.TextBox textBox4;
		private Telerik.Reporting.TextBox textBox5;
		private Telerik.Reporting.TextBox textBox6;
		private Telerik.Reporting.TextBox textBox7;
	}
}