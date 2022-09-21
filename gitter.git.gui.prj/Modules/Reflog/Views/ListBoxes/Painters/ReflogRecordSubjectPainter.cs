﻿#region Copyright Notice
/*
 * gitter - VCS repository management tool
 * Copyright (C) 2022  Popovskiy Maxim Vladimirovitch <amgine.gitter@gmail.com>
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

using System.Collections.Generic;

/// <summary>Paints <see cref="Revision.Subject"/> for <see cref="ReflogRecordListItem"/> and <see cref="SubjectColumn"/>.</summary>
sealed class ReflogRecordSubjectPainter : RevisionSubjectPainterBase<ReflogRecordListItem>
{
	public ReflogRecordSubjectPainter(IGraphStyle graphStyle)
		: base(graphStyle)
	{
	}

	/// <inheritdoc/>
	protected override bool TryGetData(ReflogRecordListItem item, out Revision revision, out GraphCell[] graph, out List<PointerBounds> drawnPointers)
	{
		Assert.IsNotNull(item);

		revision      = item.DataContext.Revision;
		graph         = default;
		drawnPointers = item.DrawnPointers;
		return true;
	}
}