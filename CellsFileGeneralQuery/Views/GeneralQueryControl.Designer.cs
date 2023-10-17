namespace furaku.CellsFileGeneralQuery;

partial class GeneralQueryControl
{
	/// <summary> 
	/// 必要なデザイナー変数です。
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary> 
	/// 使用中のリソースをすべてクリーンアップします。
	/// </summary>
	/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region コンポーネント デザイナーで生成されたコード

	/// <summary> 
	/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
	/// コード エディターで変更しないでください。
	/// </summary>
	private void InitializeComponent()
	{
		components = new System.ComponentModel.Container();
		specificationFileButton = new Button();
		openFileDialog = new OpenFileDialog();
		startPanel = new Panel();
		outputBox = new RichTextBox();
		outputTab = new TabControl();
		tabPage1 = new TabPage();
		resultFindView = new TreeView();
		tabPage3 = new TabPage();
		resultQueryView = new DataGridView();
		Number = new DataGridViewTextBoxColumn();
		Value = new DataGridViewTextBoxColumn();
		tabPage2 = new TabPage();
		tableLayoutPanel1 = new TableLayoutPanel();
		queryButton = new Button();
		findButton = new Button();
		label1 = new Label();
		loadIdsBox = new TextBox();
		loadButton = new Button();
		findSQLBox = new TextBox();
		label2 = new Label();
		label3 = new Label();
		panel1 = new Panel();
		scalarSQLBox = new TextBox();
		scalarTypeBox = new ComboBox();
		outputViewTimer = new System.Windows.Forms.Timer(components);
		startPanel.SuspendLayout();
		outputTab.SuspendLayout();
		tabPage1.SuspendLayout();
		tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)resultQueryView).BeginInit();
		tabPage2.SuspendLayout();
		tableLayoutPanel1.SuspendLayout();
		panel1.SuspendLayout();
		SuspendLayout();
		// 
		// specificationFileButton
		// 
		specificationFileButton.AutoSize = true;
		specificationFileButton.Location = new Point(352, 201);
		specificationFileButton.Name = "specificationFileButton";
		specificationFileButton.Size = new Size(97, 25);
		specificationFileButton.TabIndex = 0;
		specificationFileButton.Text = "ファイル指定(&P)";
		specificationFileButton.UseVisualStyleBackColor = true;
		specificationFileButton.Click += SpecificationFileButton_Click;
		// 
		// openFileDialog
		// 
		openFileDialog.AddExtension = false;
		openFileDialog.FileName = "cells_file";
		openFileDialog.Title = "セルファイルを選択";
		// 
		// startPanel
		// 
		startPanel.Controls.Add(specificationFileButton);
		startPanel.Dock = DockStyle.Fill;
		startPanel.Location = new Point(0, 0);
		startPanel.Name = "startPanel";
		startPanel.Size = new Size(800, 426);
		startPanel.TabIndex = 1;
		// 
		// outputBox
		// 
		outputBox.BackColor = SystemColors.Window;
		outputBox.Dock = DockStyle.Fill;
		outputBox.Location = new Point(3, 3);
		outputBox.Name = "outputBox";
		outputBox.ReadOnly = true;
		outputBox.Size = new Size(780, 203);
		outputBox.TabIndex = 0;
		outputBox.Text = "";
		outputBox.WordWrap = false;
		// 
		// outputTab
		// 
		outputTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		outputTab.Controls.Add(tabPage1);
		outputTab.Controls.Add(tabPage3);
		outputTab.Controls.Add(tabPage2);
		outputTab.Location = new Point(6, 186);
		outputTab.Name = "outputTab";
		outputTab.SelectedIndex = 0;
		outputTab.Size = new Size(794, 237);
		outputTab.TabIndex = 1;
		// 
		// tabPage1
		// 
		tabPage1.Controls.Add(resultFindView);
		tabPage1.Location = new Point(4, 24);
		tabPage1.Name = "tabPage1";
		tabPage1.Padding = new Padding(3);
		tabPage1.Size = new Size(786, 209);
		tabPage1.TabIndex = 0;
		tabPage1.Text = "検索結果";
		tabPage1.UseVisualStyleBackColor = true;
		// 
		// resultFindView
		// 
		resultFindView.Dock = DockStyle.Fill;
		resultFindView.Location = new Point(3, 3);
		resultFindView.Name = "resultFindView";
		resultFindView.Size = new Size(780, 203);
		resultFindView.TabIndex = 0;
		// 
		// tabPage3
		// 
		tabPage3.Controls.Add(resultQueryView);
		tabPage3.Location = new Point(4, 24);
		tabPage3.Name = "tabPage3";
		tabPage3.Size = new Size(786, 209);
		tabPage3.TabIndex = 2;
		tabPage3.Text = "問い合わせ結果";
		tabPage3.UseVisualStyleBackColor = true;
		// 
		// resultQueryView
		// 
		resultQueryView.AllowUserToAddRows = false;
		resultQueryView.AllowUserToDeleteRows = false;
		resultQueryView.AllowUserToOrderColumns = true;
		resultQueryView.AllowUserToResizeRows = false;
		resultQueryView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		resultQueryView.Columns.AddRange(new DataGridViewColumn[] { Number, Value });
		resultQueryView.Dock = DockStyle.Fill;
		resultQueryView.Location = new Point(0, 0);
		resultQueryView.Name = "resultQueryView";
		resultQueryView.ReadOnly = true;
		resultQueryView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
		resultQueryView.RowTemplate.Height = 25;
		resultQueryView.ShowCellErrors = false;
		resultQueryView.ShowCellToolTips = false;
		resultQueryView.ShowEditingIcon = false;
		resultQueryView.ShowRowErrors = false;
		resultQueryView.Size = new Size(786, 209);
		resultQueryView.TabIndex = 0;
		// 
		// Number
		// 
		Number.HeaderText = "行";
		Number.Name = "Number";
		Number.ReadOnly = true;
		// 
		// Value
		// 
		Value.HeaderText = "値";
		Value.Name = "Value";
		Value.ReadOnly = true;
		Value.SortMode = DataGridViewColumnSortMode.NotSortable;
		Value.Width = 643;
		// 
		// tabPage2
		// 
		tabPage2.Controls.Add(outputBox);
		tabPage2.Location = new Point(4, 24);
		tabPage2.Name = "tabPage2";
		tabPage2.Padding = new Padding(3);
		tabPage2.Size = new Size(786, 209);
		tabPage2.TabIndex = 1;
		tabPage2.Text = "ログ";
		tabPage2.UseVisualStyleBackColor = true;
		// 
		// tableLayoutPanel1
		// 
		tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		tableLayoutPanel1.ColumnCount = 3;
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 133F));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 119F));
		tableLayoutPanel1.Controls.Add(queryButton, 2, 2);
		tableLayoutPanel1.Controls.Add(findButton, 2, 1);
		tableLayoutPanel1.Controls.Add(label1, 0, 0);
		tableLayoutPanel1.Controls.Add(loadIdsBox, 1, 0);
		tableLayoutPanel1.Controls.Add(loadButton, 2, 0);
		tableLayoutPanel1.Controls.Add(findSQLBox, 1, 1);
		tableLayoutPanel1.Controls.Add(label2, 0, 1);
		tableLayoutPanel1.Controls.Add(label3, 0, 2);
		tableLayoutPanel1.Controls.Add(panel1, 1, 2);
		tableLayoutPanel1.Location = new Point(3, 3);
		tableLayoutPanel1.Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 3;
		tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
		tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
		tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
		tableLayoutPanel1.Size = new Size(794, 177);
		tableLayoutPanel1.TabIndex = 0;
		// 
		// queryButton
		// 
		queryButton.Dock = DockStyle.Fill;
		queryButton.Location = new Point(678, 89);
		queryButton.Name = "queryButton";
		queryButton.Size = new Size(113, 85);
		queryButton.TabIndex = 8;
		queryButton.Text = "問い合わせ(&Q)";
		queryButton.UseVisualStyleBackColor = true;
		queryButton.Click += QueryButton_Click;
		// 
		// findButton
		// 
		findButton.Dock = DockStyle.Fill;
		findButton.Location = new Point(678, 30);
		findButton.Name = "findButton";
		findButton.Size = new Size(113, 53);
		findButton.TabIndex = 7;
		findButton.Text = "検索(&F)";
		findButton.UseVisualStyleBackColor = true;
		findButton.Click += FindButton_Click;
		// 
		// label1
		// 
		label1.Dock = DockStyle.Fill;
		label1.Location = new Point(3, 0);
		label1.Name = "label1";
		label1.Size = new Size(127, 27);
		label1.TabIndex = 0;
		label1.Text = "追加読み込みID(&I)";
		label1.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// loadIdsBox
		// 
		loadIdsBox.Dock = DockStyle.Fill;
		loadIdsBox.Location = new Point(136, 3);
		loadIdsBox.Name = "loadIdsBox";
		loadIdsBox.Size = new Size(536, 23);
		loadIdsBox.TabIndex = 1;
		// 
		// loadButton
		// 
		loadButton.Dock = DockStyle.Fill;
		loadButton.Location = new Point(678, 3);
		loadButton.Name = "loadButton";
		loadButton.Size = new Size(113, 21);
		loadButton.TabIndex = 2;
		loadButton.Text = "読み込み(&L)";
		loadButton.UseVisualStyleBackColor = true;
		loadButton.Click += LoadButton_Click;
		// 
		// findSQLBox
		// 
		findSQLBox.Dock = DockStyle.Fill;
		findSQLBox.Location = new Point(136, 30);
		findSQLBox.Multiline = true;
		findSQLBox.Name = "findSQLBox";
		findSQLBox.Size = new Size(536, 53);
		findSQLBox.TabIndex = 3;
		// 
		// label2
		// 
		label2.AutoSize = true;
		label2.Dock = DockStyle.Fill;
		label2.Location = new Point(3, 27);
		label2.Name = "label2";
		label2.Size = new Size(127, 59);
		label2.TabIndex = 5;
		label2.Text = "検索SQL(&N)";
		label2.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// label3
		// 
		label3.AutoSize = true;
		label3.Dock = DockStyle.Fill;
		label3.Location = new Point(3, 86);
		label3.Name = "label3";
		label3.Size = new Size(127, 91);
		label3.TabIndex = 6;
		label3.Text = "スカラ問い合わせSQL(&S)";
		label3.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// panel1
		// 
		panel1.Controls.Add(scalarSQLBox);
		panel1.Controls.Add(scalarTypeBox);
		panel1.Dock = DockStyle.Fill;
		panel1.Location = new Point(133, 86);
		panel1.Margin = new Padding(0);
		panel1.Name = "panel1";
		panel1.Size = new Size(542, 91);
		panel1.TabIndex = 9;
		// 
		// scalarSQLBox
		// 
		scalarSQLBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		scalarSQLBox.Location = new Point(3, 32);
		scalarSQLBox.Multiline = true;
		scalarSQLBox.Name = "scalarSQLBox";
		scalarSQLBox.Size = new Size(536, 56);
		scalarSQLBox.TabIndex = 10;
		// 
		// scalarTypeBox
		// 
		scalarTypeBox.DisplayMember = "Name";
		scalarTypeBox.FormattingEnabled = true;
		scalarTypeBox.Location = new Point(3, 3);
		scalarTypeBox.Name = "scalarTypeBox";
		scalarTypeBox.Size = new Size(94, 23);
		scalarTypeBox.TabIndex = 0;
		// 
		// outputViewTimer
		// 
		outputViewTimer.Interval = 500;
		// 
		// GeneralQueryControl
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		BackColor = SystemColors.Control;
		Controls.Add(startPanel);
		Controls.Add(outputTab);
		Controls.Add(tableLayoutPanel1);
		Name = "GeneralQueryControl";
		Size = new Size(800, 426);
		startPanel.ResumeLayout(false);
		startPanel.PerformLayout();
		outputTab.ResumeLayout(false);
		tabPage1.ResumeLayout(false);
		tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)resultQueryView).EndInit();
		tabPage2.ResumeLayout(false);
		tableLayoutPanel1.ResumeLayout(false);
		tableLayoutPanel1.PerformLayout();
		panel1.ResumeLayout(false);
		panel1.PerformLayout();
		ResumeLayout(false);
	}

	#endregion

	private Button specificationFileButton;
	private OpenFileDialog openFileDialog;
	private Panel startPanel;
	private TabControl outputTab;
	private TabPage tabPage1;
	private TabPage tabPage2;
	private RichTextBox outputBox;
	private TableLayoutPanel tableLayoutPanel1;
	private Label label1;
	private TextBox loadIdsBox;
	private Button loadButton;
	private TextBox findSQLBox;
	private TreeView resultFindView;
	private Button queryButton;
	private Button findButton;
	private Label label2;
	private Label label3;
	private ComboBox scalarTypeBox;
	private System.Windows.Forms.Timer outputViewTimer;
	private Panel panel1;
	private TextBox scalarSQLBox;
	private TabPage tabPage3;
	private DataGridView resultQueryView;
	private DataGridViewTextBoxColumn Number;
	private DataGridViewTextBoxColumn Value;
}
