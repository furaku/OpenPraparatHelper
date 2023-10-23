using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace furaku.Common.Views;

/// <summary>コンソールコントロール</summary>
public partial class ConsoleControl : UserControl
{
	/// <summary>クライアント領域までのサイズ</summary>
	protected const int ClientMargin = 23;

	/// <summary>最大行</summary>
	public virtual int MaxLines { get; set; }

	/// <summary>選択中の色</summary>
	public virtual Color SelectedColor { get; set; }

	/// <summary>テキスト描画用ブラシ</summary>
	protected virtual SolidBrush TextBrush { get; }

	/// <summary>行</summary>
	protected virtual List<(string Text, Color Color)> Lines { get; }

	/// <summary>キー押下種別</summary>
	protected virtual PressedKey Pressed { get; set; }

	/// <summary>選択文字のインデックス</summary>
	protected virtual int SelectedIndex { get; set; }

	/// <summary>選択文字の長さ</summary>
	protected virtual int SelectedLength { get; set; }

	/// <summary>文字列フォーマット</summary>
	protected virtual StringFormat StringFormat { get; }

	/// <summary>コンストラクタ</summary>
	public ConsoleControl()
	{
		InitializeComponent();
		this.Pressed = PressedKey.NONE;
		this.TextBrush = new(this.ForeColor);
		this.Lines = new();
		this.hScrollBar.Enabled = false;
		this.StringFormat = new(StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces)
		{
			Alignment = StringAlignment.Near,
		};
	}

	/// <summary>行を追加</summary>
	/// <param name="lines">行</param>
	public virtual void AppendLine(params (string, Color)[] lines)
	{
		if (lines.Length > 0)
		{
			if ((this.Lines.Count > 0) && (this.Lines.Count + lines.Length > this.MaxLines))
			{
				this.Lines.RemoveRange(0, Math.Max(this.Lines.Count + lines.Length - this.MaxLines, this.Lines.Count));
			}
			this.Lines.AddRange(lines.Take(this.MaxLines));
			this.CalculationScroll();
			this.vScrollBar.Value = Math.Max(this.vScrollBar.Maximum - this.vScrollBar.LargeChange, 0);
			this.Invalidate();
		}
	}

