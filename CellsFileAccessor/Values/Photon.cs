using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>光子</summary>
public class Photon : Particle<PhotonDynamics, PhotonOptics>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="id">ID</param>
	/// <param name="isExist">存在するか</param>
	/// <param name="age">歳</param>
	/// <param name="alpha">α</param>
	/// <param name="energy">エネルギー</param>
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
	/// <param name="cr">色赤</param>
	/// <param name="cg">色緑</param>
	/// <param name="cb">色青</param>
	/// <param name="lr">発光強度赤</param>
	/// <param name="lg">発光強度緑</param>
	/// <param name="lb">発光強度青</param>
	public Photon(
		int id, bool isExist, long age, decimal alpha, decimal energy,
		bool doMechanicsProcess, bool doHitBlock, bool doHitCell,
		decimal x, decimal y, decimal z,
		int nx, int ny, int nz,
		decimal vx, decimal vy, decimal vz,
		decimal cr, decimal cg, decimal cb,
		decimal lr, decimal lg, decimal lb)
		: base(id, isExist, age, alpha, energy,
			new(doMechanicsProcess, doHitBlock, doHitCell,
			x, y, z, nx, ny, nz, vx, vy, vz),
			new(cr, cg, cb, lr, lg, lb))
	{ }

}
