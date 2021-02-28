﻿#region Copyright Notice
/*
 * gitter - VCS repository management tool
 * Copyright (C) 2013  Popovskiy Maxim Vladimirovitch <amgine.gitter@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

namespace gitter
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;

	using gitter.Framework;
	using gitter.Framework.Controls;
	using gitter.Framework.Services;

	using Resources = gitter.Properties.Resources;

	internal sealed class RepositoryListItem : CustomListBoxItem<RepositoryLink>
	{
		private static readonly Bitmap ImgRepositorySmall            = CachedResources.Bitmaps["ImgRepository"];
		private static readonly Bitmap ImgRepositoryLarge            = CachedResources.Bitmaps["ImgRepositoryLarge"];
		private static readonly Bitmap ImgRepositoryUnavailableLarge = CachedResources.Bitmaps["ImgRepositoryUnavailableLarge"];

		private static readonly StringFormat PathStringFormat;

		private bool? _exists;

		static RepositoryListItem()
		{
			PathStringFormat = new StringFormat(GitterApplication.TextRenderer.LeftAlign);
			PathStringFormat.Trimming = StringTrimming.EllipsisPath;
			PathStringFormat.FormatFlags |= StringFormatFlags.NoClip;
		}

		public RepositoryListItem(RepositoryLink rlink)
			: base(rlink)
		{
			Verify.Argument.IsNotNull(rlink, nameof(rlink));
		}

		private bool CheckExists()
		{
			try
			{
				return Directory.Exists(DataContext.Path);
			}
			catch
			{
				return false;
			}
		}

		private bool Exists
		{
			get
			{
				if(!_exists.HasValue)
				{
					_exists = CheckExists();
				}
				return _exists.Value;
			}
		}

		private string Name
		{
			get
			{
				if(string.IsNullOrEmpty(DataContext.Description))
				{
					if(DataContext.Path.EndsWithOneOf(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
					{
						return Path.GetFileName(DataContext.Path.Substring(0, DataContext.Path.Length - 1));
					}
					else
					{
						return Path.GetFileName(DataContext.Path);
					}
				}
				return DataContext.Description;
			}
		}

		protected override Size OnMeasureSubItem(SubItemMeasureEventArgs measureEventArgs)
		{
			Assert.IsNotNull(measureEventArgs);

			switch(measureEventArgs.SubItemId)
			{
				case 0:
					return measureEventArgs.MeasureImageAndText(ImgRepositorySmall, DataContext.Path);
				case 1:
					return measureEventArgs.MeasureImageAndText(ImgRepositoryLarge, DataContext.Path);
				default:
					return Size.Empty;
			}
		}

		protected override void OnPaintSubItem(SubItemPaintEventArgs paintEventArgs)
		{
			Assert.IsNotNull(paintEventArgs);

			switch(paintEventArgs.SubItemId)
			{
				case 0:
					paintEventArgs.PaintImageAndText(ImgRepositorySmall, DataContext.Path, paintEventArgs.Brush, PathStringFormat);
					break;
				case 1:
					var icon = Exists
						? ImgRepositoryLarge
						: ImgRepositoryUnavailableLarge;
					paintEventArgs.PaintImage(icon);
					var cy = paintEventArgs.Bounds.Y + 2;
					GitterApplication.TextRenderer.DrawText(
						paintEventArgs.Graphics, Name, paintEventArgs.Font, paintEventArgs.Brush, 36, cy);
					cy += 16;
					var rc = new Rectangle(36, cy, paintEventArgs.Bounds.Width - 42, 16);
					if((paintEventArgs.State & ItemState.Selected) == ItemState.Selected && GitterApplication.Style.Type == GitterStyleType.DarkBackground)
					{
						GitterApplication.TextRenderer.DrawText(
							paintEventArgs.Graphics, DataContext.Path, paintEventArgs.Font, paintEventArgs.Brush, rc, PathStringFormat);
					}
					else
					{
						using(var textBrush = new SolidBrush(GitterApplication.Style.Colors.GrayText))
						{
							GitterApplication.TextRenderer.DrawText(
								paintEventArgs.Graphics, DataContext.Path, paintEventArgs.Font, textBrush, rc, PathStringFormat);
						}
					}
					break;
			}
		}

		public override ContextMenuStrip GetContextMenu(ItemContextMenuRequestEventArgs requestEventArgs)
		{
			Assert.IsNotNull(requestEventArgs);

			var menu = new RepositoryMenu(this);
			Utility.MarkDropDownForAutoDispose(menu);
			return menu;
		}
	}
}
