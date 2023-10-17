using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories.DbEntities;

/// <summary>ニューラルネットワークエンティティ</summary>
[Table("t_nuralnetowork_value")]
[PrimaryKey(nameof(CellID), nameof(Index), nameof(IsOutput), nameof(IsWeight))]
public class NeuralNetworkValueEntity
{
	/// <summary>細胞ID</summary>
	[Column("cell_id")]
	public virtual int CellID { get; set; }

	/// <summary>インデックス</summary>
	[Column("indexa")]
	public virtual int Index { get; set; }

	/// <summary>出力か</summary>
	[Column("is_output")]
	public virtual bool IsOutput { get; set; }

	/// <summary>重み係数か</summary>
	[Column("is_weight")]
	public virtual bool IsWeight { get; set; }

	/// <summary>値</summary>
	[Column("value")]
	public virtual decimal Value { get; set; }

	/// <inheritdoc/>
	public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		var another = (NeuralNetworkValueEntity)obj;
		return this.CellID == another.CellID
			&& this.Index == another.Index
			&& this.IsOutput == another.IsOutput
			&& this.IsWeight == another.IsWeight;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		return this.CellID ^ this.Index * (this.IsOutput ? 1 : -1) * (this.IsWeight ? 1 : -1);
	}

}
