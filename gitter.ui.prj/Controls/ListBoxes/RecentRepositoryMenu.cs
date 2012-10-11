﻿namespace gitter
{
	using System;
	using System.Windows.Forms;

	using Resources = gitter.Properties.Resources;

	sealed class RecentRepositoryMenu : ContextMenuStrip
	{
		private readonly RecentRepositoryListItem _repository;

		public RecentRepositoryMenu(RecentRepositoryListItem repository)
		{
			Verify.Argument.IsNotNull(repository, "repository");

			_repository = repository;

			Items.Add(new ToolStripMenuItem(Resources.StrOpen, null, (s, e) => _repository.Activate()));
			Items.Add(GuiItemFactory.GetOpenUrlItem<ToolStripMenuItem>(Resources.StrOpenInWindowsExplorer, null, _repository.DataContext));
			Items.Add(GuiItemFactory.GetOpenCmdAtItem<ToolStripMenuItem>(Resources.StrOpenCommandLine, null, _repository.DataContext));
			Items.Add(GuiItemFactory.GetRemoveRecentRepositoryItem<ToolStripMenuItem>(_repository.DataContext));
		}

		public RecentRepositoryListItem Repository
		{
			get { return _repository; }
		}
	}
}
