using furaku.CellsFileAccessorLib.Repositories.DbEntities;
using furaku.CellsFileAccessorLib.Services;
using furaku.CellsFileAccessorLib.Values;
using furaku.Common.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories;

/// <summary>細胞リポジトリ（Sqlite）</summary>
public class CellInSqlite : Repository<Cell>, IDisposable
{
	#region 静的メンバ
	/// <summary>エンティティからの変換（細胞）</summary>
	/// <param name="cell">エンティティ</param>
	/// <returns>細胞</returns>
	protected static Cell ConvertCell(CellEntity cell)
	{
		return new Cell(
			cell.CellID, true, cell.Age, cell.Alpha, cell.Energy,
			cell.DoMechanicsProcess, cell.DoHitBlock, cell.DoHitCell,
			cell.PositionX, cell.PositionY, cell.PositionZ,
			cell.PositionOfDivisionX, cell.PositionOfDivisionY, cell.PositionOfDivisionZ,
			cell.VelocityX, cell.VelocityY, cell.VelocityZ,
			cell.DoSpringProcess, cell.ForceX, cell.ForceY, cell.ForceZ,
			cell.Mass, cell.Radius,
			cell.ColorRed, cell.ColorGeen, cell.ColorBlue, cell.LigthRed, cell.LigthGreen, cell.LigthBlue,
			cell.AlphaChannelRed, cell.AlphaChannelGreen, cell.AlphaChannelBlue,
			cell.Book.ToCharArray(), cell.BookMark.ToCharArray(), cell.AdvancedBookMark.ToCharArray(),
			cell.WaitingConnectionHands, cell.IsWaitingDisconnection,
			ConvertConnection(cell.ConnectionCount, cell.Connections.ToArray()),
			ConvertNeuralNetworkn(cell.IsEnabledNeuralNetwork, cell.IsEatingTry, cell.IsTakingTry, cell.IsEmittingTry, cell.NeuralNetworkValues.ToArray())
			);
	}

	/// <summary>エンティティからの変換（細胞結合）</summary>
	/// <param name="connectionCount">結合手数</param>
	/// <param name="connections">エンティティ</param>
	/// <returns>細胞結合</returns>
	protected static CellConnection?[] ConvertConnection(int connectionCount, params CellConnectionEntity[] connections)
	{
		List<CellConnection?> ret = new();
		for (int i = 0; i < connectionCount; i++)
		{
			var connection = connections.SingleOrDefault(c => c.HandID == i);
			ret.Add(connection is null ? null : new CellConnection(connection.TargetCellID, connection.SpringConstant, connection.Length, connection.S));
		}
		return ret.ToArray();
	}

	/// <summary>エンティティからの変換（ニューラルネットワーク）</summary>
	/// <param name="isEnabled">有効か</param>
	/// <param name="isEatingTry">捕食試行中か</param>
	/// <param name="isTakingTry">取り込み試行中か</param>
	/// <param name="isEmittingTry">発光試行中か</param>
	/// <param name="neuralNetworkValue">エンティティ</param>
	/// <returns>ニューラルネットワーク</returns>
	protected static NeuralNetwork ConvertNeuralNetworkn(bool isEnabled, bool isEatingTry, bool isTakingTry, bool isEmittingTry,
		params NeuralNetworkValueEntity[] neuralNetworkValue)
	{
		return new NeuralNetwork(isEnabled,
			neuralNetworkValue.Where(nuralnetwork => !nuralnetwork.IsOutput && !nuralnetwork.IsWeight)
				.OrderBy(nuralnetwork => nuralnetwork.Index).Select(nuralnetwork => nuralnetwork.Value).ToArray(),
			neuralNetworkValue.Where(nuralnetwork => nuralnetwork.IsOutput && !nuralnetwork.IsWeight)
				.OrderBy(nuralnetwork => nuralnetwork.Index).Select(nuralnetwork => nuralnetwork.Value).ToArray(),
			neuralNetworkValue.Where(nuralnetwork => !nuralnetwork.IsOutput && nuralnetwork.IsWeight)
				.OrderBy(nuralnetwork => nuralnetwork.Index).Select(nuralnetwork => nuralnetwork.Value).ToArray(),
			neuralNetworkValue.Where(nuralnetwork => nuralnetwork.IsOutput && nuralnetwork.IsWeight)
				.OrderBy(nuralnetwork => nuralnetwork.Index).Select(nuralnetwork => nuralnetwork.Value).ToArray(),
			isEatingTry, isTakingTry, isEmittingTry
			);
	}

