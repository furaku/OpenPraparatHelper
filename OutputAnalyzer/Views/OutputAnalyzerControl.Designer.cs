namespace furaku.OutputAnalyzer;

partial class OutputAnalyzerControl
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Component Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify 
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		label1 = new Label();
		executeButton = new Button();
		openpraparatPathBox = new TextBox();
		logfilePathBox = new TextBox();
		csvfilePathBox = new TextBox();
		label2 = new Label();
		label3 = new Label();
		outputBox = new Common.Views.ConsoleControl();
		switchFileBox = new NumericUpDown();
		label4 = new Label();
		refButton = new Button();
		label5 = new Label();
		intervalBox = new NumericUpDown();
		openFileDialog = new OpenFileDialog();
		((System.ComponentModel.ISupportInitialize)switchFileBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)intervalBox).BeginInit();
		SuspendLayout();
		// 
		// label1
		// 
		label1.AutoSize = true;
		label1.Location = new Point(3, 6);
		label1.Name = "label1";
		label1.Size = new Size(172, 15);
		label1.TabIndex = 0;
		label1.Text = "OpenPraparat実行ファイルパス(&P)";
		// 
		// executeButton
		// 
		executeButton.Location = new Point(3, 91);
		executeButton.Name = "executeButton";
		executeButton.Size = new Size(75, 23);
		executeButton.TabIndex = 11;
		executeButton.Text = "実行(&G)";
		executeButton.UseVisualStyleBackColor = true;
		executeButton.Click += ExecuteButton_Click;
		// 
		// openpraparatPathBox
		// 
		openpraparatPathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		openpraparatPathBox.Location = new Point(181, 3);
		openpraparatPathBox.Name = "openpraparatPathBox";
		openpraparatPathBox.Size = new Size(535, 23);
		openpraparatPathBox.TabIndex = 1;
		// 
		// logfilePathBox
		// 
		logfilePathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		logfilePathBox.Location = new Point(181, 32);
		logfilePathBox.Name = "logfilePathBox";
		logfilePathBox.Size = new Size(364, 23);
		logfilePathBox.TabIndex = 4;
		// 
		// csvfilePathBox
		// 
		csvfilePathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		csvfilePathBox.Location = new Point(181, 61);
		csvfilePathBox.Name = "csvfilePathBox";
		csvfilePathBox.Size = new Size(364, 23);
		csvfilePathBox.TabIndex = 8;
		// 
		// label2
		// 
		label2.AutoSize = true;
		label2.Location = new Point(3, 35);
		label2.Name = "label2";
		label2.Size = new Size(92, 15);
		label2.TabIndex = 3;
		label2.Text = "ログファイルパス(&L)";
		// 
		// label3
		// 
		label3.AutoSize = true;
		label3.Location = new Point(3, 64);
		label3.Name = "label3";
		label3.Size = new Size(95, 15);
		label3.TabIndex = 7;
		label3.Text = "CSVファイルパス(&C)";
		// 
		// outputBox
		// 
		outputBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		outputBox.BackColor = SystemColors.Window;
		outputBox.BorderStyle = BorderStyle.Fixed3D;
		outputBox.Font = new Font("ＭＳ ゴシック", 10F, FontStyle.Regular, GraphicsUnit.Point);
		outputBox.Location = new Point(3, 119);
		outputBox.Margin = new Padding(4, 2, 4, 2);
		outputBox.MaxLines = 0;
		outputBox.Name = "outputBox";
		outputBox.SelectedColor = Color.Empty;
		outputBox.Size = new Size(794, 304);
		outputBox.TabIndex = 12;
		outputBox.UpdateInterval = 100;
		// 
		// switchFileBox
		// 
		switchFileBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		switchFileBox.Location = new Point(677, 33);
		switchFileBox.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
		switchFileBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
		switchFileBox.Name = "switchFileBox";
		switchFileBox.Size = new Size(120, 23);
		switchFileBox.TabIndex = 6;
		switchFileBox.TextAlign = HorizontalAlignment.Right;
		switchFileBox.Value = new decimal(new int[] { 10000, 0, 0, 0 });
		// 
		// label4
		// 
		label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		label4.AutoSize = true;
		label4.Location = new Point(551, 35);
		label4.Name = "label4";
		label4.Size = new Size(120, 15);
		label4.TabIndex = 5;
		label4.Text = "ファイル切り替え行数(&S)";
		// 
		// refButton
		// 
		refButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		refButton.Location = new Point(722, 2);
		refButton.Name = "refButton";
		refButton.Size = new Size(75, 23);
		refButton.TabIndex = 2;
		refButton.Text = "参照(&R)";
		refButton.UseVisualStyleBackColor = true;
		refButton.Click += RefButton_Click;
		// 
		// label5
		// 
		label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		label5.AutoSize = true;
		label5.Location = new Point(551, 64);
		label5.Name = "label5";
		label5.Size = new Size(101, 15);
		label5.TabIndex = 9;
		label5.Text = "出力ステップ間隔(&I)";
		// 
		// intervalBox
		// 
		intervalBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		intervalBox.Location = new Point(677, 62);
		intervalBox.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
		intervalBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
		intervalBox.Name = "intervalBox";
		intervalBox.Size = new Size(120, 23);
		intervalBox.TabIndex = 10;
		intervalBox.TextAlign = HorizontalAlignment.Right;
		intervalBox.Value = new decimal(new int[] { 100, 0, 0, 0 });
		// 
		// openFileDialog
		// 
		openFileDialog.DefaultExt = "exe";
		openFileDialog.FileName = "openFileDialog1";
		openFileDialog.Filter = "実行ファイル|*.exe";
		openFileDialog.Title = "OpenPraparat実行ファイル選択";
		// 
		// OutputAnalyzerControl
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		Controls.Add(label5);
		Controls.Add(intervalBox);
		Controls.Add(refButton);
		Controls.Add(label4);
		Controls.Add(switchFileBox);
		Controls.Add(outputBox);
		Controls.Add(label3);
		Controls.Add(label2);
		Controls.Add(csvfilePathBox);
		Controls.Add(logfilePathBox);
		Controls.Add(executeButton);
		Controls.Add(label1);
		Controls.Add(openpraparatPathBox);
		Margin = new Padding(0);
		Name = "OutputAnalyzerControl";
		Size = new Size(800, 426);
		((System.ComponentModel.ISupportInitialize)switchFileBox).EndInit();
		((System.ComponentModel.ISupportInitialize)intervalBox).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion
	private TextBox openpraparatPathBox;
	private Label label1;
	private Button executeButton;
	private TextBox logfilePathBox;
	private TextBox csvfilePathBox;
	private Label label2;
	private Label label3;
	private Common.Views.ConsoleControl outputBox;
	private NumericUpDown switchFileBox;
	private Label label4;
	private Button refButton;
	private Label label5;
	private NumericUpDown intervalBox;
	private OpenFileDialog openFileDialog;
}