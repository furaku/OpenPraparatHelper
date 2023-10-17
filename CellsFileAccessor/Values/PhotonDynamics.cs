using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>光子動力学</summary>
public class PhotonDynamics : ParticleDynamics
{
	/// <summary>コンストラクタ</summary>
	/// <param name="doMechanicsProcess">物理演算を行うか</param>
	/// <param name="doHitBlock">ブロックとの衝突を行うか</param>
	/// <param name="doHitCell">他の細胞との衝突を行うか</param>
	/// <param name="x">位置X</param>
	/// <param name="y">位置Y</param>
	/// <param name="z">位置Z</param>
	/// <param name="nx">分割位置X</param>
	/// <param name="ny">分割位置Y</param>
	/// <param name="nz">分割位置Z</param>
	/// <param name="vx">速度X</param>
	/// <param name="vy">速度Y</param>
	/// <param name="vz">速度Z</param>
	public PhotonDynamics(
		bool doMechanicsProcess, bool doHitBlock, bool doHitCell,
		decimal x, decimal y, decimal z,
		int nx, int ny, int nz,
		decimal vx, decimal vy, decimal vz)
		: base(doMechanicsProcess, doHitBlock, doHitCell, x, y, z, nx, ny, nz, vx, vy, vz)
	{ }
}