	/// <summary>エンティティへの変換（細胞）</summary>
	/// <param name="cell">細胞</param>
	/// <returns>エンティティ</returns>
	protected static CellEntity ConvertCell(Cell cell)
	{
		(var connectCount, var connections) = ConvertConnection(cell.ID, cell.Connections);
		var ret = new CellEntity()
		{
			CellID = cell.ID,
			Age = cell.Age,
			Alpha = cell.Alpha,
			Energy = cell.Energy,
			DoMechanicsProcess = cell.Dynamics.DoMechanicsProcess,
			DoHitBlock = cell.Dynamics.DoHitBlock,
			DoHitCell = cell.Dynamics.DoHitCell,
			PositionX = cell.Dynamics.Position.X,
			PositionY = cell.Dynamics.Position.Y,
			PositionZ = cell.Dynamics.Position.Z,
			PositionOfDivisionX = (int)cell.Dynamics.PositionOfDivision.X,
			PositionOfDivisionY = (int)cell.Dynamics.PositionOfDivision.Y,
			PositionOfDivisionZ = (int)cell.Dynamics.PositionOfDivision.Z,
			VelocityX = cell.Dynamics.Velocity.X,
			VelocityY = cell.Dynamics.Velocity.Y,
			VelocityZ = cell.Dynamics.Velocity.Z,
			ColorRed = cell.Optics.Color.Red,
			ColorGeen = cell.Optics.Color.Green,
			ColorBlue = cell.Optics.Color.Blue,
			LigthRed = cell.Optics.Light.Red,
			LigthGreen = cell.Optics.Light.Green,
			LigthBlue = cell.Optics.Light.Blue,
			DoSpringProcess = cell.Dynamics.DoSpringProcess,
			ForceX = cell.Dynamics.Force.X,
			ForceY = cell.Dynamics.Force.Y,
			ForceZ = cell.Dynamics.Force.Z,
			Mass = cell.Dynamics.Mass,
			Radius = cell.Dynamics.Radius,
			AlphaChannelRed = cell.Optics.AlphaChannel.Red,
			AlphaChannelGreen = cell.Optics.AlphaChannel.Green,
			AlphaChannelBlue = cell.Optics.AlphaChannel.Blue,
			Book = new string(cell.Genome.Book.ToArray()),
			BookMark = new string(cell.Genome.BookMark.ToArray()),
			AdvancedBookMark = new string(cell.Genome.AdvancedBookMark.ToArray()),
			WaitingConnectionHands = cell.Genome.WaitingConnectionHands,
			IsWaitingDisconnection = cell.Genome.IsWaitingDisconnection,
			IsEnabledNeuralNetwork = cell.NeuralNetwork.IsEnabled,
			IsEatingTry = cell.NeuralNetwork.IsEatingTry,
			IsTakingTry = cell.NeuralNetwork.IsTakingTry,
			IsEmittingTry = cell.NeuralNetwork.IsEmittingTry,
			ConnectionCount = connectCount,
		};
		((List<CellConnectionEntity>)ret.Connections).AddRange(connections);
		((List<NeuralNetworkValueEntity>)ret.NeuralNetworkValues).AddRange(ConvertNeuralNetworkn(cell.ID, cell.NeuralNetwork));
		return ret;
	}

	/// <summary>エンティティへ変換（細胞結合）</summary>
	/// <param name="cellID">細胞ID</param>
	/// <param name="connections">細胞結合</param>
	/// <returns>エンティティ</returns>
	protected static (int count, CellConnectionEntity[]) ConvertConnection(int cellID, IEnumerable<CellConnection?> connections)
	{
		return (connections.Count(), connections
			.Where(connection => connection is not null)
			.Select((connection, i) => new CellConnectionEntity()
			{
				CellID = cellID,
				HandID = i,
				TargetCellID = connection!.TargetID,
				SpringConstant = connection.SpringConstant,
				Length = connection.Length,
				S = connection.S,
			})
			.ToArray());
	}

	/// <summary>エンティティへ変換（ニューラルネットワーク）</summary>
	/// <param name="cellID">細胞ID</param>
	/// <param name="neuralNetwork">ニューラルネットワーク</param>
	/// <returns>エンティティ</returns>
	protected static NeuralNetworkValueEntity[] ConvertNeuralNetworkn(int cellID, NeuralNetwork neuralNetwork)
	{
		List<NeuralNetworkValueEntity> ret = new(neuralNetwork.Inputs.Select((input, i) => new NeuralNetworkValueEntity()
		{
			CellID = cellID,
			Index = i,
			IsOutput = false,
			IsWeight = false,
			Value = input,
		}));
		ret.AddRange(neuralNetwork.Outputs.Select((output, i) => new NeuralNetworkValueEntity()
		{
			CellID = cellID,
			Index = i,
			IsOutput = true,
			IsWeight = false,
			Value = output,
		}));
		ret.AddRange(neuralNetwork.InputWeights.Select((inputWdight, i) => new NeuralNetworkValueEntity()
		{
			CellID = cellID,
			Index = i,
			IsOutput = false,
			IsWeight = true,
			Value = inputWdight,
		}));
		ret.AddRange(neuralNetwork.OutputWeights.Select((outputWdight, i) => new NeuralNetworkValueEntity()
		{
			CellID = cellID,
			Index = i,
			IsOutput = true,
			IsWeight = true,
			Value = outputWdight,
		}));
		return ret.ToArray();
	}
	#endregion

