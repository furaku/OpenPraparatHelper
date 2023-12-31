﻿using furaku.CellsFileAccessorLib.Services;
using furaku.CellsFileAccessorLib.Values;
using furaku.Common.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories;

/// <summary>リポジトリ</summary>
/// <typeparam name="T">要素の型</typeparam>
public abstract class Repository<T>
{
	/// <summary>検索</summary>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="sendProcessMessageMethod">工程メッセージ送信メソッド</param>
	/// <param name="elements_ids">要素IDs</param>
	/// <returns>要素</returns>
	public abstract IEnumerable<T> Find(
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null,
		params int[] elements_ids);

	/// <summary>検索</summary>
	/// <param name="sql">SQL</param>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="snedProcessMessageMethod">工程メッセージ送信メソッド</param>
	/// <returns>結果</returns>
	public abstract IEnumerable<T> Find(
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? snedProcessMessageMethod = null);

	/// <summary>問い合わせ</summary>
	/// <typeparam name="R">結果の型</typeparam>
	/// <param name="sql">SQL</param>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="sendProcessMessageMethod">工程メッセージ送信メソッド</param>
	/// <param name="parameters">パラメータ</param>
	/// <returns>結果</returns>
	public abstract IEnumerable<R> Query<R>(
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null,
		params object[] parameters);

	/// <summary>マージ</summary>
	/// <param name="elemnets">要素</param>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="sendProcessMessageMethod">工程メッセージ送信メソッド</param>
	public abstract void Marge(
		IEnumerable<T> elemnets,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null);

	/// <summary>マージ</summary>
	/// <param name="cancellationToken">キャンセルトークン</param>
	/// <param name="sendProcessMessageMethod">工程メッセージ送信メソッド</param>
	/// <param name="elemnets">要素</param>
	public virtual void Marge(
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? sendProcessMessageMethod = null,
		params T[] elemnets)
	{
		this.Marge((IEnumerable<T>)elemnets, cancellationToken, sendProcessMessageMethod);
	}
}
