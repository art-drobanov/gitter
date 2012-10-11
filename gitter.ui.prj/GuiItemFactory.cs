﻿namespace gitter
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Diagnostics;

	using gitter.Framework;

	using Resources = gitter.Properties.Resources;

	static class GuiItemFactory
	{
		public static T GetRemoveRecentRepositoryItem<T>(string path)
			where T : ToolStripItem, new()
		{
			Verify.Argument.IsNeitherNullNorWhitespace(path, "path");

			var item = new T()
			{
				Text = Resources.StrRemoveRepository,
				Image = CachedResources.Bitmaps["ImgRepositoryRemove"],
				Tag = path,
			};
			item.Click += OnRemoveRecentRepositoryClick;
			return item;
		}

		public static T GetRemoveRepositoryItem<T>(RepositoryListItem repository)
			where T : ToolStripItem, new()
		{
			Verify.Argument.IsNotNull(repository, "repository");

			var item = new T()
			{
				Text = Resources.StrRemoveRepository,
				Image = CachedResources.Bitmaps["ImgRepositoryRemove"],
				Tag = repository,
			};
			item.Click += OnRemoveRepositoryClick;
			return item;
		}

		public static T GetOpenUrlItem<T>(string name, Image image, string url)
			where T : ToolStripItem, new()
		{
			Verify.Argument.IsNeitherNullNorWhitespace(url, "url");

			var item = new T()
			{
				Image = image,
				Text = name != null ? name : url,
				Tag = url,
			};
			item.Click += OnOpenUrlItemClick;
			return item;
		}

		public static T GetOpenCmdAtItem<T>(string name, Image image, string path)
			where T : ToolStripItem, new()
		{
			Verify.Argument.IsNeitherNullNorWhitespace(path, "path");

			var item = new T()
			{
				Image = image,
				Text = name != null ? name : path,
				Tag = path,
			};
			item.Click += OnOpenCmdAtItemClick;
			return item;
		}

		private static void OnRemoveRecentRepositoryClick(object sender, EventArgs e)
		{
			var item = (ToolStripItem)sender;
			var path = (string)item.Tag;

			GitterApplication.WorkingEnvironment.RecentRepositories.Remove(path);
		}

		private static void OnRemoveRepositoryClick(object sender, EventArgs e)
		{
			var item = (ToolStripItem)sender;
			var repository = (RepositoryListItem)item.Tag;
			repository.Remove();
		}

		private static void OnOpenUrlItemClick(object sender, EventArgs e)
		{
			var item = (ToolStripItem)sender;
			var url = (string)item.Tag;

			Utility.OpenUrl(url);
		}

		private static void OnOpenCmdAtItemClick(object sender, EventArgs e)
		{
			var item = (ToolStripItem)sender;
			var path = (string)item.Tag;

			var psi = new ProcessStartInfo("cmd")
				{
					WorkingDirectory = path,
				};
			using(var p = new Process())
			{
				p.StartInfo = psi;
				p.Start();
			}
		}
	}
}