	/// <summary>DBコンテキスト</summary>
	protected virtual CellContext DbContext { get; private init; }

	/// <summary>コンストラクタ</summary>
	public CellInSqlite()
	{
		this.DbContext = new();
		this.DbContext.Database.OpenConnection();
		this.DbContext.Database.EnsureCreated();
	}

	/// <inheritdoc/>
	public override IEnumerable<Cell> Find(
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null,
		params int[] elements_ids)
	{
		IEnumerable<Cell> ret;
		if (elements_ids.Length > 0)
		{
			ret = this.Find(string.Format(@"
SELECT *
	FROM t_cell
	WHERE t_cell.cell_id IN ({0})
", string.Join(',', elements_ids)), cancellationToken, sendProcessMessageMethod);
		}
		else
		{
			ret = this.Find($@"
SELECT *
	FROM t_cell
", cancellationToken, sendProcessMessageMethod);
		}

		return ret;
	}

	/// <inheritdoc/>
	public override IEnumerable<Cell> Find(
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null)
	{
		sendProcessMessageMethod?.Invoke(new StartFindCellToSqlite(this));
		foreach (var cell in this.DbContext
			.Cells
			.FromSql(FormattableStringFactory.Create(sql))
			.Include(cell => cell.Connections)
			.Include(cell => cell.NeuralNetworkValues)
			.Select(elem => ConvertCell(elem)))
		{
			cancellationToken?.ThrowIfCancellationRequested();
			sendProcessMessageMethod?.Invoke(new FindCellToSqlite(this, cell));
			yield return cell;
		}
		sendProcessMessageMethod?.Invoke(new EndFindCellToSqlite(this));
	}

	/// <inheritdoc/>
	public override IEnumerable<R> Query<R>(
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null,
		params object[] parameters)
	{
		sendProcessMessageMethod?.Invoke(new StartQueryCellToSqlite(this));
		var i = 1;
		foreach (var result in this.DbContext.Database.SqlQueryRaw<R>(sql, parameters))
		{
			cancellationToken?.ThrowIfCancellationRequested();
			sendProcessMessageMethod?.Invoke(new QueryCellToSqlite(this, i++, result));
			yield return result;
		}
		sendProcessMessageMethod?.Invoke(new EndQueryCellToSqlite(this));
	}

	/// <inheritdoc/>
	public override void Marge(
		IEnumerable<Cell> elemnets,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null)
	{
		// TODO 既に有る物は更新

		cancellationToken?.ThrowIfCancellationRequested();
		sendProcessMessageMethod?.Invoke(new StartMargeToSqlite(this));
		var database = this.DbContext.Database;
		using var locker = new ManualResetEventSlim(true);
		foreach (var elemnet in elemnets)
		{
			locker.Wait();
			cancellationToken?.ThrowIfCancellationRequested();
			locker.Reset();
			Task.Run(() =>
			{
				var cell = ConvertCell(elemnet);
				var sql = @$"INSERT INTO t_cell (
cell_id,
age,
alpha,
energy,
do_mechanics_process,
do_hit_block,
do_hit_cell,
position_x,
position_y,
position_z,
position_of_division_x,
position_of_division_y,
position_of_division_z,
velocity_x,
velocity_y,
velocity_z,
color_red,
color_green,
color_blue,
light_red,
light_green,
light_blue,
do_spring_process,
force_x,
force_y,
force_z,
mass,
radius,
alpha_channel_red,
alpha_channel_green,
alpha_channel_blue,
book,
bookmark,
advanced_bookmark,
waiting_connection_hands,
is_waiting_disconnection,
is_enabled_neuralnetwork,
is_eatingtry,
is_takingtry,
is_emittingtry,
connection_count
) VALUES (
{cell.CellID},
{cell.Age},
{cell.Alpha},
{cell.Energy},
{cell.DoMechanicsProcess},
{cell.DoHitBlock},
{cell.DoHitCell},
{cell.PositionX},
{cell.PositionY},
{cell.PositionZ},
{cell.PositionOfDivisionX},
{cell.PositionOfDivisionY},
{cell.PositionOfDivisionZ},
{cell.VelocityX},
{cell.VelocityY},
{cell.VelocityZ},
{cell.ColorRed},
{cell.ColorGeen},
{cell.ColorBlue},
{cell.LigthRed},
{cell.LigthGreen},
{cell.LigthBlue},
{cell.DoSpringProcess},
{cell.ForceX},
{cell.ForceY},
{cell.ForceZ},
{cell.Mass},
{cell.Radius},
{cell.AlphaChannelRed},
{cell.AlphaChannelGreen},
{cell.AlphaChannelBlue},
'{cell.Book}',
'{cell.BookMark}',
'{cell.AdvancedBookMark}',
{cell.WaitingConnectionHands},
{cell.IsWaitingDisconnection},
{cell.IsEnabledNeuralNetwork},
{cell.IsEatingTry},
{cell.IsTakingTry},
{cell.IsEmittingTry},
{cell.ConnectionCount}
)";
				database.ExecuteSqlRaw(sql);

				foreach (var cellConnection in cell.Connections)
				{
					sql = @$"INSERT INTO t_connection (
cell_id,
hand_id,
target_cell_id,
spring_constant,
length,
s
) VALUES (
{cellConnection.CellID},
{cellConnection.HandID},
{cellConnection.TargetCellID},
{cellConnection.SpringConstant},
{cellConnection.Length},
{cellConnection.S}
)";
					database.ExecuteSqlRaw(sql);
				}

				foreach (var neuralNetworkValue in cell.NeuralNetworkValues)
				{
					sql = @$"INSERT INTO t_nuralnetowork_value (
cell_id,
index_,
is_output,
is_weight,
value
) VALUES (
{neuralNetworkValue.CellID},
{neuralNetworkValue.Index},
{neuralNetworkValue.IsOutput},
{neuralNetworkValue.IsWeight},
{neuralNetworkValue.Value}
)";
					database.ExecuteSqlRaw(sql);
				}
				sendProcessMessageMethod?.Invoke(new MargeCellToSqlite(this, elemnet));
				locker.Set();
			});
		}
		locker.Wait();
		sendProcessMessageMethod?.Invoke(new EndMargeToSqlite(this));
	}

