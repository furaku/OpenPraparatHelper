using furaku.CellsFileAccessorLib.Repositories;
using furaku.CellsFileAccessorLib.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace furaku.CellsFileAccessorLib.Services;

/// <summary>細胞ファイルアクセサ</summary>
public class CellsFileAccessor : IDisposable
{
	/// <summary>ファイルパス</summary>
	public virtual string FilePath { get; private init; }

	/// <summary>細胞のSqliteのリポジトリ</summary>
	protected virtual CellInSqlite CellSqliteRepository { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="filePath">ファイルパス</param>
	public CellsFileAccessor(string filePath)
	{
		this.CellSqliteRepository = new();
		this.FilePath = filePath;
	}

	/// <summary>ファイルから粒子の読み込み</summary>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="processMessageEnqueueMethod">工程メッセージ追加メッセージ</param>
	/// <param name="ids">粒子ID</param>
	/// <exception cref="OperationCanceledException">キャンセル例外</exception>
	/// <exception cref="FailToFileAccessException">ファイルアクセス失敗</exception>
	/// <exception cref="InvalidFileContentException">ファイル内容不正</exception>
	public virtual void Load(
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null,
		params int[] ids)
	{
		var containsIds = this.CellSqliteRepository.Query<int>($@"
SELECT t_cell.cell_id
	FROM t_cell
	WHERE t_cell.cell_id IN ({string.Join(',', ids)})
");

		var exceptIds = ids.Except(containsIds).ToArray();
		var particles = new ParticleInFile(this.FilePath)
			.Find(null, processMessageEnqueueMethod, exceptIds);
		this.CellSqliteRepository.Marge(particles.Cast<Cell>(), cancellationToken, processMessageEnqueueMethod);
	}

	/// <summary>検索</summary>
	/// <param name="sql">SQL</param>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="processMessageEnqueueMethod">工程メッセージ追加メッセージ</param>
	/// <returns>結果</returns>
	public virtual IEnumerable<Cell> Find(
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null)
	{
		return this.CellSqliteRepository.Find(sql, cancellationToken, processMessageEnqueueMethod);
	}

	/// <summary>問い合わせ</summary>
	/// <typeparam name="R">結果の型</typeparam>
	/// <param name="sql">SQL</param>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="processMessageEnqueueMethod">工程メッセージ追加メッセージ</param>
	/// <param name="parameters">パラメータ</param>
	/// <returns>結果</returns>
	public virtual IEnumerable<R> Query<R>(
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null,
		params object[] parameters)
	{
		return this.CellSqliteRepository.Query<R>(sql, cancellationToken, processMessageEnqueueMethod, parameters);
	}

	/// <summary>破棄</summary>
	public virtual void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>破棄</summary>
	/// <param name="disposing">破棄中か</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			this.CellSqliteRepository.Dispose();
		}
	}
}
