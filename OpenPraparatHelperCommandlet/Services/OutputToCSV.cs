using System.DirectoryServices;
using System.Management.Automation;

namespace furaku.OpenPraparatHelperCommandlet.Services;

/// <summary>OpenPraparatの出力をファイルに保存する</summary>
[Cmdlet("Export", "OpenPraparat")]
public class OutputToCSV : Cmdlet
{
	/// <summary>パイプライン入力</summary>
	[Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
	public virtual string[]? Inputs { get; set; }

	[Parameter(Position = 0)]
	public virtual string? Log { get; set; }

	[Parameter(Position = 1)]
	public virtual string? Csv { get; set; }

	/// <inheritdoc/>
	protected override void ProcessRecord()
	{
		StreamWriter? log = null;
		OpenPraparatOutputCSVWriter? csv = null;
		try
		{
			if (Log is not null)
			{
				log = new StreamWriter(Log);
			}
			if (Csv is not null)
			{
				csv = new OpenPraparatOutputCSVWriter(Csv);
			}
			foreach (var input in Inputs!)
			{
				log?.WriteLine(input);
				csv?.WriteLine(input);
				WriteObject(input, true);
			}
		}
		finally
		{
			log?.Dispose();
			csv?.Dispose();
		}
	}
}