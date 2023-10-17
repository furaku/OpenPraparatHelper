using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furaku.OpenPraparatHelperCommandlet.Services;

public class OpenPraparatOutputCSVWriter : IDisposable
{
	protected virtual StreamWriter Writer { get; }

	protected virtual bool IsProcessing { get; set; }

	private bool disposedValue;

	public OpenPraparatOutputCSVWriter(string path)
	{
		this.Writer = new StreamWriter(path);
		IsProcessing = false;
	}

	public virtual void WriteLine(string line)
	{
		if (this.IsProcessing)
		{
			if (line.Trim() == "#####################################################")
			{
				this.Writer.WriteLine();
				this.IsProcessing = false;
			}
			else
			{
				var parameter = line.Split('=');
				if (parameter.Length == 2)
				{
					var label = parameter[0].Trim();
					var val = parameter[1].Trim();
					switch (label)
					{
						case "WORLD_STEP":
							this.Write(val, 1);
							break;
						case "CALC_CLL":
							this.Write(val, 1);
							break;
						case "NOT_CALC_CLL":
							this.Write(val, 1);
							break;
						case "NUMBER_OF_INFO_TRANS":
							this.Write(val, 1);
							break;
						case "CALC_CLL-INFO_TRANS":
							this.Write(val, 1);
							break;
						case "CALC_CLL+NOT_CALC_CLL":
							this.Write(val, 1);
							break;
						case "CONN_CLL":
							this.Write(val, 1);
							break;
						case "EAT_COUNT*":
							this.Write(val, 1);
							break;
						case "EAT_INFO*":
							this.Write(val, 9);
							break;
						case "EATEN_INFO*":
							this.Write(val, 9);
							break;
						case "FUSION_COUNT*":
							this.Write(val, 1);
							break;
						case "EXPANSION_COUNT*":
							this.Write(val, 1);
							break;
						case "CONNECT_COUNT*":
							this.Write(val, 1);
							break;
						case "DISCONNECT_COUNT*":
							this.Write(val, 1);
							break;
						case "TURN_BOOKMARKER_COUNT*":
							this.Write(val, 1);
							break;
						case "MUTATION_COUNT*":
							this.Write(val, 1);
							break;
						case "DEATHS_COUNT*":
							this.Write(val, 1);
							break;
						case "CELLS(SUN_ID)%LIGHT_AF":
							this.Write(val, 3);
							break;
						case "sun_theta":
							this.Write(val, 1);
							break;
						case "TOT_ENERGY*":
							this.Write(val, 1);
							break;
						case "AVE_ENERGY*":
							this.Write(val, 1);
							break;
						case "EVERY_STEP_COST_A":
							this.Write(val, 1);
							break;
						case "CHANGE_L_COUNT*":
							this.Write(val, 1);
							break;
						case "ENERGY_TRANSIT_COUNT*":
							this.Write(val, 1);
							break;
						case "CENTER_OF_GRAV":
							this.Write(val, 3);
							break;
						case "AGE INFO FOR CELL":
							this.Write(val, 4);
							break;
						case "AGE INFO FOR IT":
							this.Write(val, 4);
							break;
						case "TOT_M*":
							this.Write(val, 1);
							break;
						case "ALONE_COUNT*":
							this.Write(val, 1, false);
							break;
					}
				}
			}
		}
		else
		{
			if (line.Trim() == "#####################################################")
			{
				this.IsProcessing = true;
			}
		}
	}

	protected virtual void Write(string val, int count, bool addComma = true)
	{
		var vals = val.Split(" ", StringSplitOptions.RemoveEmptyEntries);
		for (var i = 0; i < count; i++)
		{
			if (vals.Length > i)
			{
				this.Writer.Write(vals[i]);
			}
			if (i != count - 1 || addComma)
			{
				this.Writer.Write(",");
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				Writer.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
