using furaku.OpenPraparatHelper.Services;
using furaku.CellsFileAccessorLib.Repositories;
using furaku.CellsFileAccessorLib.Services;
using furaku.CellsFileAccessorLib.Values;
using furaku.CellsFileGeneralQuery.Values;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace furaku.CellsFileGeneralQuery;

/// <summary>汎用問い合わせコントロール</summary>
public partial class GeneralQueryControl : UserControl, IExtend
{
	/// <inheritdoc/>
	public virtual string DesiredKey => "generalQuery";

	/// <inheritdoc/>
	public virtual string DefaultTabName => "汎用問い合わせ";

	/// <summary>細胞ファイルアクセサ</summary>
	protected virtual CellsFileAccessor? CellsFileAccessor { get; set; }

	/// <summary>工程メッセンジャ</summary>
	protected virtual ConcurrentQueue<IProcessMessage> ProcessMessages { get; private init; }

	/// <summary>タスク</summary>
	protected virtual ISimpleCancelableTask? Task { get; set; }

	private GenralQueryControlState _state;
	/// <summary>状態</summary>
	public GenralQueryControlState State
	{
		get => _state;
		set
		{
			_state = value;
			this.loadIdsBox.Enabled = _state == GenralQueryControlState.NONE;
			this.loadButton.Enabled = _state == GenralQueryControlState.NONE || _state == GenralQueryControlState.LOAD;
			this.loadButton.Text = (_state == GenralQueryControlState.LOAD || _state == GenralQueryControlState.CANCEL_LOAD) ? "キャンセル(&L)" : "読み込み(&L)";

			this.findSQLBox.Enabled = _state == GenralQueryControlState.NONE;
			this.findButton.Enabled = _state == GenralQueryControlState.NONE || _state == GenralQueryControlState.FIND;
			this.findButton.Text = (_state == GenralQueryControlState.FIND || _state == GenralQueryControlState.CANCEL_FIND) ? "キャンセル(&F)" : "検索(&F)";

			this.scalarTypeBox.Enabled = _state == GenralQueryControlState.NONE;
			this.scalarSQLBox.Enabled = _state == GenralQueryControlState.NONE;
			this.queryButton.Enabled = _state == GenralQueryControlState.NONE || _state == GenralQueryControlState.QUERY;
			this.queryButton.Text = (_state == GenralQueryControlState.QUERY || _state == GenralQueryControlState.CANCEL_QUERY) ? "キャンセル(&Q)" : "問い合わせ(&Q))";
		}
	}

	/// <summary>コンストラクタ</summary>
	public GeneralQueryControl()
	{
		this.InitializeComponent();
		this.scalarTypeBox.Items.AddRange(SqlType.Elemnts.ToArray());
		this.scalarTypeBox.SelectedIndex = 0;
		this.State = GenralQueryControlState.NO_FILE;
		this.Dock = DockStyle.Fill;
		this.openFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
		this.CellsFileAccessor = null;
		this.ProcessMessages = new();
		this.Task = null;
		this.outputViewTimer.Tick += (s, e) =>
		{
			var sb = new StringBuilder();
			while (this.ProcessMessages.TryDequeue(out var message))
			{
				if (message is StartFindParticleFromFile)
				{
					sb.AppendLine();
					sb.AppendLine("ファイルからの読み込みを開始");
				}
				else if (message is EndFindParticleFromFile)
				{
					sb.AppendLine("ファイルからの読み込みを完了");
				}
				else if (message is ReadParticleFromFile readParticle)
				{
					sb.AppendLine(string.Format("粒子を読み込み ID：{0}", readParticle.Value.ID));
				}
				else if (message is StartFindCellToSqlite)
				{
					sb.AppendLine();
					sb.AppendLine("細胞の検索を開始");
				}
				else if (message is EndFindCellToSqlite)
				{
					sb.AppendLine("細胞の検索を完了");
				}
				else if (message is FindCellToSqlite findCell)
				{
					sb.AppendLine(string.Format("細胞を発見 ID：{0}", findCell.Value.ID));
				}
				else if (message is StartQueryCellToSqlite)
				{
					sb.AppendLine();
					sb.AppendLine("細胞への問い合わせを開始");
				}
				else if (message is EndQueryCellToSqlite)
				{
					sb.AppendLine("細胞への問い合わせを完了");
				}
				else if (message is QueryCellToSqlite queryResult)
				{
					sb.AppendLine(string.Format("問い合わせ結果 {0}：{1}", queryResult.Value.Number, queryResult.Value.Result));
				}
				else if (message is StartCancelation)
				{
					sb.AppendLine("キャンセルを開始");
				}
				else if (message is EndCnacelation)
				{
					sb.AppendLine("キャンセルを完了");
				}
			}
			if (sb.Length > 0)
			{
				this.outputBox.AppendText(sb.ToString());
				this.outputBox.ScrollToCaret();
			}
		};
		this.outputViewTimer.Start();
	}

