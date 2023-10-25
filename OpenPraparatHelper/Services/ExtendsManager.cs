using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace furaku.OpenPraparatHelper.Services;

/// <summary>拡張機能管理器</summary>
public class ExtendsManager
{
	private IExtend[]? _extends;
	/// <summary>拡張機能</summary>
	/// <exception cref="NullReferenceException">読み込み前にアクセス</exception>
	public virtual IExtend[] Extends
	{
		get => _extends!;
		private set => _extends = value;
	}

	/// <summary>読み込み</summary>
	/// <returns>拡張機能</returns>
	public virtual IEnumerable<IExtend> Load()
	{
		string[]? files = null;
		try
		{
			string pluginDirectory = Path.Combine(Application.StartupPath, "plugins");
			files = Directory.GetFiles(pluginDirectory).ToArray();
		}
		catch (IOException) { }
		catch (UnauthorizedAccessException) { }

		List<IExtend> extends = new();
		if (files is not null)
		{
			foreach (var pulginLibrary in files)
			{
				var fileExtends = Path.GetExtension(pulginLibrary);
				if (fileExtends is null
					|| fileExtends != ".cap")
				{
					continue;
				}

				Assembly assembly;
				try
				{
					assembly = Assembly.LoadFrom(pulginLibrary);

				}
				catch
				{
					continue;
				}

				foreach (var type in assembly.GetTypes())
				{
					if (type.GetInterface(nameof(IExtend)) is not null && type.IsSubclassOf(typeof(Control)))
					{
						var extend = (IExtend)Activator.CreateInstance(type)!;
						extends.Add(extend);
						yield return extend;
					}
				}
			}
		}
		this.Extends = extends.ToArray();
	}
}

/// <summary>拡張機能</summary>
public interface IExtend
{
	/// <summary>標準タブ名</summary>
	string DefaultTabName { get; }

	/// <summary>閉じたイベント</summary>
	void Closed();
}
