using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.Common.Services;

/// <summary>簡易キャンセル可能タスク</summary>
public class SimpleCancelableTask<R> : ISimpleCancelableTask
{
	private bool _disposedValue;

	/// <summary>タスク</summary>
	protected virtual Task<R> Task { get; private init; }

	/// <summary>キャンセルソース</summary>
	protected virtual CancellationTokenSource Cancellation { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="operation">処理</param>
	public SimpleCancelableTask(Func<CancellationToken, R> operation)
	{
		this.Cancellation = new();
		this.Task = new(() =>
		{
			return operation(this.Cancellation.Token);
		});
	}

	/// <summary>開始</summary>
	/// <returns>タスク</returns>
	public async Task<R> Start()
	{
		this.Task.Start();
		return await this.Task.WaitAsync(Timeout.InfiniteTimeSpan);
	}

	/// <inheritdoc/>
	public virtual void Cancel()
	{
		this.Cancellation.Cancel();
	}

	/// <summary>破棄</summary>
	/// <param name="disposing">破棄するか</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				try
				{
					if (!this.Cancellation.IsCancellationRequested)
					{
						this.Cancel();
					}
					if (!this.Task.IsCompleted)
					{
						this.Task.Wait();
					}
				}
				catch
				{

				}
				this.Cancellation.Dispose();
				this.Task.Dispose();
			}

			_disposedValue = true;
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

/// <summary>簡易キャンセル可能タスク</summary>
public class SimpleCancelableTask : ISimpleCancelableTask
{
	private bool _disposedValue;

	/// <summary>タスク</summary>
	protected virtual SimpleCancelableTask<int> Task { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="operation">処理</param>
	public SimpleCancelableTask(Action<CancellationToken> operation)
	{
		this.Task = new(token =>
		{
			operation(token);
			return 0;
		});
	}

	/// <summary>開始</summary>
	/// <returns>タスク</returns>
	public async Task Start()
	{
		await this.Task.Start();
	}

	/// <inheritdoc/>
	public virtual void Cancel()
	{
		this.Task.Cancel();
	}

	/// <summary>破棄</summary>
	/// <param name="disposing">破棄するか</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				this.Task.Dispose();
			}

			_disposedValue = true;
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

/// <summary>簡易キャンセル可能タスク</summary>
public interface ISimpleCancelableTask : IDisposable
{
	/// <summary>キャンセル</summary>
	void Cancel();
}