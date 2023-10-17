using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.Common.Values;

/// <summary>三次元ベクトル</summary>
/// <typeparam name="T">精度型</typeparam>
public abstract class Vector3<T>
{
	/// <summary>E1</summary>
	protected virtual T E1 { get; private init; }

	/// <summary>E2</summary>
	protected virtual T E2 { get; private init; }

	/// <summary>E3</summary>
	protected virtual T E3 { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="e1">e1</param>
	/// <param name="e2">e2</param>
	/// <param name="e3">e3</param>
	protected Vector3(T e1, T e2, T e3)
	{
		E1 = e1;
		E2 = e2;
		E3 = e3;
	}
}

/// <summary>直交座標</summary>
public class CartesianCoordinates : Vector3<decimal>
{
	/// <summary>X</summary>
	public virtual decimal X { get => E1; }

	/// <summary>Y</summary>
	public virtual decimal Y { get => E2; }

	/// <summary>Z</summary>
	public virtual decimal Z { get => E3; }

	/// <summary>コンストラクタ</summary>
	/// <param name="x">X</param>
	/// <param name="y">Y</param>
	/// <param name="z">Z</param>
	public CartesianCoordinates(decimal x, decimal y, decimal z) : base(x, y, z) { }
}

/// <summary>RGB色</summary>
public class RgbColor : Vector3<decimal>
{
	/// <summary>赤</summary>
	public virtual decimal Red { get => E1; }

	/// <summary>緑</summary>
	public virtual decimal Green { get => E2; }

	/// <summary>青</summary>
	public virtual decimal Blue { get => E3; }

	/// <summary>コンストラクタ</summary>
	/// <param name="red">赤</param>
	/// <param name="green">緑</param>
	/// <param name="blue">青</param>
	public RgbColor(decimal red, decimal green, decimal blue) : base(red, green, blue) { }
}
