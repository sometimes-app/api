using System;
namespace Sometimes
{
	public static class Extensions
	{
		public static T ValidatedNullable<T>(this T Object)
		{
			if(Object is null)
			{
				throw new ArgumentNullException(nameof(Object));
			}

			return Object;
		}
	}
}

