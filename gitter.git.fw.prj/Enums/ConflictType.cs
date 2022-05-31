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

namespace gitter.Git;

/// <summary>Conflict type.</summary>
public enum ConflictType
{
	/// <summary>No conflict.</summary>
	None = 0,

	/// <summary>DD</summary>
	BothDeleted,
	/// <summary>AA</summary>
	BothAdded,
	/// <summary>UU</summary>
	BothModified,

	/// <summary>AU</summary>
	AddedByUs,
	/// <summary>UA</summary>
	AddedByThem,
	/// <summary>DU</summary>
	DeletedByUs,
	/// <summary>UD</summary>
	DeletedByThem,

	/// <summary>Unknown conflict type.</summary>
	Unknown,
}
