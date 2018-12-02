using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Core
{
	public static class FsoExtensions
	{
		public static readonly Dictionary<MediaTypeType, int> MediaTypeRanks = new Dictionary<MediaTypeType, int>
		{
			{ MediaTypeType.Video, 0 },
			{ MediaTypeType.Audio, 1 },
			{ MediaTypeType.Subtitles, 2 },
			{ MediaTypeType.Image, 3 },
			{ MediaTypeType.Text, 4 },
			{ MediaTypeType.Unknown, 5 }
		};

		public static IOrderedEnumerable<Fso> OrderObjectsBy(this IEnumerable<Fso> objects, GroupBy groupBy, OrderBy orderBy)
		{
			IOrderedEnumerable<Fso> groupedObjects;

			switch (groupBy)
			{
				case GroupBy.FileSystemInfoType:
					groupedObjects = objects.OrderBy(obj => GetFileSystemInfoRank(obj));
					break;

				case GroupBy.None:
					throw new NotSupportedException("Ungrouped ordering is not implemented yet");

				default:
					throw new NotImplementedException($"Group ordering value \"{groupBy}\" is not implemented");
			}

			switch (orderBy)
			{
				case OrderBy.Name: return groupedObjects.ThenBy(obj => obj.Name, StringComparer.CurrentCulture);
				case OrderBy.NameDesc: return groupedObjects.ThenByDescending(obj => obj.Name, StringComparer.CurrentCulture);
				case OrderBy.MediaType: return groupedObjects.ThenBy(obj => GetMediaTypeRank(obj.MediaType.Type));
				case OrderBy.MediaTypeDesc: return groupedObjects.ThenByDescending(obj => GetMediaTypeRank(obj.MediaType.Type));
				// Creation date is not exposed by IFileInfo or IFileProvider
				//case OrderBy.Created: return groupedObjects.ThenBy(obj => obj.Created);
				//case OrderBy.CreatedDesc: return groupedObjects.ThenByDescending(obj => obj.Created);
				case OrderBy.Modified: return groupedObjects.ThenBy(obj => obj.Modified);
				case OrderBy.ModifiedDesc: return groupedObjects.ThenByDescending(obj => obj.Modified);

				case OrderBy.None:
					return groupedObjects;

				default:
					throw new NotImplementedException($"Ordering value \"{orderBy}\" is not implemented");
			}
		}

		private static int GetFileSystemInfoRank(Fso obj)
		{
			return obj.IsDir ? 0 : obj.IsFile ? 1 : 2;
		}

		private static int GetMediaTypeRank(MediaTypeType mediaType)
		{
			return MediaTypeRanks[mediaType];
		}
	}
}
