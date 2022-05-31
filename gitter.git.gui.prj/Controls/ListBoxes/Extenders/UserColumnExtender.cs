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

namespace gitter.Git.Gui.Controls;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using gitter.Framework;
using gitter.Framework.Controls;

using Resources = gitter.Git.Gui.Properties.Resources;

/// <summary>Extender for <see cref="UserColumn"/>.</summary>
[ToolboxItem(false)]
partial class UserColumnExtender : ExtenderBase
{
	private ICheckBoxWidget _chkShowEmail;
	private ICheckBoxWidget _chkShowAvatar;
	private bool _disableEvents;

	/// <summary>Create <see cref="UserColumnExtender"/>.</summary>
	/// <param name="column">Related column.</param>
	public UserColumnExtender(UserColumn column)
		: base(column)
	{
		SuspendLayout();
		Name = nameof(UserColumnExtender);
		Size = new(138, 52);
		ResumeLayout();

		CreateControls();
		SubscribeToColumnEvents();
	}

	/// <inheritdoc/>
	public override IDpiBoundValue<Size> ScalableSize { get; } = DpiBoundValue.Size(new(138, 52));

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if(disposing)
		{
			if(_chkShowEmail is not null)
			{
				_chkShowEmail.Dispose();
				_chkShowEmail = null;
			}
			if(_chkShowAvatar is not null)
			{
				_chkShowAvatar.Dispose();
				_chkShowAvatar = null;
			}
			UnsubscribeFromColumnEvents();
		}
		base.Dispose(disposing);
	}

	private void CreateControls()
	{
		var conv = new DpiConverter(this);

		var iconSize = conv.ConvertX(16);

		var height  = conv.ConvertY(27);
		var spacing = conv.ConvertY(-4);

		_chkShowEmail?.Dispose();
		_chkShowEmail = Style.CreateCheckBox();
		_chkShowEmail.IsChecked = Column.ShowEmail;
		_chkShowEmail.IsCheckedChanged += OnShowEmailCheckedChanged;
		_chkShowEmail.Image = CachedResources.ScaledBitmaps[@"mail", iconSize];
		_chkShowEmail.Text = Resources.StrShowEmail;
		_chkShowEmail.Control.Bounds = new Rectangle(conv.ConvertX(6), 0, Width - conv.ConvertX(6) * 2, height);
		_chkShowEmail.Control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
		_chkShowEmail.Control.Parent = this;

		_chkShowAvatar?.Dispose();
		_chkShowAvatar = Style.CreateCheckBox();
		_chkShowAvatar.IsChecked = Column.ShowAvatar;
		_chkShowAvatar.IsCheckedChanged += OnShowAvatarCheckedChanged;
		_chkShowAvatar.Image = CommonIcons.Gravatar.GetImage(iconSize);
		_chkShowAvatar.Text = Resources.StrShowAvatar;
		_chkShowAvatar.Control.Bounds = new Rectangle(conv.ConvertX(6), spacing + height, Width - conv.ConvertX(6) * 2, height);
		_chkShowAvatar.Control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
		_chkShowAvatar.Control.Parent = this;
	}

	/// <inheritdoc/>
	protected override void OnStyleChanged()
	{
		base.OnStyleChanged();
		CreateControls();
	}

	public new UserColumn Column => (UserColumn)base.Column;

	private void SubscribeToColumnEvents()
	{
		Column.ShowEmailChanged  += OnColumnShowEmailChanged;
		Column.ShowAvatarChanged += OnColumnShowAvatarChanged;
	}

	private void UnsubscribeFromColumnEvents()
	{
		Column.ShowEmailChanged  -= OnColumnShowEmailChanged;
		Column.ShowAvatarChanged -= OnColumnShowAvatarChanged;
	}

	private void OnColumnShowEmailChanged(object sender, EventArgs e)
	{
		ShowEmail = Column.ShowEmail;
	}

	private void OnColumnShowAvatarChanged(object sender, EventArgs e)
	{
		ShowAvatar = Column.ShowAvatar;
	}

	public bool ShowEmail
	{
		get => _chkShowEmail is not null ? _chkShowEmail.IsChecked : Column.ShowEmail;
		private set
		{
			if(_chkShowEmail is not null)
			{
				_disableEvents = true;
				_chkShowEmail.IsChecked = value;
				_disableEvents = false;
			}
		}
	}

	public bool ShowAvatar
	{
		get => _chkShowAvatar is not null ? _chkShowAvatar.IsChecked : Column.ShowAvatar;
		private set
		{
			if(_chkShowAvatar is not null)
			{
				_disableEvents = true;
				_chkShowAvatar.IsChecked = value;
				_disableEvents = false;
			}
		}
	}

	private void OnShowEmailCheckedChanged(object sender, EventArgs e)
	{
		if(!_disableEvents)
		{
			_disableEvents = true;
			Column.ShowEmail = _chkShowEmail.IsChecked;
			_disableEvents = false;
		}
	}

	private void OnShowAvatarCheckedChanged(object sender, EventArgs e)
	{
		if(!_disableEvents)
		{
			_disableEvents = true;
			Column.ShowAvatar = _chkShowAvatar.IsChecked;
			_disableEvents = false;
		}
	}
}
