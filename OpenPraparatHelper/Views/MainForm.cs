using furaku.OpenPraparatHelper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace furaku.OpenPraparatHelper;

/// <summary>メインフォーム</summary>
public partial class MainForm : Form
{
	/// <summary>拡張機能管理器</summary>
	public virtual ExtendsManager ExtendsManager { get; private init; }

	/// <summary>コンストラクタ</summary>
	public MainForm()
	{
		InitializeComponent();

		ExtendsManager = new();
		foreach (var extends in ExtendsManager.Load())
		{
			string key = extends.GetType().FullName!;
			this.functionsTab.TabPages.Add(key, extends.DefaultTabName);
			this.functionsTab.TabPages[key].Controls.Add((Control)extends);
		}
	}
}
