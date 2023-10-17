using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>ゲノム</summary>
public class Genome
{
	#region 静的メンバ
	/// <summary>コードの正規表現</summary>
	protected static Regex CodeRegex { get; }

	/// <summary>静的コンストラクタ</summary>
	static Genome() => CodeRegex = new Regex(@"^[A-ZA-z\d]*$");
	#endregion

	private readonly char[] _book;
	/// <summary>本</summary>
	public virtual IEnumerable<char> Book { get => _book; }

	private readonly char[] _bookMark;
	/// <summary>栞</summary>
	public virtual IEnumerable<char> BookMark { get => _bookMark; }

	private readonly char[] _advancedBookMark;
	/// <summary>拡張栞</summary>
	public virtual IEnumerable<char> AdvancedBookMark { get => _advancedBookMark; }

	/// <summary>接続待ちの結合手</summary>
	public virtual int? WaitingConnectionHands { get; private init; }

	/// <summary>切断待ちか</summary>
	public virtual bool IsWaitingDisconnection { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="book">本</param>
	/// <param name="bookmark">栞</param>
	/// <param name="advanced_bookmark">拡張栞</param>
	/// <param name="waitingConnectionHands">接続待ちの結合手</param>
	/// <param name="isWaitingDisconnection">切断待ちか</param>
	public Genome(char[] book, char[] bookmark, char[] advanced_bookmark, int? waitingConnectionHands, bool isWaitingDisconnection)
	{
		_book = book;
		_bookMark = bookmark;
		_advancedBookMark = advanced_bookmark;
		WaitingConnectionHands = waitingConnectionHands;
		IsWaitingDisconnection = isWaitingDisconnection;
	}

	/// <summary>
	/// ゲノム動作種別
	/// </summary>
	public enum ActionType
	{
		/// <summary>
		/// 展開
		/// </summary>
		EXPANSION,
		/// <summary>
		/// 結合
		/// </summary>
		CONNECTION,
		/// <summary>
		/// 切断
		/// </summary>
		DISCONNECTION,
		/// <summary>
		/// 遷移
		/// </summary>
		TRANSITION,
	}
}
