﻿namespace gitter.Git
{
	using System;

	using Resources = gitter.Git.Properties.Resources;

	/// <summary>Git remote tracking branch.</summary>
	public sealed class RemoteBranch : BranchBase
	{
		/// <summary>Create <see cref="Branch"/> object.</summary>
		/// <param name="repository">Host repository.</param>
		/// <param name="name">Branch name.</param>
		/// <param name="pointer">Branch position.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="repository"/> == null or</para>
		/// <para><paramref name="position"/> == null.</para>
		/// </exception>
		internal RemoteBranch(Repository repository, string name, IRevisionPointer pointer)
			: base(repository, name, pointer)
		{
		}

		/// <summary>Gets a value indicating whether this branch is remote.</summary>
		/// <value><c>true</c>.</value>
		public override bool IsRemote
		{
			get { return true; }
		}

		/// <summary>Gets a value indicating whether this branch is current HEAD.</summary>
		/// <value><c>false</c>.</value>
		/// <remarks><see cref="RemoteBranch"/> can't be current HEAD.</remarks>
		public override bool IsCurrent
		{
			get { return false; }
		}

		/// <summary>Returns remote this branch is associated with.</summary>
		/// <value>Remote this branch is associated with.</value>
		public Remote Remote
		{
			get
			{
				lock(Repository.Remotes.SyncRoot)
				{
					foreach(var remote in Repository.Remotes)
					{
						if(Name.StartsWith(remote.Name + "/"))
						{
							return remote;
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets the type of this reference.</summary>
		/// <value><see cref="ReferenceType.RemoteBranch"/>.</value>
		public override ReferenceType Type
		{
			get { return ReferenceType.RemoteBranch; }
		}

		/// <summary>Gets the full branch name.</summary>
		/// <value>Full branch name.</value>
		public override string FullName
		{
			get { return GitConstants.RemoteBranchPrefix + Name; }
		}

		/// <summary>Delete branch.</summary>
		/// <exception cref="T:git.BranchIsNotFullyMergedException">Branch is not fully merged and can only be deleted by calling <see cref="Delete(bool)"/> with <paramref name="force"/> == true.</exception>
		/// <exception cref="T:gitter.Git.GitException">Failed to delete <paramref name="branch"/>.</exception>
		public override void Delete()
		{
			#region validate state

			if(IsDeleted)
			{
				throw new InvalidOperationException(
					Resources.ExcObjectIsDeleted.UseAsFormat("RemoteBranch"));
			}

			#endregion

			Repository.Refs.Remotes.Delete(this, false);
		}

		/// <summary>Delete branch from remote and local repository.</summary>
		public void DeleteFromRemote()
		{
			#region validate state

			if(IsDeleted)
			{
				throw new InvalidOperationException(
					Resources.ExcObjectIsDeleted.UseAsFormat("RemoteBranch"));
			}

			#endregion

			var remote = Remote;
			if(remote == null) throw new GitException(string.Format("Unable to find remote for branch '{0}'", Name));
			string branchName = Name.Substring(remote.Name.Length + 1);
			string remoteRefName = GitConstants.LocalBranchPrefix + branchName;
			using(Repository.Monitor.BlockNotifications(
				RepositoryNotifications.BranchChanged))
			{
				Repository.Accessor.RemoveRemoteReferences(
					new AccessLayer.RemoveRemoteReferencesParameters(
						remote.Name,
						remoteRefName));
			}
			Refresh();
		}

		/// <summary>Delete branch.</summary>
		/// <param name="force">Delete branch irrespective of its merged status.</param>
		/// <exception cref="T:git.BranchIsNotFullyMergedException">Branch is not fully merged and can only be deleted if <paramref name="force"/> == true.</exception>
		/// <exception cref="T:gitter.Git.GitException">Failed to delete <paramref name="branch"/>.</exception>
		/// <exception cref="InvalidOperationException">This <see cref="Branch"/> is already deleted.</exception>
		public override void Delete(bool force)
		{
			#region validate state

			if(IsDeleted)
			{
				throw new InvalidOperationException(
					Resources.ExcObjectIsDeleted.UseAsFormat("RemoteBranch"));
			}

			#endregion

			Repository.Refs.Remotes.Delete(this, force);
		}

		/// <summary>Makes shure that this <see cref="Branch"/> exists and <see cref="M:Position"/> is correct.</summary>
		/// <exception cref="InvalidOperationException">This <see cref="Branch"/> is deleted.</exception>
		public override void Refresh()
		{
			#region validate state

			if(IsDeleted)
			{
				throw new InvalidOperationException(
					Resources.ExcObjectIsDeleted.UseAsFormat("RemoteBranch"));
			}

			#endregion

			Repository.Refs.Remotes.Refresh(this);
		}
	}
}