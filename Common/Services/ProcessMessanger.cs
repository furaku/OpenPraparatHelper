using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.Common.Services;

/// <summary>工程メッセージ</summary>
/// <typeparam name="S">送信者の型</typeparam>
/// <typeparam name="V">値の型</typeparam>
public abstract class ProcessMassage<S, V> : IProcessMessage where S : class
{
	/// <summary>送信者</summary>
	public virtual S Sender { get; private init; }
	object IProcessMessage.Sneder => this.Sender;

	/// <summary>値</summary>
	public virtual V Value { get; private init; }
	object? IProcessMessage.Value => this.Value;

	/// <inheritdoc/>
	public virtual DateTime Time { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="val">値</param>
	public ProcessMassage(S sender, V val)
	{
		this.Sender = sender;
		this.Value = val;
		this.Time = DateTime.Now;
	}
}

/// <summary>工程メッセージ</summary>
public interface IProcessMessage
{
	/// <summary>送信者</summary>
	object Sneder { get; }
	/// <summary>値</summary>
	object? Value { get; }
	/// <summary>日時</summary>
	DateTime Time { get; }
}
