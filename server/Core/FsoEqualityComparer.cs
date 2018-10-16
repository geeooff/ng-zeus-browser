using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Core
{
	public class FsoEqualityComparer : EqualityComparer<Fso>
	{
		public static new FsoEqualityComparer Default = new FsoEqualityComparer();

		public override bool Equals(Fso x, Fso y)
		{
			return x?.Uri == y?.Uri;
		}

		public override int GetHashCode(Fso obj)
		{
			return obj.Uri.GetHashCode();
		}
	}
}
