using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories.DbEntities;

/// <summary>粒子エンティティ</summary>
public class ParticleEntity
{
	/// <summary>細胞ID</summary>
	[Column("cell_id")]
	public virtual int CellID { get; set; }

	/// <summary>歳</summary>
	[Column("age")]
	public virtual long Age { get; set; }

	/// <summary>α</summary>
	[Column("alpha")]
	public virtual decimal Alpha { get; set; }

	/// <summary>エネルギー</summary>
	[Column("energy")]
	public virtual decimal Energy { get; set; }

	/// <summary>物理演算を行うか</summary>
	[Column("do_mechanics_process")]
	public virtual bool DoMechanicsProcess { get; set; }

	/// <summary>ブロックとの衝突を行うか</summary>
	[Column("do_hit_block")]
	public virtual bool DoHitBlock { get; set; }

	/// <summary>他の細胞との衝突を行うか</summary>
	[Column("do_hit_cell")]
	public virtual bool DoHitCell { get; set; }

	/// <summary>位置X</summary>
	[Column("position_x")]
	public virtual decimal PositionX { get; set; }

	/// <summary>位置Y</summary>
	[Column("position_y")]
	public virtual decimal PositionY { get; set; }

	/// <summary>位置Z</summary>
	[Column("position_z")]
	public virtual decimal PositionZ { get; set; }

	/// <summary>（グリッド中）分割位置X</summary>
	[Column("position_of_division_x")]
	public virtual int PositionOfDivisionX { get; set; }

	/// <summary>（グリッド中）分割位置Y</summary>
	[Column("position_of_division_y")]
	public virtual int PositionOfDivisionY { get; set; }

	/// <summary>（グリッド中）分割位置Z</summary>
	[Column("position_of_division_z")]
	public virtual int PositionOfDivisionZ { get; set; }

	/// <summary>速度X</summary>
	[Column("velocity_x")]
	public virtual decimal VelocityX { get; set; }

	/// <summary>速度Y</summary>
	[Column("velocity_y")]
	public virtual decimal VelocityY { get; set; }

	/// <summary>速度Z</summary>
	[Column("velocity_z")]
	public virtual decimal VelocityZ { get; set; }

	/// <summary>赤色</summary>
	[Column("color_red")]
	public virtual decimal ColorRed { get; set; }

	/// <summary>緑色</summary>
	[Column("color_green")]
	public virtual decimal ColorGeen { get; set; }

	/// <summary>青色</summary>
	[Column("color_blue")]
	public virtual decimal ColorBlue { get; set; }

	/// <summary>光赤</summary>
	[Column("light_red")]
	public virtual decimal LigthRed { get; set; }

	/// <summary>光緑</summary>
	[Column("light_green")]
	public virtual decimal LigthGreen { get; set; }

	/// <summary>光青</summary>
	[Column("light_blue")]
	public virtual decimal LigthBlue { get; set; }

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		return this.CellID == ((ParticleEntity)obj).CellID;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return this.CellID;
	}
}
