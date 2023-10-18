using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories.DbEntities;

/// <summary>細胞エンティティ</summary>
[Table("t_cell")]
[PrimaryKey(nameof(CellID))]
public class CellEntity : ParticleEntity
{
	/// <summary>バネの運動演算を行うか</summary>
	[Column("do_spring_process")]
	public virtual bool DoSpringProcess { get; set; }

	/// <summary>力X</summary>
	[Column("force_x")]
	public virtual decimal ForceX { get; set; }

	/// <summary>力Y</summary>
	[Column("force_y")]
	public virtual decimal ForceY { get; set; }

	/// <summary>力Z</summary>
	[Column("force_z")]
	public virtual decimal ForceZ { get; set; }

	/// <summary>質量</summary>
	[Column("mass")]
	public virtual decimal Mass { get; set; }

	/// <summary>半径</summary>
	[Column("radius")]
	public virtual decimal Radius { get; set; }

	/// <summary>光赤吸収率赤色</summary>
	[Column("alpha_channel_red")]
	public virtual decimal AlphaChannelRed { get; set; }

	/// <summary>光緑吸収率緑色</summary>
	[Column("alpha_channel_green")]
	public virtual decimal AlphaChannelGreen { get; set; }

	/// <summary>光青吸収率青色</summary>
	[Column("alpha_channel_blue")]
	public virtual decimal AlphaChannelBlue { get; set; }

	/// <summary>本</summary>
	[Column("book")]
	public virtual string Book { get; set; }

	/// <summary>栞</summary>
	[Column("bookmark")]
	public virtual string BookMark { get; set; }

	/// <summary>拡張栞</summary>
	[Column("advanced_bookmark")]
	public virtual string AdvancedBookMark { get; set; }

	/// <summary>接続待ちの結合手</summary>
	[Column("waiting_connection_hands")]
	public virtual int? WaitingConnectionHands { get; set; }

	/// <summary>切断待ちか</summary>
	[Column("is_waiting_disconnection")]
	public virtual bool IsWaitingDisconnection { get; set; }

	/// <summary>有効か</summary>
	[Column("is_enabled_neuralnetwork")]
	public virtual bool IsEnabledNeuralNetwork { get; set; }

	/// <summary>捕食試行中か</summary>
	[Column("is_eatingtry")]
	public virtual bool IsEatingTry { get; set; }

	/// <summary>取り込み試行中か</summary>
	[Column("is_takingtry")]
	public virtual bool IsTakingTry { get; set; }

	/// <summary>発光試行中か</summary>
	[Column("is_emittingtry")]
	public virtual bool IsEmittingTry { get; set; }

	/// <summary>細胞結合手数</summary>
	[Column("connection_count")]
	public virtual int ConnectionCount { get; set; }

	/// <summary>細胞結合</summary>
	public virtual ICollection<CellConnectionEntity> Connections { get; private init; }

	/// <summary>ニューラルネットワークの値</summary>
	public virtual ICollection<NeuralNetworkValueEntity> NeuralNetworkValues { get; private init; }

	/// <summary>コンストラクタ</summary>
	public CellEntity()
	{
		Book = string.Empty;
		BookMark = string.Empty;
		AdvancedBookMark = string.Empty;
		Connections = new List<CellConnectionEntity>();
		NeuralNetworkValues = new List<NeuralNetworkValueEntity>();
	}
}
