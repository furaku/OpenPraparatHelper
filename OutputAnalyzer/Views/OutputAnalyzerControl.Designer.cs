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
		components = new System.ComponentModel.Container();
		label1 = new Label();
		executeButton = new Button();
		openpraparatPathBox = new TextBox();
		outputBox = new TextBox();
		timer = new System.Windows.Forms.Timer(components);
		logfilePathBox = new TextBox();
		csvfilePathBox = new TextBox();
		label2 = new Label();
		label3 = new Label();
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
		executeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		executeButton.Location = new Point(722, 90);
		executeButton.Name = "executeButton";
		executeButton.Size = new Size(75, 23);
		executeButton.TabIndex = 6;
		executeButton.Text = "実行(&G)";
		executeButton.UseVisualStyleBackColor = true;
		executeButton.Click += ExecuteButton_Click;
		// 
		// openpraparatPathBox
		// 
		openpraparatPathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		openpraparatPathBox.Location = new Point(181, 3);
		openpraparatPathBox.Name = "openpraparatPathBox";
		openpraparatPathBox.Size = new Size(616, 23);
		openpraparatPathBox.TabIndex = 1;
		// 
		// outputBox
		// 
		outputBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		outputBox.BackColor = SystemColors.Window;
		outputBox.Location = new Point(3, 119);
		outputBox.MaxLength = int.MaxValue;
		outputBox.Multiline = true;
		outputBox.Name = "outputBox";
		outputBox.ReadOnly = true;
		outputBox.ScrollBars = ScrollBars.Both;
		outputBox.Size = new Size(794, 304);
		outputBox.TabIndex = 7;
		outputBox.WordWrap = false;
		// 
		// logfilePathBox
		// 
		logfilePathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		logfilePathBox.Location = new Point(181, 32);
		logfilePathBox.Name = "logfilePathBox";
		logfilePathBox.Size = new Size(616, 23);
		logfilePathBox.TabIndex = 3;
		// 
		// csvfilePathBox
		// 
		csvfilePathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		csvfilePathBox.Location = new Point(181, 61);
		csvfilePathBox.Name = "csvfilePathBox";
		csvfilePathBox.Size = new Size(616, 23);
		csvfilePathBox.TabIndex = 5;
		// 
		// label2
		// 
		label2.AutoSize = true;
		label2.Location = new Point(3, 35);
		label2.Name = "label2";
		label2.Size = new Size(92, 15);
		label2.TabIndex = 2;
		label2.Text = "ログファイルパス(&L)";
		// 
		// label3
		// 
		label3.AutoSize = true;
		label3.Location = new Point(3, 64);
		label3.Name = "label3";
		label3.Size = new Size(95, 15);
		label3.TabIndex = 4;
		label3.Text = "CSVファイルパス(&C)";
		// 
		// OutputAnalyzerControl
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		Controls.Add(label3);
		Controls.Add(label2);
		Controls.Add(csvfilePathBox);
		Controls.Add(logfilePathBox);
		Controls.Add(outputBox);
		Controls.Add(executeButton);
		Controls.Add(label1);
		Controls.Add(openpraparatPathBox);
		Margin = new Padding(0);
		Name = "OutputAnalyzerControl";
		Size = new Size(800, 426);
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion
	private TextBox openpraparatPathBox;
	private Label label1;
	private Button executeButton;
	private TextBox outputBox;
	private System.Windows.Forms.Timer timer;
	private TextBox logfilePathBox;
	private TextBox csvfilePathBox;
	private Label label2;
	private Label label3;
}