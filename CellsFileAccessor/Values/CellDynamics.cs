using furaku.Common.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>細胞動力学</summary>
public class CellDynamics : ParticleDynamics
{
	/// <summary>バネの運動演算を行うか</summary>
	public virtual bool DoSpringProcess { get; private init; }

	/// <summary>力</summary>
	public virtual CartesianCoordinates Force { get; private init; }

	/// <summary>質量</summary>
	public virtual decimal Mass { get; private init; }

	/// <summary>半径</summary>
	public virtual decimal Radius { get; private init; }

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
	/// <param name="doSpringProcess">バネの運動演算を行うか</param>
	/// <param name="fx">力X</param>
	/// <param name="fy">力Y</param>
	/// <param name="fz">力Z</param>
	/// <param name="mass">質量</param>
	/// <param name="radius">半径</param>
	public CellDynamics(
		bool doMechanicsProcess, bool doHitBlock, bool doHitCell,
		decimal x, decimal y, decimal z,
		int nx, int ny ,int nz,
		decimal vx, decimal vy, decimal vz,
		bool doSpringProcess,
		decimal fx, decimal fy, decimal fz,
		decimal mass, decimal radius)
		: base(doMechanicsProcess, doHitBlock, doHitCell, x, y, z, nx, ny, nz, vx, vy, vz)
	{
		DoSpringProcess = doSpringProcess;
		Force = new(fx, fy, fz);
		Mass = mass;
		Radius = radius;
	}
}
