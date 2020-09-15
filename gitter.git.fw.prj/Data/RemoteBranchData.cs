#region Copyright Notice
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

namespace gitter.Git.AccessLayer
{
	using System;

	using gitter.Framework;

	/// <summary>RemoteBranch description.</summary>
	public sealed class RemoteBranchData : INamedObject
	{
		public RemoteBranchData(string name, Hash sha1)
		{
			Verify.Argument.IsNeitherNullNorWhitespace(name, nameof(name));

			Name = name;
			SHA1 = sha1;
		}

		/// <summary>Branch's name (short format, excluding /refs/remotes/).</summary>
		public string Name { get; }

		/// <summary>SHA1 of commit, which is pointed by branch.</summary>
		public Hash SHA1 { get; }

		/// <summary>It's not actually a branch, just a representation of detached HEAD.</summary>
		public bool IsFake => false;

		/// <summary>It is a remote tracking branch.</summary>
		public bool IsRemote => true;

		/// <summary>This branch is current HEAD.</summary>
		public bool IsCurrent => false;

		public override string ToString() => Name;
	}
}
