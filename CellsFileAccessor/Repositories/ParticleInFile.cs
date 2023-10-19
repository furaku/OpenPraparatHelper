using furaku.CellsFileAccessorLib.Services;
using furaku.CellsFileAccessorLib.Values;
using furaku.Common.Services;
using furaku.Common.Values;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace furaku.CellsFileAccessorLib.Repositories;

/// <summary>>粒子リポジトリ（ファイル）</summary>
public class ParticleInFile : Repository<IParticle>
{
	#region 静的メンバ
	/// <summary>ファイル内容不正メッセージ</summary>
	protected const string InvalidFileContentMessage = "ファイル内容が異常です";

	/// <summary>数値行の分割用正規表現</summary>
	protected static readonly Regex RegexFoNumbers;

	/// <summary>静的コンストラクタ</summary>
	static ParticleInFile()
	{
		RegexFoNumbers = new("\\s+");
	}

	/// <summary>値(整数)を取得</summary>
	/// <param name="line">１行の文字</param>
	/// <param name="count">値の数。負数は任意</param>
	/// <returns>値</returns>
	/// <exception cref="InvalidFileContentException">ファイル内容不正</exception>
	protected static long[] GetIntValues(string line, int count)
	{
		var values = RegexFoNumbers.Split(line).Where(elem => !string.IsNullOrWhiteSpace(elem));
		if (count >= 0 && values.Count() != count)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}

		return values.Select(elem =>
		{
			if (!long.TryParse(elem, out var value))
			{
				throw new InvalidFileContentException(InvalidFileContentMessage);
			}
			return value;
		}).ToArray();
	}

	/// <summary>値(decimal)を取得</summary>
	/// <param name="line">１行の文字</param>
	/// <param name="count">値の数。負数は任意</param>
	/// <returns>値</returns>
	/// <exception cref="InvalidFileContentException">ファイル内容不正</exception>
	protected static decimal[] GetDecimalValues(string line, int count)
	{
		var values = RegexFoNumbers.Split(line).Where(elem => !string.IsNullOrWhiteSpace(elem));
		if (count >= 0 && values.Count() != count)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}

		return values.Select(elem =>
		{
			if (!decimal.TryParse(elem,
				NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
				null, out var value))
			{
				throw new InvalidFileContentException(InvalidFileContentMessage);
			}
			return value;
		}).ToArray();
	}

	/// <summary>文字列を取得</summary>
	/// <param name="line">１行の文字</param>
	/// <returns>値</returns>
	/// <exception cref="InvalidFileContentException">ファイル内容不正</exception>
	protected static char[] GetString(string line)
	{
		// TODO 文字検査
		var values = RegexFoNumbers.Split(line).Where(elem => !string.IsNullOrWhiteSpace(elem)).ToArray();
		if (values.Length != 1)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}

		if (!values[0].StartsWith('!'))
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}

		return values[0][1..].ToCharArray();
	}

	/// <summary>空を取得</summary>
	/// <param name="_">１行の文字</param>
	protected static void GetNull(string _) { }
	#endregion

	/// <summary>ファイルパス</summary>
	public virtual string FilePath { get; private init; }

	/// <summary>コンストラクタ</summary>
	/// <param name="filePath">ファイルパス</param>
	public ParticleInFile(string filePath)
	{
		this.FilePath = filePath;
	}

	/// <inheritdoc/>
	/// <exception cref="OperationCanceledException">キャンセル例外</exception>
	/// <exception cref="FailToFileAccessException">ファイルアクセスに失敗</exception>
	/// <exception cref="InvalidFileContentException">ファイル内容不正</exception>
	public override IEnumerable<IParticle> Find(
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null,
		params int[] elements_ids)
	{
		StreamReader? sr = null;
		try
		{
			List<int>? ids = elements_ids.Length > 0 ? elements_ids.ToList() : null;
			IParticle? particle = null;
			try
			{
				cancellationToken?.ThrowIfCancellationRequested();
				processMessageEnqueueMethod?.Invoke(new StartFindParticleFromFile(this));
				sr = new StreamReader(this.FilePath);
				if (sr.ReadLine() is null)
				{
					throw new ArgumentException(InvalidFileContentMessage);
				}
				cancellationToken?.ThrowIfCancellationRequested();
				particle = ReadParticle(sr);
			}
			catch (IOException ex)
			{
				throw new FailToFileAccessException(ex.Message, ex);
			}

			while (particle is not null)
			{
				processMessageEnqueueMethod?.Invoke(new ReadParticleFromFile(this, particle));
				if (ids is not null)
				{
					if (ids.Contains(particle.ID))
					{
						if (particle.IsExist && particle is Cell)
						{
							yield return particle;
						}
						else
						{
							processMessageEnqueueMethod?.Invoke(new NotExistsCell(this, particle));
						}
						ids.Remove(particle.ID);
						if (ids.Count <= 0)
						{
							break;
						}
					}
				}
				else
				{
					if (particle.IsExist && particle is Cell)
					{
						yield return particle;
					}
				}
				try
				{
					cancellationToken?.ThrowIfCancellationRequested();
					particle = ReadParticle(sr);
				}
				catch (IOException ex)
				{
					throw new FailToFileAccessException(ex.Message, ex);
				}
			}
		}
		finally
		{
			sr?.Dispose();
		}
		processMessageEnqueueMethod?.Invoke(new EndFindParticleFromFile(this));
	}

	/// <inheritdoc/>
	[Obsolete("未実装です。", true)]
