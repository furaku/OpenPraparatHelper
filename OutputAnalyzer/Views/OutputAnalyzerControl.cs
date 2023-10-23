using furaku.CellsFileAccessorLib.Repositories;
using furaku.Common.Services;
using furaku.OpenPraparatHelper.Services;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace furaku.OutputAnalyzer;

/// <summary>OpenPraparatの出力を解析する拡張機能</summary>
public partial class OutputAnalyzerControl : UserControl, IExtend
{
	/// <inheritdoc/>
	public virtual string DesiredKey => "outputAnalyzer";

	/// <inheritdoc/>
	public virtual string DefaultTabName => "出力解析";

	/// <summary>OpenPraparatを実行しているプロセス</summary>
	protected virtual Process? Process { get; set; }

	/// <summary>出力新規追加行</summary>
	protected virtual List<(string, Color)> OutputNewLines { get; }

	/// <summary>実行中の出力中か</summary>
	public virtual bool IsProcessing { get; protected set; }

	/// <summary>コンストラクタ</summary>
	public OutputAnalyzerControl()
	{
		InitializeComponent();

		this.outputBox.MaxLines = 30000;
		this.outputBox.SelectedColor = Color.DeepSkyBlue;
		this.Dock = DockStyle.Fill;
		this.OutputNewLines = new();
		this.IsProcessing = false;
		this.timer.Tick += (s, e) =>
		{
			(string, Color)[] lines;
			lock (this.OutputNewLines)
			{
				lines = this.OutputNewLines.ToArray();
				this.OutputNewLines.Clear();
			}
			this.outputBox.AppendLine(lines);
		};
		this.timer.Start();
	}

	/// <summary>実行ボタンクリックイベントハンドラ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント引数</param>
	protected virtual void ExecuteButton_Click(object sender, EventArgs e)
	{
		if (this.Process is null)
		{
			this.SetIsExecuting(true);
			try
			{
				this.Process = new Process();
				this.Process.StartInfo.FileName = this.openpraparatPathBox.Text.Trim();
				this.Process.StartInfo.UseShellExecute = false;
				this.Process.StartInfo.RedirectStandardOutput = true;
				this.Process.StartInfo.RedirectStandardError = true;
				this.Process.StartInfo.CreateNoWindow = true;
				try
				{
					this.Process.Start();
				}
				catch (Win32Exception ex)
				{
					throw new FaileToAccessFileException(this.Process.StartInfo.FileName, ex);
				}
				catch (InvalidOperationException ex)
				{
					throw new FaileToAccessFileException(this.Process.StartInfo.FileName, ex);
				}
				Task.Run(() =>
				{
					StreamWriter? logWriterOrg = null; ;
					TextWriter? logWriter = null;
					StreamWriter? csvWriterOrg = null;
					TextWriter? csvWriter = null;
					try
					{
						var logfilePath = this.logfilePathBox.Text.Trim();
						var logNumber = 0;
						if (!string.IsNullOrWhiteSpace(logfilePath))
						{
							try
							{
								logWriterOrg = new(logfilePath + "_" + logNumber++);
								logWriter = TextWriter.Synchronized(logWriterOrg);
							}
							catch (IOException)
							{
								this.Invoke(() =>
								{
									this.ShowMessageBox(logfilePath);
								});
							}
							catch (UnauthorizedAccessException)
							{
								this.Invoke(() =>
								{
									this.ShowMessageBox(logfilePath);
								});
							}
						}
						var csvfilePath = this.csvfilePathBox.Text.Trim();
						if (!string.IsNullOrWhiteSpace(csvfilePath))
						{
							try
							{
								csvWriterOrg = new(csvfilePath);
								csvWriter = TextWriter.Synchronized(csvWriterOrg);
							}
							catch (IOException)
							{
								this.Invoke(() =>
								{
									this.ShowMessageBox(csvfilePath);
								});
							}
							catch (UnauthorizedAccessException)
							{
								this.Invoke(() =>
								{
									this.ShowMessageBox(csvfilePath);
								});
							}
						}
						var reader = this.Process.StandardOutput;
						try
						{
							var count = 0;
							while (!this.Process.WaitForExit(0))
							{
								var line = reader.ReadLine();
								while (line is not null)
								{
									lock (this.OutputNewLines)
									{
										this.OutputNewLines.Add((line, Color.Empty));
									}
									logWriter?.WriteLineAsync(line);
									this.WriteLineToCsv(csvWriter, line);
									line = reader.ReadLine();
									if (logWriter is not null)
									{
										count = (count + 1) % (int)this.rotaionBox.Value;
										if (count == 0)
										{
											logWriter?.Dispose();
											logWriterOrg?.Dispose();
											try
											{
												logWriterOrg = new(logfilePath + "_" + logNumber++);
												logWriter = TextWriter.Synchronized(logWriterOrg);
											}
											catch (IOException)
											{
												this.Invoke(() =>
												{
													this.ShowMessageBox(logfilePath);
												});
											}
											catch (UnauthorizedAccessException)
											{
												this.Invoke(() =>
												{
													this.ShowMessageBox(logfilePath);
												});
											}
										}
									}
								}
							}
						}
						catch (ExternalException) { }
						catch (InvalidOperationException) { }
					}
					finally
					{
						logWriter?.Dispose();
						logWriterOrg?.Dispose();
						csvWriter?.Dispose();
						csvWriterOrg?.Dispose();
						this.Process.Dispose();
						this.Process = null;
						this.Invoke(() => this.SetIsExecuting(false));
					}
				});
				Task.Run(() =>
				{
					var reader = this.Process.StandardError;
					try
					{
						while (!this.Process.WaitForExit(0))
						{
							var line = reader.ReadLine();
							while (line is not null)
							{
								lock (this.OutputNewLines)
								{
									this.OutputNewLines.Add((line, Color.Red));
								}
								line = reader.ReadLine();
							}
						}
					}
					catch (ExternalException) { }
					catch (InvalidOperationException) { }
					catch (NullReferenceException) { }
				});
			}
			catch (FaileToAccessFileException ex)
			{
				this.ShowMessageBox(ex.Path);
				this.SetIsExecuting(false);
			}
		}
		else
		{
			this.Process.Kill(true);
		}
	}

