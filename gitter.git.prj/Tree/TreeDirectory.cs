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

namespace gitter.Git
{
	using System;
	using System.Collections.Generic;

	public sealed class TreeDirectory : TreeItem
	{
		#region Data

		private readonly List<TreeDirectory> _directories;
		private readonly List<TreeFile> _files;
		private readonly List<TreeCommit> _commits;

		#endregion

		#region Events

		public event EventHandler<TreeDirectoryEventArgs> DirectoryAdded;
		public event EventHandler<TreeDirectoryEventArgs> DirectoryDeleted;

		public event EventHandler<TreeFileEventArgs> FileAdded;
		public event EventHandler<TreeFileEventArgs> FileDeleted;

		public event EventHandler<TreeCommitEventArgs> CommitAdded;
		public event EventHandler<TreeCommitEventArgs> CommitDeleted;

		private void InvokeDirectoryAdded(TreeDirectory folder)
		{
			var handler = DirectoryAdded;
			if(handler != null) handler(this, new TreeDirectoryEventArgs(folder));
		}

		private void OnDirectoryDeleted(TreeDirectory folder)
		{
			var handler = DirectoryDeleted;
			if(handler != null) handler(this, new TreeDirectoryEventArgs(folder));
		}

		private void InvokeFileAdded(TreeFile file)
		{
			var handler = FileAdded;
			if(handler != null) handler(this, new TreeFileEventArgs(file));
		}

		private void OnFileDeleted(TreeFile file)
		{
			var handler = FileDeleted;
			if(handler != null) handler(this, new TreeFileEventArgs(file));
		}

		private void OnCommitAdded(TreeCommit commit)
		{
			var handler = CommitAdded;
			if(handler != null) handler(this, new TreeCommitEventArgs(commit));
		}

		private void OnCommitDeleted(TreeCommit commit)
		{
			var handler = CommitDeleted;
			if(handler != null) handler(this, new TreeCommitEventArgs(commit));
		}

		#endregion

		public TreeDirectory(Repository repository, string relativePath, TreeDirectory parent, FileStatus status, string name)
			: base(repository, relativePath, parent, status, name)
		{
			_directories = new List<TreeDirectory>();
			_files = new List<TreeFile>();
			_commits = new List<TreeCommit>();
		}

		public TreeDirectory(Repository repository, string relativePath, TreeDirectory parent, string name)
			: this(repository, relativePath, parent, FileStatus.Unknown, name)
		{
		}

		internal void AddDirectory(TreeDirectory folder)
		{
			folder.Parent = this;
			_directories.Add(folder);
			InvokeDirectoryAdded(folder);
		}

		internal void AddFile(TreeFile file)
		{
			file.Parent = this;
			_files.Add(file);
			InvokeFileAdded(file);
		}

		internal void RemoveDirectory(TreeDirectory folder)
		{
			folder.Parent = null;
			_directories.Remove(folder);
			OnDirectoryDeleted(folder);
		}

		internal void RemoveDirectoryAt(int index)
		{
			var folder = _directories[index];
			folder.Parent = null;
			_directories.RemoveAt(index);
			OnDirectoryDeleted(folder);
		}

		internal void RemoveFileAt(int index)
		{
			var file = _files[index];
			file.Parent = null;
			_files.RemoveAt(index);
			OnFileDeleted(file);
		}

		internal void RemoveFile(TreeFile file)
		{
			file.Parent = null;
			_files.Remove(file);
			OnFileDeleted(file);
		}

		internal void AddCommit(TreeCommit commit)
		{
			commit.Parent = this;
			_commits.Add(commit);
			OnCommitAdded(commit);
		}

		internal void RemoveCommit(TreeCommit commit)
		{
			if(_commits.Remove(commit))
			{
				commit.Parent = null;
				OnCommitDeleted(commit);
			}
		}

		internal void RemoveCommitAt(int index)
		{
			var commit = _commits[index];
			_commits.RemoveAt(index);
			commit.Parent = null;
			OnCommitDeleted(commit);
		}

		public IList<TreeDirectory> Directories
		{
			get { return _directories; }
		}

		public IList<TreeFile> Files
		{
			get { return _files; }
		}

		public IList<TreeCommit> Commits
		{
			get { return _commits; }
		}

		public override TreeItemType ItemType
		{
			get { return TreeItemType.Tree; }
		}

		public bool IsEmpty
		{
			get { return _files.Count == 0 && _directories.Count == 0; }
		}
	}
}
