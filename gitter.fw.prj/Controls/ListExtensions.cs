﻿#region Copyright Notice
/*
 * gitter - VCS repository management tool
 * Copyright (C) 2014  Popovskiy Maxim Vladimirovitch <amgine.gitter@gmail.com>
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

namespace gitter.Framework.Controls
{
	using System;
	using System.Collections.Generic;

	public static class ListExtensions
	{
		public static T FindPrevious<T>(this IList<T> items, int currentIndex, Predicate<T> filter)
		{
			Verify.Argument.IsNotNull(items, nameof(items));
			Verify.Argument.IsNotNull(filter, nameof(filter));

			while(--currentIndex >= 0)
			{
				var item = items[currentIndex];
				if(filter(item))
				{
					return item;
				}
			}
			return default;
		}

		public static T FindNext<T>(this IList<T> items, int currentIndex, Predicate<T> filter)
		{
			Verify.Argument.IsNotNull(items, nameof(items));
			Verify.Argument.IsNotNull(filter, nameof(filter));

			while(++currentIndex < items.Count)
			{
				var item = items[currentIndex];
				if(filter(item))
				{
					return item;
				}
			}
			return default;
		}
	}
}
