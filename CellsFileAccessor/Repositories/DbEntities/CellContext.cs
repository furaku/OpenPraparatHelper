using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories.DbEntities;

/// <summary>細胞のDBコンテキスト</summary>
public class CellContext : DbContext
{
	/// <summary>細胞</summary>
	public virtual DbSet<CellEntity> Cells { get; set; }

	/// <inheritdoc/>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		var connectionStringBuilder = new SqliteConnectionStringBuilder
		{
			DataSource = ":memory:"
		};
		optionsBuilder.UseSqlite(new SqliteConnection(connectionStringBuilder.ToString()));
	}
}
