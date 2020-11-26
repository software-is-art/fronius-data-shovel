using System;
using Xunit;

namespace FroniusSolarApiTests.V1 {
	static class Extensions {
		public static void Validate<T>(this T obj, Action<T> action) {
			Assert.NotEqual(default, obj);
			action(obj);
		}
	}
}
