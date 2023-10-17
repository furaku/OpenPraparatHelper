using furaku.Common.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>粒子動力学</summary>
public abstract class ParticleDynamics
{
	/// <summary>物理演算を行うか</summary>
	public virtual bool DoMechanicsProcess { get; private init; }

	/// <summary>ブロックとの衝突を行うか</summary>
	public virtual bool DoHitBlock { get; private init; }

	/// <summary>他の細胞との衝突を行うか</summary>
	public virtual bool DoHitCell { get; private init; }

	/// <summary>位置</summary>
	public virtual CartesianCoordinates Position { get; private init; }

	/// <summary>（グリッド中）分割位置</summary>
	public virtual CartesianCoordinates PositionOfDivision { get; private init; }

	/// <summary>速度</summary>
	public virtual CartesianCoordinates Velocity { get; private init; }

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
	protected ParticleDynamics(
		bool doMechanicsProcess, bool doHitBlock, bool doHitCell,
		decimal x, decimal y, decimal z,
		int nx, int ny, int nz,
		decimal vx, decimal vy, decimal vz)
	{
		DoMechanicsProcess = doMechanicsProcess;
		DoHitBlock = doHitBlock;
		DoHitCell = doHitCell;
		Position = new(x, y, z);
		PositionOfDivision = new(nx, ny, nz);
		Velocity = new(vx, vy, vz);
	}
}