	/// <inheritdoc/>
	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		var startRow = this.vScrollBar.Value / this.FontHeight;
		var endRow = Math.Min((this.vScrollBar.Value + this.vScrollBar.LargeChange) / this.FontHeight + 1, this.Lines.Count - 1);
		var index = this.Lines.Take(startRow).Sum(elem => elem.Text.Length);
		for (var row = startRow; row <= endRow; row++)
		{
			(var text, var color) = this.Lines[row];
			if (color == Color.Empty)
			{
				color = this.ForeColor;
			}
			float x = -this.hScrollBar.Value;
			int y = row * this.FontHeight - this.vScrollBar.Value;
			if (((index + text.Length) > this.SelectedIndex) && (index < (this.SelectedIndex + this.SelectedLength)))
			{
				var start = index > this.SelectedIndex ? 0 : this.SelectedIndex - index;
				var end = ((index + text.Length) > (this.SelectedIndex + this.SelectedLength)) ? (this.SelectedIndex + this.SelectedLength - index) : text.Length;
				if (start > 0)
				{
					this.TextBrush.Color = color;
					e.Graphics.DrawString(text[..start], this.Font, this.TextBrush, x, y, this.StringFormat);
					x += this.MeasureString(e.Graphics, text[..start]).Sum();
				}
				this.TextBrush.Color = Color.FromArgb(0xFF - color.R, 0xFF - color.G, 0xFF - color.B);
				e.Graphics.FillRectangle(new SolidBrush(this.SelectedColor), x + 2, y, this.MeasureString(e.Graphics, text[start..end]).Sum(), this.FontHeight);
				e.Graphics.DrawString(text[start..end], this.Font, this.TextBrush, x, y, this.StringFormat);
				x += this.MeasureString(e.Graphics, text[start..end]).Sum();
				if (end < text.Length)
				{
					this.TextBrush.Color = color;
					e.Graphics.DrawString(text[end..], this.Font, this.TextBrush, x, y, this.StringFormat);
				}
			}
			else
			{
				this.TextBrush.Color = color;
				e.Graphics.DrawString(text, this.Font, this.TextBrush, -this.hScrollBar.Value, y, this.StringFormat);
			}
			index += this.Lines[row].Text.Length;
		}
	}

	/// <inheritdoc/>
	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);

		if (this.Lines is not null)
		{
			this.vScrollBar.LargeChange = Math.Max(this.Height - ClientMargin, 0);
			this.hScrollBar.LargeChange = Math.Max(this.Width - ClientMargin, 0);
			this.CalculationScroll();
		}
	}

	/// <inheritdoc/>
	protected override void OnMouseWheel(MouseEventArgs e)
	{
		base.OnMouseWheel(e);

		if (this.vScrollBar.Enabled)
		{
			var newValue = this.vScrollBar.Value - e.Delta;
			newValue = Math.Max(newValue, 0);
			newValue = Math.Min(newValue, this.vScrollBar.Maximum - this.vScrollBar.LargeChange);
			if (this.vScrollBar.Value != newValue)
			{
				this.vScrollBar.Value = newValue;
				this.Invalidate();
			}
		}
	}

	/// <inheritdoc/>
	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e);

		if (e.Control)
		{
			if (e.KeyCode == Keys.A)
			{
				this.Pressed = PressedKey.SELECT_ALL;
			}
			else if (e.KeyCode == Keys.C)
			{
				this.Pressed = PressedKey.COPY;
			}
		}
	}

	/// <inheritdoc/>
	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		base.OnKeyPress(e);

		switch (this.Pressed)
		{
			case PressedKey.NONE:
				break;
			case PressedKey.SELECT_ALL:
				this.Pressed = PressedKey.NONE;
				this.SelectedIndex = 0;
				this.SelectedLength = this.Lines.Sum(elem => elem.Text.Length);
				e.Handled = true;
				break;
			case PressedKey.COPY:
				this.Pressed = PressedKey.NONE;
				var index = 0;
				StringBuilder sb = new();
				foreach ((var text, _) in this.Lines)
				{
					if (index + text.Length > this.SelectedIndex)
					{
						var start = index > this.SelectedIndex ? 0 : this.SelectedIndex - index;
						var end = (((index + text.Length) > (this.SelectedIndex + this.SelectedLength)) ? (this.SelectedIndex + this.SelectedLength - index) : text.Length) - start;
						sb.Append(text.AsSpan(start, end));
						if ((index + text.Length) > (this.SelectedIndex + this.SelectedLength))
						{
							break;
						}
						sb.AppendLine();
					}
					index += text.Length;
				}
				if (sb.Length > 0)
				{
					Clipboard.SetText(sb.ToString());
				}
				e.Handled = true;
				break;
		}
	}

	/// <inheritdoc/>
	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);

		this.Capture = true;
		this.SelectedIndex = this.PointToCharIndex(e.Location);
	}

	/// <inheritdoc/>
	protected override void OnMouseUp(MouseEventArgs e)
	{
		base.OnMouseUp(e);

		if (this.Capture)
		{
			this.Capture = false;
			var end = this.PointToCharIndex(e.Location);
			if (end >= this.SelectedIndex)
			{
				this.SelectedLength = end - this.SelectedIndex;
			}
			else
			{
				this.SelectedLength = this.SelectedIndex - end;
				this.SelectedIndex -= this.SelectedLength;
			}
			this.Invalidate();
		}
	}

	/// <summary>スクロールイベントハンドラ</summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント引数</param>
	protected virtual void ScrollBar_Scroll(object sender, ScrollEventArgs e)
	{
		if (e.NewValue != e.OldValue)
		{
			this.Invalidate();
		}
	}

	/// <summary>スクロールバー状態を計算</summary>
	protected virtual void CalculationScroll()
	{
		var height = this.Lines.Count * this.FontHeight;
		if (height <= this.vScrollBar.LargeChange)
		{
			this.vScrollBar.Enabled = false;
		}
		else
		{
			this.vScrollBar.Maximum = height;
			this.vScrollBar.Enabled = true;
		}

		using var g = this.CreateGraphics();
		var width = this.Lines.Count > 0 ? (int)this.Lines.Max(elem => this.MeasureString(g, elem.Text).Sum()) : 0;
		if (width <= this.hScrollBar.LargeChange)
		{
			this.hScrollBar.Enabled = false;
		}
		else
		{
			this.hScrollBar.Maximum = width;
			this.hScrollBar.Enabled = true;
		}
	}

	/// <summary>位置から文字のインデクスを算出</summary>
	/// <param name="point">位置</param>
	/// <returns>インデックス</returns>
	protected virtual int PointToCharIndex(Point point)
	{
		var row = Math.Min((point.Y + vScrollBar.Value) / this.FontHeight, this.Lines.Count - 1);
		var xPoint = point.X + hScrollBar.Value;
		using var g = this.CreateGraphics();

		var totalWidth = 0f;
		var column = 0;
		foreach (var witdh in this.MeasureString(g, this.Lines[row].Text))
		{
			totalWidth += witdh;
			if (totalWidth > xPoint)
			{
				break;
			}
			column++;
		}

		return this.Lines.Take(row).Sum(elem => elem.Text.Length) + column;
	}

	/// <summary>文字の幅を計測</summary>
	/// <param name="g">グラフィック</param>
	/// <param name="text">テキスト</param>
	/// <returns>幅</returns>
	protected virtual IEnumerable<float> MeasureString(Graphics g, string text)
	{
		var size = g.MeasureString(text, this.Font, new PointF(0, 0), this.StringFormat);
		for (var i = 0; i <= ((text.Length - 1) / 32); i++)
		{

			var crs = text[(i * 32)..(Math.Min(text.Length, (i + 1) * 32))].Select((_, j) => new CharacterRange(i * 32 + j, 1)).ToArray();
			this.StringFormat.SetMeasurableCharacterRanges(crs);
			foreach (var region in g.MeasureCharacterRanges(text, this.Font, new RectangleF(0, 0, size.Width, size.Height), this.StringFormat))
			{
				yield return region.GetBounds(g).Width;
			}
		}
	}

	/// <summary>キー押下種別</summary>
	protected enum PressedKey
	{
		/// <summary>無し</summary>
		NONE,
		/// <summary>全選択</summary>
		SELECT_ALL,
		/// <summary>コピー</summary>
		COPY,
	}
}
