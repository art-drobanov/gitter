	using gitter.Framework;

				Preallocated<DiffColumnHeader>.EmptyArray,
						Preallocated<DiffLineState>.EmptyArray,
						Preallocated<int>.EmptyArray,
				lines.Add(new DiffLine(DiffLineState.Header, Preallocated<DiffLineState>.EmptyArray, Preallocated<int>.EmptyArray, ReadLine()));
				lines.Add(new DiffLine(state, Preallocated<DiffLineState>.EmptyArray, Preallocated<int>.EmptyArray, ReadLine()));
			return new DiffHunk(Preallocated<DiffColumnHeader>.EmptyArray, lines, new DiffStats(0, 0, lines.Count - headers, headers), true);
							if(int.TryParse(strIndex, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
							if(int.TryParse(strIndex, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
			bool isCombined = ReadDiffFileHeader1(out var source, out var target);