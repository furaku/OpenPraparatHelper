namespace furaku.OpenPraparatHelper;

partial class MainForm
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

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		functionsTab = new TabControl();
		menuStrip = new MenuStrip();
		openFileDialog = new OpenFileDialog();
		SuspendLayout();
		// 
		// functionsTab
		// 
		functionsTab.Dock = DockStyle.Fill;
		functionsTab.Location = new Point(0, 24);
		functionsTab.Name = "functionsTab";
		functionsTab.SelectedIndex = 0;
		functionsTab.Size = new Size(800, 426);
		functionsTab.TabIndex = 0;
		// 
		// menuStrip
		// 
		menuStrip.Location = new Point(0, 0);
		menuStrip.Name = "menuStrip";
		menuStrip.Size = new Size(800, 24);
		menuStrip.TabIndex = 1;
		menuStrip.Text = "menuStrip1";
		// 
		// openFileDialog
		// 
		openFileDialog.FileName = "openFileDialog1";
		// 
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(800, 450);
		Controls.Add(functionsTab);
		Controls.Add(menuStrip);
		MainMenuStrip = menuStrip;
		Name = "MainForm";
		Text = "OepnPraparat解析";
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private TabControl functionsTab;
	private MenuStrip menuStrip;
	private OpenFileDialog openFileDialog;
}