	/// <summary>
	/// 実行中かを設定
	/// </summary>
	/// <param name="isExecuting">実行中か</param>
	protected virtual void SetIsExecuting(bool isExecuting)
	{
		this.openpraparatPathBox.Enabled = !isExecuting;
		this.logfilePathBox.Enabled = !isExecuting;
		this.csvfilePathBox.Enabled = !isExecuting;
		this.executeButton.Text = isExecuting ? "停止(&G)" : "実行(&G)";
	}

	/// <summary>行をCSVに書き出し</summary>
	/// <param name="writer">書き出し器</param>
	/// <param name="line">行</param>
	protected virtual void WriteLineToCsv(TextWriter? writer, string line)
	{
		if (this.IsProcessing)
		{
			if (line.Trim() == "#####################################################")
			{
				writer?.WriteLineAsync();
				this.IsProcessing = false;
			}
			else
			{
				var parameter = line.Split('=');
				if (parameter.Length == 2)
				{
					var label = parameter[0].Trim();
					var val = parameter[1].Trim();
					switch (label)
					{
						case "WORLD_STEP":
							this.WriteValue(writer, val, 1);
							break;
						case "CALC_CLL":
							this.WriteValue(writer, val, 1);
							break;
						case "NOT_CALC_CLL":
							this.WriteValue(writer, val, 1);
							break;
						case "NUMBER_OF_INFO_TRANS":
							this.WriteValue(writer, val, 1);
							break;
						case "CALC_CLL-INFO_TRANS":
							this.WriteValue(writer, val, 1);
							break;
						case "CALC_CLL+NOT_CALC_CLL":
							this.WriteValue(writer, val, 1);
							break;
						case "CONN_CLL":
							this.WriteValue(writer, val, 1);
							break;
						case "EAT_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "EAT_INFO*":
							this.WriteValue(writer, val, 9);
							break;
						case "EATEN_INFO*":
							this.WriteValue(writer, val, 9);
							break;
						case "FUSION_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "EXPANSION_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "CONNECT_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "DISCONNECT_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "TURN_BOOKMARKER_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "MUTATION_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "DEATHS_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "CELLS(SUN_ID)%LIGHT_AF":
							this.WriteValue(writer, val, 3);
							break;
						case "sun_theta":
							this.WriteValue(writer, val, 1);
							break;
						case "TOT_ENERGY*":
							this.WriteValue(writer, val, 1);
							break;
						case "AVE_ENERGY*":
							this.WriteValue(writer, val, 1);
							break;
						case "EVERY_STEP_COST_A":
							this.WriteValue(writer, val, 1);
							break;
						case "CHANGE_L_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "ENERGY_TRANSIT_COUNT*":
							this.WriteValue(writer, val, 1);
							break;
						case "CENTER_OF_GRAV":
							this.WriteValue(writer, val, 3);
							break;
						case "AGE INFO FOR CELL":
							this.WriteValue(writer, val, 4);
							break;
						case "AGE INFO FOR IT":
							this.WriteValue(writer, val, 4);
							break;
						case "TOT_M*":
							this.WriteValue(writer, val, 1);
							break;
						case "ALONE_COUNT*":
							this.WriteValue(writer, val, 1, false);
							break;
					}
				}
			}
		}
		else
		{
			if (line.Trim() == "#####################################################")
			{
				this.IsProcessing = true;
			}
		}
	}

	/// <summary>値の書き出し</summary>
	/// <param name="writer">書き出し器</param>
	/// <param name="val">値</param>
	/// <param name="count">個数</param>
	/// <param name="addComma">カンマを追加するか</param>
	protected virtual void WriteValue(TextWriter? writer, string val, int count, bool addComma = true)
	{
		var vals = val.Split(" ", StringSplitOptions.RemoveEmptyEntries);
		for (var i = 0; i < count; i++)
		{
			if (vals.Length > i)
			{
				writer?.WriteAsync(vals[i]);
			}
			if (i != count - 1 || addComma)
			{
				writer?.WriteAsync(",");
			}
		}
	}

	/// <summary>メッセージボックスを表示</summary>
	/// <param name="path">パス</param>
	protected virtual void ShowMessageBox(string path)
	{
		MessageBox.Show(this, "ファイルにアクセス出来ませんでした。" + Environment.NewLine + "パス：" + path, this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Error);
	}

	/// <inheritdoc/>
	public virtual void Closed()
	{
		this.Process?.Close();
	}
}

/// <summary>ファイルアクセス失敗例外</summary>
public class FaileToAccessFileException : Exception
{
	/// <summary>ファイルパス</summary>
	public virtual string Path { get; }

	/// <summary>コンストラクタ</summary>
	/// <param name="path">ファイルパス</param>
	/// <param name="inner">内部例外</param>
	public FaileToAccessFileException(string path, Exception inner) : base("Faile to access the file", inner)
	{
		this.Path = path;
	}
}
