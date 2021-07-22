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

namespace gitter.Git.Gui
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	using gitter.Framework;
	using gitter.Framework.Controls;

	using Resources = gitter.Git.Gui.Properties.Resources;

	sealed class ReferencesChangedNotification : NotificationContent
	{
		private readonly ReferenceChange[] _changes;

		private const int HorizontalMargin = 2;
		private const int VerticalMargin = 2;
		private const int ItemHeight = 18;
		private const int MaxItems = 10;

		public ReferencesChangedNotification(ReferenceChange[] changes)
		{
			_changes = changes;
			Height = Measure(changes, new Dpi(DeviceDpi));
		}

		private static int Measure(ReferenceChange[] changes, Dpi dpi)
		{
			var conv = DpiConverter.FromDefaultTo(dpi);
			if(changes is not { Length: > 0 })
			{
				return conv.ConvertY(VerticalMargin) * 2 + conv.ConvertY(ItemHeight);
			}
			else
			{
				int count = changes.Length;
				if(count > MaxItems)
				{
					count = MaxItems;
				}
				return conv.ConvertY(VerticalMargin) * 2 + count * conv.ConvertY(ItemHeight);
			}
		}

		private static Bitmap GetIcon(ReferenceType referenceType, int size)
			=> referenceType switch
			{
				ReferenceType.RemoteBranch => CachedResources.ScaledBitmaps[@"rbranch", size],
				ReferenceType.LocalBranch  => CachedResources.ScaledBitmaps[@"branch",  size],
				ReferenceType.Tag          => CachedResources.ScaledBitmaps[@"tag",     size],
				_ => null,
			};

		private static string GetTextPrefix(ReferenceChangeType change)
			=> change switch
			{
				ReferenceChangeType.Added   => Resources.StrAdded,
				ReferenceChangeType.Moved   => Resources.StrUpdated,
				ReferenceChangeType.Removed => Resources.StrRemoved,
				_ => string.Empty,
			};

		protected override void OnPaint(PaintEventArgs e)
		{
			Assert.IsNotNull(e);

			using(var background = new SolidBrush(BackColor))
			{
				e.Graphics.FillRectangle(background, e.ClipRectangle);
			}
			var conv = new DpiConverter(this);
			int x = conv.ConvertX(HorizontalMargin);
			int y = conv.ConvertY(VerticalMargin);
			using var brush = new SolidBrush(ForeColor);
			if(_changes is not { Length: > 0 })
			{
				GitterApplication.TextRenderer.DrawText(
					e.Graphics, Resources.StrsEverythingIsUpToDate, Font, brush, new Point(x, y + 2));
			}
			else
			{
				var itemHeight = conv.ConvertY(ItemHeight);
				var v = conv.ConvertX(54);
				var spacing = conv.ConvertX(4);
				for(int i = 0; i < _changes.Length; ++i)
				{
					if(i == MaxItems - 1 && _changes.Length > MaxItems)
					{
						GitterApplication.TextRenderer.DrawText(
							e.Graphics, Resources.StrfNMoreChangesAreNotShown.UseAsFormat(_changes.Length - MaxItems + 1),
							Font, brush, new Point(x, y + 2));
						break;
					}
					var prefix = GetTextPrefix(_changes[i].ChangeType);
					if(!string.IsNullOrWhiteSpace(prefix))
					{
						GitterApplication.TextRenderer.DrawText(
							e.Graphics, prefix, Font, brush, new Point(x, y + 2));
					}
					var icon = GetIcon(_changes[i].ReferenceType, conv.ConvertX(16));
					if(icon is not null)
					{
						e.Graphics.DrawImage(icon, new Rectangle(x + v, y + (itemHeight - icon.Height) / 2, icon.Width, icon.Height));
					}
					GitterApplication.TextRenderer.DrawText(
						e.Graphics, _changes[i].Name, Font, brush, new Point(x + v + (icon is not null ? icon.Width : 0) + spacing, y + 2));
					y += itemHeight;
				}
			}
		}
	}
}
