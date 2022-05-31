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

namespace gitter.Git.AccessLayer.CLI;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using gitter.Framework.CLI;

/// <summary>Git command line executor.</summary>
internal interface ICommandExecutor
{
	GitOutput ExecuteCommand(Command command, CommandExecutionFlags flags);

	GitOutput ExecuteCommand(Command command, Encoding encoding, CommandExecutionFlags flags);

	int ExecuteCommand(Command command, IOutputReceiver stdOutReceiver, IOutputReceiver stdErrReceiver, CommandExecutionFlags flags);

	int ExecuteCommand(Command command, Encoding encoding, IOutputReceiver stdOutReceiver, IOutputReceiver stdErrReceiver, CommandExecutionFlags flags);

	Task<GitOutput> ExecuteCommandAsync(Command command, CommandExecutionFlags flags, CancellationToken cancellationToken = default);

	Task<GitOutput> ExecuteCommandAsync(Command command, Encoding encoding, CommandExecutionFlags flags, CancellationToken cancellationToken = default);

	Task<int> ExecuteCommandAsync(Command command, IOutputReceiver stdOutReceiver, IOutputReceiver stdErrReceiver, CommandExecutionFlags flags, CancellationToken cancellationToken = default);

	Task<int> ExecuteCommandAsync(Command command, Encoding encoding, IOutputReceiver stdOutReceiver, IOutputReceiver stdErrReceiver, CommandExecutionFlags flags, CancellationToken cancellationToken = default);
}
