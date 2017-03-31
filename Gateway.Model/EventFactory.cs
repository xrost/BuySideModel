using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gateway.Model
{
	internal static class EventFactory
	{
		private static readonly Dictionary<Type, ConstructorInfo> eventsWithOrderId;

		public static IDomainEvent Create<T>(SellSide sellSide) where T : IDomainEvent
		{
			ConstructorInfo constructor;
			if (!eventsWithOrderId.TryGetValue(typeof(T), out constructor))
				throw new InvalidOperationException();
			return (T)constructor.Invoke(new object[] { sellSide.Id });
		}

		static EventFactory()
		{
			eventsWithOrderId = GetConstructorsWithSingleIntParam().ToDictionary(c => c.DeclaringType);
		}

		private static IEnumerable<ConstructorInfo> GetConstructorsWithSingleIntParam()
		{
			var domainEventInterface = typeof(IDomainEvent).GetTypeInfo();
			return
				typeof(BuySideEvents).Assembly.DefinedTypes
					.Where(t => domainEventInterface.IsAssignableFrom(t))
					.Select(FindConstructor)
					.Where(c => c != null);
		}

		private static ConstructorInfo FindConstructor(TypeInfo type)
		{
			return type.DeclaredConstructors.FirstOrDefault(ConstructorAcceptsSingleInt);
		}

		private static bool ConstructorAcceptsSingleInt(ConstructorInfo constructor)
		{
			var paramList = constructor.GetParameters();
			return paramList.Length == 1 && paramList[0].ParameterType == typeof(int);
		}
	}
}