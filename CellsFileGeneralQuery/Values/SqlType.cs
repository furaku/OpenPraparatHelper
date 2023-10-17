using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileGeneralQuery.Values;

/// <summary>SQL対応型</summary>
public class SqlType
{
	#region 静的メンバ
	private static readonly SqlType[] _instances;

	/// <summary>要素</summary>
	public static IEnumerable<SqlType> Elemnts => _instances.AsEnumerable();

	/// <summary>bool</summary>
	public static readonly SqlType BOOL;

	/// <summary>string</summary>
	public static readonly SqlType STRING;

	/// <summary>int</summary>
	public static readonly SqlType INTEGER;

	/// <summary>decimal</summary>
	public static readonly SqlType DECIMAL;

	/// <summary>double</summary>
	public static readonly SqlType DOUBLE;

	static SqlType()
	{
		BOOL = new SqlType(SqlTypes.BOOL, "bool");
		STRING = new SqlType(SqlTypes.STRING, "string");
		INTEGER = new SqlType(SqlTypes.INTEGER, "int");
		DECIMAL = new SqlType(SqlTypes.DECIMAL, "decimal");
		DOUBLE = new SqlType(SqlTypes.DOUBLE, "double");
		_instances = new SqlType[] { BOOL, STRING, INTEGER, DECIMAL, DOUBLE, };
	}
	#endregion

	/// <summary>型</summary>
	public virtual SqlTypes Type { get; private init; }

	/// <summary>名前</summary>
	public virtual string Name { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="type">型</param>
	/// <param name="name">名前</param>
	protected SqlType(SqlTypes type, string name)
	{
		this.Type = type;
		this.Name = name;
	}
}

/// <summary>SQL対応型</summary>
public enum SqlTypes
{
	/// <summary>bool</summary>
	BOOL,
	/// <summary>string</summary>
	STRING,
	/// <summary>int</summary>
	INTEGER,
	/// <summary>decinmal</summary>
	DECIMAL,
	/// <summary>double</summary>
	DOUBLE,
}
