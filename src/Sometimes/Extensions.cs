using System;
namespace Sometimes
{
	public static class Extensions
	{
		public static void ValidatedNullable<T>(this T Object)
		{
			if(Object is null)
			{
				throw new ArgumentNullException(nameof(Object));
			}
		}
	}
}

