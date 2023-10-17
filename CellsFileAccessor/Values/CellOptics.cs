using furaku.Common.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>細胞光学</summary>
public class CellOptics : ParticleOptics
{
	/// <summary>光吸収率</summary>
	public virtual RgbColor AlphaChannel { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="cr">色赤</param>
	/// <param name="cg">色緑</param>
	/// <param name="cb">色青</param>
	/// <param name="ir">発光強度赤</param>
	/// <param name="ig">発光強度緑</param>
	/// <param name="ib">発光強度青</param>
	/// <param name="ar">吸収率赤</param>
	/// <param name="ag">吸収率緑</param>
	/// <param name="ab">吸収率青</param>
	public CellOptics(
		decimal cr, decimal cg, decimal cb,
		decimal ir, decimal ig, decimal ib,
		decimal ar, decimal ag, decimal ab)
		: base (cr, cg, cb, ir, ig, ib)
	{
		AlphaChannel = new(ar, ag, ab);
	}
}
