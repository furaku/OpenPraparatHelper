using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>光子光学</summary>
public class PhotonOptics : ParticleOptics
{
	/// <summary>コンストラクタ</summary>
	/// <param name="cr">色赤</param>
	/// <param name="cg">色緑</param>
	/// <param name="cb">色青</param>
	/// <param name="lr">光赤</param>
	/// <param name="lg">光緑</param>
	/// <param name="lb">光青</param>
	public PhotonOptics(
		decimal cr, decimal cg, decimal cb,
		decimal lr, decimal lg, decimal lb)
		: base(cr, cg, cb, lr, lg, lb)
	{ }
}
