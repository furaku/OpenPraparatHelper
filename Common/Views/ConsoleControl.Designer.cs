namespace furaku.Common.Views
{
	partial class ConsoleControl
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
			vScrollBar = new VScrollBar();
			hScrollBar = new HScrollBar();
			SuspendLayout();
			// 
			// vScrollBar
			// 
			vScrollBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			vScrollBar.Location = new Point(126, 0);
			vScrollBar.Name = "vScrollBar";
			vScrollBar.Size = new Size(17, 109);
			vScrollBar.TabIndex = 0;
			vScrollBar.Scroll += ScrollBar_Scroll;
			// 
			// hScrollBar
			// 
			hScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			hScrollBar.Location = new Point(0, 109);
			hScrollBar.Name = "hScrollBar";
			hScrollBar.Size = new Size(126, 17);
			hScrollBar.TabIndex = 1;
			hScrollBar.Scroll += ScrollBar_Scroll;
			// 
			// ConsoleControl
			// 
			AutoScaleDimensions = new SizeF(7F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Window;
			BorderStyle = BorderStyle.Fixed3D;
			Controls.Add(hScrollBar);
			Controls.Add(vScrollBar);
			Font = new Font("ＭＳ ゴシック", 10F, FontStyle.Regular, GraphicsUnit.Point);
			Margin = new Padding(4, 2, 4, 2);
			Name = "ConsoleControl";
			Size = new Size(146, 127);
			ResumeLayout(false);
		}

		#endregion

		private VScrollBar vScrollBar;
		private HScrollBar hScrollBar;
	}
}
