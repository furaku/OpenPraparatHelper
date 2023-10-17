using furaku.Common.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>粒子光学</summary>
public abstract class ParticleOptics
{
	/// <summary>色</summary>
	public virtual RgbColor Color { get; private init; }

	/// <summary>光</summary>
	public virtual RgbColor Light { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="cr">色赤</param>
	/// <param name="cg">色緑</param>
	/// <param name="cb">色青</param>
	/// <param name="lr">光赤</param>
	/// <param name="lg">光緑</param>
	/// <param name="lb">光青</param>
	protected ParticleOptics(
		decimal cr, decimal cg, decimal cb,
		decimal lr, decimal lg, decimal lb)
	{
		Color = new(cr, cg, cb);
		Light = new(lr, lg, lb);
	}
}