#pragma warning disable CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
	public override IEnumerable<IParticle> Find(
#pragma warning restore CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	[Obsolete("未実装です。", true)]
#pragma warning disable CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
	public override IEnumerable<R> Query<R>(
#pragma warning restore CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
		string sql,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null,
		params object[] parameters)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	[Obsolete("未実装です。", true)]
#pragma warning disable CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
	public override void Marge(
#pragma warning restore CS0809 // 旧形式のメンバーが、旧形式でないメンバーをオーバーライドします
		IEnumerable<IParticle> elemnets,
		CancellationToken? cancellationToken = null,
		Action<IProcessMessage>? processMessageEnqueueMethod = null)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// 粒子を読み込み
	/// </summary>
	/// <param name="sr">StreamReader</param>
	/// <returns>粒子</returns>
	/// <exception cref="IOException">ファイルアクセス失敗</exception>
	/// <exception cref="InvalidFileContentException">ファイル内容不正</exception>
	protected virtual IParticle? ReadParticle(StreamReader sr)
	{
		long[] invValues;
		decimal[] desimalValues;
		var line = sr.ReadLine();
		if (line is null)
		{
			return null;
		}
		var isExist = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var book = GetString(line);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var bookmark = GetString(line);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var bookmarker_advance = GetString(line);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 3);
		var x = desimalValues[0];
		var y = desimalValues[1];
		var z = desimalValues[2];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		invValues = GetIntValues(line, 3);
		var nx = (int)desimalValues[0];
		var ny = (int)desimalValues[1];
		var nz = (int)desimalValues[2];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 3);
		var vx = desimalValues[0];
		var vy = desimalValues[1];
		var vz = desimalValues[2];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 3);
		var fx = desimalValues[0];
		var fy = desimalValues[1];
		var fz = desimalValues[2];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 3);
		var cr = desimalValues[0];
		var cg = desimalValues[1];
		var cb = desimalValues[2];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 3);
		var ar = desimalValues[0];
		var ag = desimalValues[1];
		var ab = desimalValues[2];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var ir = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var ig = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var ib = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var mass = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var radius = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var energy = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		desimalValues = GetDecimalValues(line, 1);
		var alpha = desimalValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		invValues = GetIntValues(line, 1);
		var id = (int)invValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		invValues = GetIntValues(line, 1);
		var age = invValues[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] inputs = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] outputs = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] inputWeights = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] outputWeights = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] springConstantsOfConnection = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] lengthOfConnection = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		decimal[] sprametersOfConnectionCell = GetDecimalValues(line, -1);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		GetNull(line);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		int[] idsOfConnectionCell = GetIntValues(line, -1).Select(elem => (int)elem).ToArray();

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		GetNull(line);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var doHitBlock = GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var doHitCell = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var doConnectionSpringProcess = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var doMechanicsProcess = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var isEatingTry = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var isTakingTry = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var isEmittingTry = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var isLight = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var isEnabledNeuralNetwork = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		GetNull(line);

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var waitingConnectionHands = (int)GetIntValues(line, 1)[0];

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		var isWaitingDisconnection = (int)GetIntValues(line, 1)[0] != 0;

		line = sr.ReadLine();
		if (line is null)
		{
			throw new InvalidFileContentException(InvalidFileContentMessage);
		}
		GetNull(line);

		if (isLight)
		{
			return new Photon(id, isExist, age, alpha, energy, doMechanicsProcess, doHitBlock, doHitCell,
				x, y, z, nx, ny, nz, vx, vy, vz, cr, cg, cb, ir, ig, ib);
		}
		return new Cell(id, isExist, age, alpha, energy, doMechanicsProcess, doHitBlock, doHitCell, x, y, z, nx, ny, nz, vx, vy, vz,
			doConnectionSpringProcess, fx, fy, fz, mass, radius, cr, cg, cb, ir, ig, ib, ar, ag, ab, book, bookmark, bookmarker_advance,
			waitingConnectionHands, isWaitingDisconnection,
			ArrayUtility
			.Assemble((array1, array2, array3, array4) => array1 == 0 ? null :
			new CellConnection(array1, array2, array3, array4),
				idsOfConnectionCell, springConstantsOfConnection, lengthOfConnection, sprametersOfConnectionCell).ToArray(),
			new NeuralNetwork(isEnabledNeuralNetwork, inputs, outputs, inputWeights, outputWeights, isEatingTry, isTakingTry, isEmittingTry));
	}
}

