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

namespace gitter.Git.Gui.Views;

using System;

using Autofac;

using gitter.Framework;
using gitter.Framework.Controls;

abstract class GitViewFactoryBase : ViewFactoryBase
{
	protected GitViewFactoryBase(Guid guid, string name, IImageProvider imageProvider, bool singleton = false)
		: base(guid, name, imageProvider, singleton)
	{
	}

	public ILifetimeScope Scope { get; set; }
}

class GitViewFactoryBase<T> : GitViewFactoryBase where T : ViewBase
{
	public GitViewFactoryBase(Guid guid, string name, IImageProvider imageProvider, bool singleton = false)
		: base(guid, name, imageProvider, singleton)
	{
	}

	protected override ViewBase CreateViewCore(IWorkingEnvironment environment)
		=> Scope.Resolve<T>();
}
