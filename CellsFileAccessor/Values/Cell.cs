using furaku.Common.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>細胞</summary>
public class Cell : Particle<CellDynamics, CellOptics>
{
	/// <summary>ゲノム</summary>
	public virtual Genome Genome { get; private init; }

	private readonly CellConnection?[] _connections;
	/// <summary>結合</summary>
	public virtual IEnumerable<CellConnection?> Connections { get => _connections; }

	/// <summary>ニューラルネットワーク</summary>
	public virtual NeuralNetwork NeuralNetwork { get; private init; }

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
	/// <param name="doConnectionSpringProcess">バネの運動演算を行うか</param>
	/// <param name="fx">力X</param>
	/// <param name="fy">力Y</param>
	/// <param name="fz">力Z</param>
	/// <param name="mass">質量</param>
	/// <param name="radius">半径</param>
	/// <param name="cr">色赤</param>
	/// <param name="cg">色緑</param>
	/// <param name="cb">色青</param>
	/// <param name="lr">発光強度赤</param>
	/// <param name="lg">発光強度緑</param>
	/// <param name="lb">発光強度青</param>
	/// <param name="ar">吸収率赤</param>
	/// <param name="ag">吸収率緑</param>
	/// <param name="ab">吸収率青</param>
	/// <param name="book">ゲノム本</param>
	/// <param name="bookmark">栞</param>
	/// <param name="advanced_bookmar">拡張栞</param>
	/// <param name="waitingConnectionHands">接続待ちの結合手</param>
	/// <param name="isWaitingDisconnection">切断待ちか</param>
	/// <param name="idsOfConnectionCell">結合相手のID</param>
	/// <param name="springConstantsOfConnection">バネ定数</param>
	/// <param name="lengthOfConnection">バネ自然長</param>
	/// <param name="sprametersOfConnectionCell">パラメータS</param>
	/// <param name="isEnabledNeuralNetwork">ニューラルネットワーク有効か</param>
	/// <param name="inputs">入力値</param>
	/// <param name="outputs">出力値</param>
	/// <param name="inputWeights">入力側重み係数</param>
	/// <param name="outputWeights">出力側重み係数</param>
	/// <param name="isEatingTry">捕食試行中か</param>
	/// <param name="isTakingTry">取り込み試行中か</param>
	/// <param name="isEmittingTry">発光試行中か</param>
	public Cell(
		int id, bool isExist, long age, decimal alpha, decimal energy,
		bool doMechanicsProcess, bool doHitBlock, bool doHitCell,
		decimal x, decimal y, decimal z,
		int nx, int ny, int nz,
		decimal vx, decimal vy, decimal vz,
		bool doConnectionSpringProcess,
		decimal fx, decimal fy, decimal fz,
		decimal mass, decimal radius,
		decimal cr, decimal cg, decimal cb,
		decimal lr, decimal lg, decimal lb,
		decimal ar, decimal ag, decimal ab,
		char[] book, char[] bookmark, char[] advanced_bookmar,
		int? waitingConnectionHands, bool isWaitingDisconnection,
		int?[] idsOfConnectionCell, decimal[] springConstantsOfConnection, decimal[] lengthOfConnection, decimal[] sprametersOfConnectionCell,
		bool isEnabledNeuralNetwork, decimal[] inputs, decimal[] outputs, decimal[] inputWeights, decimal[] outputWeights,
		bool isEatingTry, bool isTakingTry, bool isEmittingTry)
		: base (id, isExist, age, alpha, energy,
			new(doMechanicsProcess, doHitBlock, doHitCell,
			x, y, z, nx, ny, nz, vx, vy, vz, doConnectionSpringProcess, fx, fy, fz, mass, radius),
			new(cr, cg, cb, lr, lg, lb, ar, ag, ab))
	{
		Genome = new(book, bookmark, advanced_bookmar, waitingConnectionHands, isWaitingDisconnection);
		_connections = ArrayUtility
			.Assemble((array1, array2, array3, array4) => array1 is null ? null :
			new CellConnection(array1.Value, array2, array3, array4),
				idsOfConnectionCell, springConstantsOfConnection, lengthOfConnection, sprametersOfConnectionCell).ToArray();
		NeuralNetwork = new(isEnabledNeuralNetwork, inputs, outputs, inputWeights, outputWeights, isEatingTry, isTakingTry, isEmittingTry);
	}

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
	/// <param name="doConnectionSpringProcess">バネの運動演算を行うか</param>
	/// <param name="fx">力X</param>
	/// <param name="fy">力Y</param>
	/// <param name="fz">力Z</param>
	/// <param name="mass">質量</param>
	/// <param name="radius">半径</param>
	/// <param name="cr">色赤</param>
	/// <param name="cg">色緑</param>
	/// <param name="cb">色青</param>
	/// <param name="lr">発光強度赤</param>
	/// <param name="lg">発光強度緑</param>
	/// <param name="lb">発光強度青</param>
	/// <param name="ar">吸収率赤</param>
	/// <param name="ag">吸収率緑</param>
	/// <param name="ab">吸収率青</param>
	/// <param name="book">ゲノム本</param>
	/// <param name="bookmark">栞</param>
	/// <param name="advanced_bookmar">拡張栞</param>
	/// <param name="waitingConnectionHands">接続待ちの結合手</param>
	/// <param name="isWaitingDisconnection">切断待ちか</param>
	/// <param name="connections">細胞結合</param>
	/// <param name="nuralNetwork">ニューラルネットワーク</param>
	public Cell(
		int id, bool isExist, long age, decimal alpha, decimal energy,
		bool doMechanicsProcess, bool doHitBlock, bool doHitCell,
		decimal x, decimal y, decimal z,
		int nx, int ny, int nz,
		decimal vx, decimal vy, decimal vz,
		bool doConnectionSpringProcess,
		decimal fx, decimal fy, decimal fz,
		decimal mass, decimal radius,
		decimal cr, decimal cg, decimal cb,
		decimal lr, decimal lg, decimal lb,
		decimal ar, decimal ag, decimal ab,
		char[] book, char[] bookmark, char[] advanced_bookmar,
		int? waitingConnectionHands, bool isWaitingDisconnection,
		CellConnection?[] connections, NeuralNetwork nuralNetwork)
		: base(id, isExist, age, alpha, energy,
			new(doMechanicsProcess, doHitBlock, doHitCell,
			x, y, z, nx, ny, nz, vx, vy, vz, doConnectionSpringProcess, fx, fy, fz, mass, radius),
			new(cr, cg, cb, lr, lg, lb, ar, ag, ab))
	{
		Genome = new(book, bookmark, advanced_bookmar, waitingConnectionHands, isWaitingDisconnection);
		_connections = connections;
		NeuralNetwork = nuralNetwork;
	}
}