	/// <summary>破棄</summary>
	public virtual void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);

	}

	/// <summary>破棄</summary>
	/// <param name="disposing">破棄中</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			this.DbContext.Dispose();
		}
	}
}

#region 工程メッセージ
/// <summary>Sqliteにマージ開始</summary>
public class StartMargeToSqlite : ProcessMessage<CellInSqlite, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public StartMargeToSqlite(CellInSqlite sender) : base(sender, null) { }
}

/// <summary>Sqliteにマージ終了</summary>
public class EndMargeToSqlite : ProcessMessage<CellInSqlite, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public EndMargeToSqlite(CellInSqlite sender) : base(sender, null) { }
}

/// <summary>Sqliteに細胞をマージ</summary>
public class MargeCellToSqlite : ProcessMessage<CellInSqlite, Cell>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="cell">細胞</param>
	public MargeCellToSqlite(CellInSqlite sender, Cell cell) : base(sender, cell) { }
}

/// <summary>Sqliteに細胞の検索開始</summary>
public class StartFindCellToSqlite : ProcessMessage<CellInSqlite, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public StartFindCellToSqlite(CellInSqlite sender) : base(sender, null) { }
}

/// <summary>Sqliteに細胞の検索終了</summary>
public class EndFindCellToSqlite : ProcessMessage<CellInSqlite, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public EndFindCellToSqlite(CellInSqlite sender) : base(sender, null) { }
}

/// <summary>Sqliteに細胞の検索</summary>
public class FindCellToSqlite : ProcessMessage<CellInSqlite, Cell>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="cell">細胞</param>
	public FindCellToSqlite(CellInSqlite sender, Cell cell) : base(sender, cell) { }
}

/// <summary>Sqliteに細胞の問い合わせ開始</summary>
public class StartQueryCellToSqlite : ProcessMessage<CellInSqlite, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public StartQueryCellToSqlite(CellInSqlite sender) : base(sender, null) { }
}

/// <summary>Sqliteに細胞の問い合わせ終了</summary>
public class EndQueryCellToSqlite : ProcessMessage<CellInSqlite, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public EndQueryCellToSqlite(CellInSqlite sender) : base(sender, null) { }
}

/// <summary>Sqliteに細胞の問い合わせ</summary>
public class QueryCellToSqlite : ProcessMessage<CellInSqlite, (int Number, object? Result)>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="number">番号</param>
	/// <param name="result">結果</param>
	public QueryCellToSqlite(CellInSqlite sender, int number, object? result) : base(sender, (number, result)) { }
}
#endregion

