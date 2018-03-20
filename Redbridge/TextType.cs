﻿using System.Runtime.Serialization;

namespace Redbridge.SDK
{
	[DataContract]
	public enum TextType
	{
		[EnumMember]
		Plain = 0,

		[EnumMember]
		RichTextFormat = 1,
	}
}
