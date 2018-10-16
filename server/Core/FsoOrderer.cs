using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Core
{
	public class FsoOrderer : Comparer<Fso>
	{
		public OrderBy OrderBy { get; private set; }

		public FsoOrderer() : this(OrderBy.None) { }

		public FsoOrderer(OrderBy order) => OrderBy = order;

		public static new FsoOrderer Default = new FsoOrderer();

		public override int Compare(Fso x, Fso y)
		{
			if (x == null && y == null)
				return 0;
			else if (x == null && y != null)
				return -1;
			else if (x != null && y == null)
				return 1;
			
			//switch (OrderBy)
			//{
			//	case OrderBy.None: return 0;
			//	case OrderBy.Name: return StringComparer.CurrentCulture.Compare(x, y);
			//	case OrderBy.NameDesc: return StringComparer.CurrentCulture.Compare(x, y) * -1;
			//	case OrderBy.MediaType: return StringComparer.CurrentCulture.Compare(x, y);
			//	case OrderBy.MediaTypeDesc: return StringComparer.CurrentCulture.Compare(x, y) * -1;
			//}
			throw new NotImplementedException();
		}
	}
}
