using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
	[JsonObject(MemberSerialization.OptOut)]
	public class MediaType
    {
		[JsonConverter(typeof(StringEnumConverter))]
		public MediaTypeType Type { get; set; }

		public string MimeType { get; set; }

		public string PlayerMimeType { get; set; }		
	}
}