	/// <inheritdoc/>
	public virtual void Closed()
	{
		this.Task?.Dispose();
		this.Task = null;
		this.CellsFileAccessor?.Dispose();
		this.CellsFileAccessor = null;
	}

	/// <summary>ファイル指定ボタンクリックイベントハンドラ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント引数</param>
	protected virtual void SpecificationFileButton_Click(object sender, EventArgs e)
	{
		this.specificationFileButton.Enabled = false;
		if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
		{
			this.CellsFileAccessor?.Dispose();
			this.CellsFileAccessor = new(this.openFileDialog.FileNames[0]);
			this.State = GenralQueryControlState.NONE;
			this.startPanel.Visible = false;
		}
	}

	/// <summary>読み込みボタンクリックイベントハンドラ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント引数</param>
	protected virtual async void LoadButton_Click(object sender, EventArgs e)
	{
		if (this.State == GenralQueryControlState.LOAD)
		{
			this.State = GenralQueryControlState.CANCEL_LOAD;
			this.Task?.Cancel();
			this.ProcessMessages.Enqueue(new StartCancelation(this));
		}
		else
		{
			this.State = GenralQueryControlState.LOAD;
			try
			{
				this.outputTab.SelectedIndex = 2;
				int[] ids = this.loadIdsBox.Text.Split(',')
					.Where(elem => !string.IsNullOrWhiteSpace(elem))
					.Select(elem => int.Parse(elem)).ToArray();
				var task = new SimpleCancelableTask(token =>
				{
					this.CellsFileAccessor!.Load(token, this.ProcessMessages.Enqueue, ids);
				});
				this.Task = task;
				await task.Start();
				this.ShowMessageBox(MessageBoxMessage.COMPLETED_LOAD);
			}
			catch (FormatException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_ID);
			}
			catch (OverflowException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_ID);
			}
			catch (OperationCanceledException)
			{
				this.ProcessMessages.Enqueue(new EndCnacelation(this));
				this.ShowMessageBox(MessageBoxMessage.COMPLETED_CANCEL);
			}
			catch (FailToFileAccessException)
			{
				this.ShowMessageBox(MessageBoxMessage.FAIL_TO_ACCESE_FILE);
			}
			catch (InvalidFileContentException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_FILE_CONTENT);
			}
			finally
			{
				this.Task?.Dispose();
				this.Task = null;
				this.State = GenralQueryControlState.NONE;
			}
		}
	}

	/// <summary>検索ボタンクリックイベントハンドラ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント引数</param>
	protected async void FindButton_Click(object sender, EventArgs e)
	{
		if (this.State == GenralQueryControlState.FIND)
		{
			this.State = GenralQueryControlState.CANCEL_FIND;
			this.Task?.Cancel();
			this.ProcessMessages.Enqueue(new StartCancelation(this));
		}
		else
		{
			this.State = GenralQueryControlState.FIND;
			try
			{
				this.outputTab.SelectedIndex = 2;
				this.resultFindView.Nodes.Clear();
				var task = new SimpleCancelableTask<TreeNode[]>(token =>
				{
					return this.CellsFileAccessor!
					.Find(this.findSQLBox.Text, token, this.ProcessMessages.Enqueue)
					.Select(elem => CreateNode(elem))
					.ToArray();
				});
				this.Task = task;
				var elements = await task.Start();
				this.resultFindView.BeginUpdate();
				this.resultFindView.Nodes.AddRange(elements);
				this.resultFindView.EndUpdate();
				this.outputTab.SelectedIndex = 0;
				this.ShowMessageBox(MessageBoxMessage.COMPLETE_FIND);
			}
			catch (OperationCanceledException)
			{
				this.ProcessMessages.Enqueue(new EndCnacelation(this));
				this.ShowMessageBox(MessageBoxMessage.COMPLETED_CANCEL);
			}
			catch (ArgumentException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_SQL);
			}
			catch (SqliteException ex)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_SQL, ex.Message);
			}
			catch (InvalidOperationException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_SQL);
			}
			finally
			{
				this.Task?.Dispose();
				this.Task = null;
				this.State = GenralQueryControlState.NONE;
			}
		}
	}

	/// <summary>ノード作成</summary>
	/// <param name="cell">細胞</param>
	/// <returns>ノード</returns>
	protected virtual TreeNode CreateNode(Cell cell)
	{
		var node = new TreeNode("ID:" + cell.ID);
		node.Nodes.Add("歳:" + cell.Age);
		node.Nodes.Add("α:" + cell.Alpha);
		node.Nodes.Add("エネルギー:" + cell.Energy);
		var dynamicsNode = new TreeNode("動力学");
		dynamicsNode.Nodes.Add("物理演算を行うか:" + (cell.Dynamics.DoMechanicsProcess ? "Yes" : "No"));
		dynamicsNode.Nodes.Add("ブロックとの衝突を行うか:" + (cell.Dynamics.DoHitBlock ? "Yes" : "No"));
		dynamicsNode.Nodes.Add("他の細胞との衝突を行うか:" + (cell.Dynamics.DoHitCell ? "Yes" : "No"));
		var positionNode = new TreeNode("位置");
		positionNode.Nodes.Add("x:" + (cell.Dynamics.Position.X));
		positionNode.Nodes.Add("y:" + (cell.Dynamics.Position.Y));
		positionNode.Nodes.Add("z:" + (cell.Dynamics.Position.Z));
		dynamicsNode.Nodes.Add(positionNode);
		var positionOfDivisionNode = new TreeNode("分割位置");
		positionOfDivisionNode.Nodes.Add("x:" + (cell.Dynamics.PositionOfDivision.X));
		positionOfDivisionNode.Nodes.Add("y:" + (cell.Dynamics.PositionOfDivision.Y));
		positionOfDivisionNode.Nodes.Add("z:" + (cell.Dynamics.PositionOfDivision.Z));
		dynamicsNode.Nodes.Add(positionOfDivisionNode);
		var velocityNode = new TreeNode("速度");
		velocityNode.Nodes.Add("x:" + (cell.Dynamics.Velocity.X));
		velocityNode.Nodes.Add("y:" + (cell.Dynamics.Velocity.Y));
		velocityNode.Nodes.Add("z:" + (cell.Dynamics.Velocity.Z));
		dynamicsNode.Nodes.Add(velocityNode);
		dynamicsNode.Nodes.Add("バネの運動演算を行うか:" + (cell.Dynamics.DoSpringProcess ? "Yes" : "No"));
		var force = new TreeNode("力");
		force.Nodes.Add("x:" + (cell.Dynamics.Force.X));
		force.Nodes.Add("y:" + (cell.Dynamics.Force.Y));
		force.Nodes.Add("z:" + (cell.Dynamics.Force.Z));
		dynamicsNode.Nodes.Add(force);
		dynamicsNode.Nodes.Add("質量:" + cell.Dynamics.Mass);
		dynamicsNode.Nodes.Add("半径:" + cell.Dynamics.Radius);
		node.Nodes.Add(dynamicsNode);
		var opticsNode = new TreeNode("光学");
		var colorNode = new TreeNode("色");
		colorNode.Nodes.Add("赤:" + cell.Optics.Color.Red);
		colorNode.Nodes.Add("緑:" + cell.Optics.Color.Green);
		colorNode.Nodes.Add("青:" + cell.Optics.Color.Blue);
		opticsNode.Nodes.Add(colorNode);
		var lightNode = new TreeNode("光");
		lightNode.Nodes.Add("赤:" + cell.Optics.Light.Red);
		lightNode.Nodes.Add("緑:" + cell.Optics.Light.Green);
		lightNode.Nodes.Add("青:" + cell.Optics.Light.Blue);
		opticsNode.Nodes.Add(lightNode);
		var alphaChannelNode = new TreeNode("吸収率");
		alphaChannelNode.Nodes.Add("赤:" + cell.Optics.AlphaChannel.Red);
		alphaChannelNode.Nodes.Add("緑:" + cell.Optics.AlphaChannel.Green);
		alphaChannelNode.Nodes.Add("青:" + cell.Optics.AlphaChannel.Blue);
		opticsNode.Nodes.Add(alphaChannelNode);
		node.Nodes.Add(opticsNode);
		var genomeNode = new TreeNode("ゲノム");
		genomeNode.Nodes.Add("本:" + new string(cell.Genome.Book.Take(short.MaxValue).ToArray()));
		genomeNode.Nodes.Add("栞:" + new string(cell.Genome.BookMark.ToArray()));
		genomeNode.Nodes.Add("拡張栞:" + new string(cell.Genome.AdvancedBookMark.ToArray()));
		genomeNode.Nodes.Add("結合待ちの手の番号:" + cell.Genome.WaitingConnectionHands);
		genomeNode.Nodes.Add("切断待ち:" + (cell.Genome.IsWaitingDisconnection ? "Yes" : "No"));
		node.Nodes.Add(genomeNode);
		var connectionsNode = new TreeNode("結合");
		connectionsNode.Nodes.AddRange(cell.Connections.Select((elem, index) =>
		{
			var connectionNode = new TreeNode(index.ToString());
			if (elem is null)
			{
				connectionNode.Nodes.Add("無し");
			}
			else
			{
				connectionNode.Nodes.Add("結合相手:" + elem.TargetID);
				connectionNode.Nodes.Add("バネ定数:" + elem.SpringConstant);
				connectionNode.Nodes.Add("バネ自然長:" + elem.Length);
				connectionNode.Nodes.Add("パラメータS:" + elem.S);
			}
			return connectionNode;
		}).ToArray());
		node.Nodes.Add(connectionsNode);
		var neuralNetworkNode = new TreeNode("ニューラルネットワーク");
		neuralNetworkNode.Nodes.Add("有効か:" + (cell.NeuralNetwork.IsEnabled ? "Yes" : "No"));
		var inputsNodes = new TreeNode("入力値");
		inputsNodes.Nodes.AddRange(cell.NeuralNetwork.Inputs
			.Select((elem, index) => new TreeNode(string.Format("{0}:{1}", index, elem)))
			.ToArray());
		neuralNetworkNode.Nodes.Add(inputsNodes);
		var outputsNodes = new TreeNode("出力値");
		outputsNodes.Nodes.AddRange(cell.NeuralNetwork.Outputs
			.Select((elem, index) => new TreeNode(string.Format("{0}:{1}", index, elem)))
			.ToArray());
		neuralNetworkNode.Nodes.Add(outputsNodes);
		var inputsWeightNodes = new TreeNode("入力重み係数");
		inputsWeightNodes.Nodes.AddRange(cell.NeuralNetwork.InputWeights
			.Select((elem, index) => new TreeNode(string.Format("{0}:{1}", index, elem)))
			.ToArray());
		neuralNetworkNode.Nodes.Add(inputsWeightNodes);
		var outputsWeightNodes = new TreeNode("出力重み係数");
		outputsWeightNodes.Nodes.AddRange(cell.NeuralNetwork.OutputWeights
			.Select((elem, index) => new TreeNode(string.Format("{0}:{1}", index, elem)))
			.ToArray());
		neuralNetworkNode.Nodes.Add(outputsWeightNodes);
		neuralNetworkNode.Nodes.Add("捕食試行中か:" + (cell.NeuralNetwork.IsEatingTry ? "Yes" : "No"));
		neuralNetworkNode.Nodes.Add("取り込み試行中か:" + (cell.NeuralNetwork.IsTakingTry ? "Yes" : "No"));
		neuralNetworkNode.Nodes.Add("発光試行中か:" + (cell.NeuralNetwork.IsEmittingTry ? "Yes" : "No"));
		node.Nodes.Add(neuralNetworkNode);

		return node;
	}

	/// <summary>問い合わせボタンクリックイベントハンドラ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント引数</param>
	protected async void QueryButton_Click(object sender, EventArgs e)
	{
		if (this.State == GenralQueryControlState.QUERY)
		{
			this.State = GenralQueryControlState.CANCEL_QUERY;
			this.Task?.Cancel();
			this.ProcessMessages.Enqueue(new StartCancelation(this));
		}
		else
		{
			this.State = GenralQueryControlState.QUERY;
			try
			{
				this.outputTab.SelectedIndex = 2;
				this.resultQueryView.Rows.Clear();
				var sqltype = (SqlType)this.scalarTypeBox.SelectedItem;
				var task = new SimpleCancelableTask<(int Number, string Value)[]>(token =>
				{
					return (sqltype.Type switch
					{
						SqlTypes.BOOL => this.CellsFileAccessor!
						.Query<bool>(this.scalarSQLBox.Text, token, this.ProcessMessages.Enqueue)
						.Select((elem, index) => (index, elem.ToString())),
						SqlTypes.STRING => this.CellsFileAccessor!
						.Query<string>(this.scalarSQLBox.Text, token, this.ProcessMessages.Enqueue)
						.Select((elem, index) => (index, elem[..Math.Min(elem.Length, short.MaxValue)])),
						SqlTypes.INTEGER => this.CellsFileAccessor!
						.Query<int>(this.scalarSQLBox.Text, token, this.ProcessMessages.Enqueue)
						.Select((elem, index) => (index, elem.ToString())),
						SqlTypes.DECIMAL => this.CellsFileAccessor!
						.Query<decimal>(this.scalarSQLBox.Text, token, this.ProcessMessages.Enqueue)
						.Select((elem, index) => (index, elem.ToString())),
						SqlTypes.DOUBLE => this.CellsFileAccessor!
						.Query<double>(this.scalarSQLBox.Text, token, this.ProcessMessages.Enqueue)
						.Select((elem, index) => (index, elem.ToString())),
						_ => throw new NotImplementedException(),
					}).ToArray();
				});
				this.Task = task;
				foreach (var result in await task.Start())
				{
					this.resultQueryView.Rows.Add();
					var row = this.resultQueryView.Rows[^1];
					row.Cells[0].Value = result.Number;
					row.Cells[1].Value = result.Value;
				}
				this.outputTab.SelectedIndex = 1;
				this.ShowMessageBox(MessageBoxMessage.COMPLETE_QUERY);
			}
			catch (OperationCanceledException)
			{
				this.ProcessMessages.Enqueue(new EndCnacelation(this));
				this.ShowMessageBox(MessageBoxMessage.COMPLETED_CANCEL);
			}
			catch (ArgumentException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_SQL);
			}
			catch (SqliteException ex)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_SQL, ex.Message);
			}
			catch (InvalidOperationException)
			{
				this.ShowMessageBox(MessageBoxMessage.INVALID_SQL);
			}
			finally
			{
				this.Task?.Dispose();
				this.Task = null;
				this.State = GenralQueryControlState.NONE;
			}
		}
	}

	/// <summary>メッセージボックスを表示</summary>
	/// <param name="message">メッセージ</param>
	/// <param name="arguments">引数</param>
	protected virtual void ShowMessageBox(MessageBoxMessage message, params object[] arguments)
	{
		switch (message)
		{
			case MessageBoxMessage.COMPLETED_LOAD:
				MessageBox.Show(this, "読み込みを完了しました。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				break;
			case MessageBoxMessage.COMPLETE_FIND:
				MessageBox.Show(this, "検索を完了しました。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				break;
			case MessageBoxMessage.COMPLETE_QUERY:
				MessageBox.Show(this, "問い合わせを完了しました。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				break;
			case MessageBoxMessage.COMPLETED_CANCEL:
				MessageBox.Show(this, "キャンセルを完了しました。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				break;
			case MessageBoxMessage.INVALID_ID:
				MessageBox.Show(this, "IDの指定が不正です。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				break;
			case MessageBoxMessage.INVALID_SQL:
				var text = arguments.Length > 0 ? "SQLが不正です。\n" + arguments[0] : "SQLが不正です。";
				MessageBox.Show(this, text, this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				break;
			case MessageBoxMessage.FAIL_TO_ACCESE_FILE:
				MessageBox.Show(this, "ファイルにアクセス出来ませでした。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				break;
			case MessageBoxMessage.INVALID_FILE_CONTENT:
				MessageBox.Show(this, "ファイル内容が不正です。", this.DefaultTabName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}

/// <summary>状態</summary>
public enum GenralQueryControlState
{
	/// <summary>ファイル指定無</summary>
	NO_FILE = -1,
	/// <summary>無し</summary>
	NONE = 0,
	/// <summary>読み込み中</summary>
	LOAD,
	/// <summary>検索中</summary>
	FIND,
	/// <summary>問い合わせ中</summary>
	QUERY,
	/// <summary>読み込みキャンセル中</summary>
	CANCEL_LOAD = 11,
	/// <summary>検索キャンセル中</summary>
	CANCEL_FIND,
	/// <summary>問い合わせキャンセル中</summary>
	CANCEL_QUERY,
}

/// <summary>メッセージボックスメッセージ</summary>
public enum MessageBoxMessage
{
	/// <summary>読み込みが完了</summary>
	COMPLETED_LOAD,
	/// <summary>検索が完了</summary>
	COMPLETE_FIND,
	/// <summary>問い合せが完了</summary>
	COMPLETE_QUERY,
	/// <summary>キャンセルが完了</summary>
	COMPLETED_CANCEL,
	/// <summary>IDが不正</summary>
	INVALID_ID,
	/// <summary>SQLが不正</summary>
	INVALID_SQL,
	/// <summary>ファイルアクセスに失敗</summary>
	FAIL_TO_ACCESE_FILE,
	/// <summary>ファイル内容が不正</summary>
	INVALID_FILE_CONTENT,
}

#region 工程メッセージ
/// <summary>キャンセル開始</summary>
public class StartCancelation : ProcessMassage<GeneralQueryControl, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public StartCancelation(GeneralQueryControl sender) : base(sender, null) { }
}

/// <summary>キャンセル終了</summary>
public class EndCnacelation : ProcessMassage<GeneralQueryControl, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public EndCnacelation(GeneralQueryControl sender) : base(sender, null) { }
}
#endregion
