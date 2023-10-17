using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Values;

/// <summary>ニューラルネットワーク</summary>
public class NeuralNetwork
{
	/// <summary>有効か</summary>
	public virtual bool IsEnabled { get; private init; }

	private readonly decimal[] _inputs;
	/// <summary>入力値</summary>
	public virtual IEnumerable<decimal> Inputs { get => _inputs; }

	private readonly decimal[] _outputs;
	/// <summary>出力値</summary>
	public virtual IEnumerable<decimal> Outputs { get => _outputs; }

	private readonly decimal[] _inputWeights;
	/// <summary>入力側重み係数</summary>
	public virtual IEnumerable<decimal> InputWeights { get => _inputWeights; }

	private readonly decimal[] _outputWeights;
	/// <summary>出力側重み係数</summary>
	public virtual IEnumerable<decimal> OutputWeights { get => _outputWeights; }

	/// <summary>捕食試行中か</summary>
	public virtual bool IsEatingTry { get; private init; }

	/// <summary>取り込み試行中か</summary>
	public virtual bool IsTakingTry { get; private init; }

	/// <summary>発光試行中か</summary>
	public virtual bool IsEmittingTry { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="isEnabled">有効か</param>
	/// <param name="inputs">入力値</param>
	/// <param name="outputs">出力値</param>
	/// <param name="inputWeights">入力側重み係数</param>
	/// <param name="outputWeights">出力側重み係数</param>
	/// <param name="isEatingTry">捕食試行中か</param>
	/// <param name="isTakingTry">取り込み試行中か</param>
	/// <param name="isEmittingTry">発光試行中か</param>
	public NeuralNetwork(bool isEnabled, decimal[] inputs, decimal[] outputs, decimal[] inputWeights, decimal[] outputWeights,
		bool isEatingTry, bool isTakingTry, bool isEmittingTry)
	{
		IsEnabled = isEnabled;
		_inputs = inputs;
		_outputs = outputs;
		_inputWeights = inputWeights;
		_outputWeights = outputWeights;
		IsEatingTry = isEatingTry;
		IsTakingTry = isTakingTry;
		IsEmittingTry = isEmittingTry;
	}
}
