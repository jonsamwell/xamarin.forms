﻿using System;
namespace Redbridge.SDK
{
	public class DependencyResolutionException : RedbridgeException
	{
		public DependencyResolutionException() { }

		public DependencyResolutionException(string message) : base(message) { }

		public DependencyResolutionException(string message, Exception inner) : base(message, inner) { }
	}
}
