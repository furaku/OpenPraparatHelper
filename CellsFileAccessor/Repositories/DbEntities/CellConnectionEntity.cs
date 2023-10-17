using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories.DbEntities;

/// <summary>細胞結合エンティティ</summary>
[Table("t_connection")]
[PrimaryKey(nameof(CellID), nameof(HandID))]
[Index(nameof(CellID), nameof(TargetCellID), IsUnique = true)]
public class CellConnectionEntity
{
	/// <summary>自身の細胞ID</summary>
	[Column("cell_id")]
	public virtual int CellID { get; set; }

	/// <summary>自身の結合手</summary>
	[Column("hand_id")]
	public virtual int HandID { get; set; }

	/// <summary>結合相手のID</summary>
	[Column("target_cell_id")]
	public virtual int TargetCellID { get; set; }

	/// <summary>バネ定数</summary>
	[Column("spring_constant")]
	public virtual decimal SpringConstant { get; set; }

	/// <summary>バネ自然長</summary>
	[Column("length")]
	public virtual decimal Length { get; set; }

	/// <summary>パラメータS</summary>
	[Column("s")]
	public virtual decimal S { get; set; }

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		var another = (CellConnectionEntity)obj;
		return this.CellID == another.CellID && this.HandID == another.HandID;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return this.CellID ^ this.HandID;
	}
}