#region 例外
/// <summary>ファイルアクセス失敗例外</summary>
[Serializable]
public class FailToFileAccessException : Exception
{
	/// <inheritdoc />
	public FailToFileAccessException() { }
	/// <inheritdoc />
	public FailToFileAccessException(string message) : base(message) { }
	/// <inheritdoc />
	public FailToFileAccessException(string message, Exception inner) : base(message, inner) { }
	/// <inheritdoc />
	protected FailToFileAccessException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

/// <summary>ファイル内容不正例外</summary>
[Serializable]
public class InvalidFileContentException : Exception
{
	/// <inheritdoc />
	public InvalidFileContentException() { }
	/// <inheritdoc />
	public InvalidFileContentException(string message) : base(message) { }
	/// <inheritdoc />
	public InvalidFileContentException(string message, Exception inner) : base(message, inner) { }
	/// <inheritdoc />
	protected InvalidFileContentException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
#endregion

#region 工程メッセージ
/// <summary>ファイルからの粒子検索開始</summary>
public class StartFindParticleFromFile : ProcessMassage<ParticleInFile, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public StartFindParticleFromFile(ParticleInFile sender) : base(sender, null) { }
}

/// <summary>ファイルからの粒子検索終了</summary>
public class EndFindParticleFromFile : ProcessMassage<ParticleInFile, object?>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	public EndFindParticleFromFile(ParticleInFile sender) : base(sender, null) { }
}

/// <summary>ファイルからの粒子読み込み</summary>
public class ReadParticleFromFile : ProcessMassage<ParticleInFile, IParticle>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="particle">粒子</param>
	public ReadParticleFromFile(ParticleInFile sender, IParticle particle) : base(sender, particle) { }
}

/// <summary>存在する細胞でない</summary>
public class NotExistsCell : ProcessMassage<ParticleInFile, IParticle>
{
	/// <summary>コンストラクタ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="particle">粒子</param>
	public NotExistsCell(ParticleInFile sender, IParticle particle) : base(sender, particle) { }
}
#endregion
