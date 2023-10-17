using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>細胞結合</summary>
public class CellConnection
{
	/// <summary>結合相手のID</summary>
	public virtual int TargetID { get; private init; }

	/// <summary>バネ定数</summary>
	public virtual decimal SpringConstant { get; private init; }

	/// <summary>バネ自然長</summary>
	public virtual decimal Length { get; private init; }

	/// <summary>パラメータS</summary>
	public virtual decimal S { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="targetID">結合相手のID</param>
	/// <param name="springConstant">バネ定数</param>
	/// <param name="length">バネ自然長</param>
	/// <param name="s">パラメータS</param>
	public CellConnection(int targetID, decimal springConstant, decimal length, decimal s)
	{
		SpringConstant = springConstant;
		Length = length;
		S = s;
		TargetID = targetID;
	}
}
