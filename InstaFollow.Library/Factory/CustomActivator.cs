using System;
using System.Linq;

namespace InstaFollow.Core.Factory
{
	public class CustomActivator
	{
		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>An instance</returns>
		public static T CreateInstance<T>()
		{
			var type = typeof(T);
			var classType = type.IsClass ? type : FindInterfaceImplementation(type);

			return (T)Activator.CreateInstance(classType, true);
		}

		/// <summary>
		/// Finds the interface implementation.
		/// </summary>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>The implementation type.</returns>
		/// <exception cref="System.InvalidOperationException">{0}: Multiple implementations.</exception>
		private static Type FindInterfaceImplementation(Type interfaceType)
		{
			var implType = AppDomain.CurrentDomain.GetAssemblies()
			   .SelectMany(s => s.GetTypes())
			   .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass).ToArray();

			if (implType.Count() > 1)
			{
				throw new InvalidOperationException(string.Format("{0}: Multiple implementations.", interfaceType));
			}

			return implType.FirstOrDefault();
		}
	}
}