using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>粒子</summary>
public abstract class Particle<D, O> : IParticle where D : ParticleDynamics where O : ParticleOptics
{
	/// <inheritdoc/>
	public virtual int ID { get; private init; }

	/// <inheritdoc/>
	public virtual bool IsExist { get; private init; }

	/// <inheritdoc/>
	public virtual long Age { get; private init; }

	/// <inheritdoc/>
	public virtual decimal Alpha { get; private init; }

	/// <inheritdoc/>
	public virtual decimal Energy { get; private init; }

	/// <summary>動力学</summary>
	public virtual D Dynamics { get; private init; }
	ParticleDynamics IParticle.Dynamics { get => this.Dynamics; }

	/// <summary>光学</summary>
	public virtual O Optics { get; private init; }
	ParticleOptics IParticle.Optics { get => this.Optics; }

	/// <summary>コンストラクタ</summary>
	/// <param name="id">ID</param>
	/// <param name="isExists">存在するか</param>
	/// <param name="age">歳</param>
	/// <param name="alpha">α</param>
	/// <param name="energy">エネルギー</param>
	/// <param name="dynamics">動力学</param>
	/// <param name="optics">光学</param>
	protected Particle(int id, bool isExists, long age, decimal alpha, decimal energy, D dynamics, O optics)
	{
		ID = id;
		IsExist = isExists;
		Age = age;
		Alpha = alpha;
		Energy = energy;
		Dynamics = dynamics;
		Optics = optics;
	}
}

/// <summary>粒子インタフェース</summary>
public interface IParticle
{
	/// <summary>ID</summary>
	int ID { get; }

	/// <summary>存在するか</summary>
	bool IsExist { get; }

	/// <summary>歳</summary>
	long Age { get; }

	/// <summary>α</summary>
	decimal Alpha { get; }

	/// <summary>エネルギー</summary>
	decimal Energy { get; }

	/// <summary>動力学</summary>
	ParticleDynamics Dynamics { get; }

	/// <summary>光学</summary>
	ParticleOptics Optics { get; }
}
