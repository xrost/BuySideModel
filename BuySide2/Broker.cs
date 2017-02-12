using System;

namespace Gateway.Model
{
	internal class Broker
	{
		private DeactivateOrderAction deactivateAction;
		public SellSideOrder Order { get; private set; }

		public Broker(IBrokerState state)
		{
			Id = state.Id;
			deactivateAction = state.DeactivateAction;
			if (deactivateAction == DeactivateOrderAction.None)
				Order = state.Order;
		}

		public int Id { get; }

		public void Accept()
		{
			EnsureOrderDoesNotExist();
			EnsureWasNotDeactivated();
			Order = new SellSideOrder();
		}

		public void RejectOrder()
		{
			EnsureOrderDoesNotExist();
			EnsureWasNotDeactivated();
			deactivateAction = DeactivateOrderAction.Rejected;
		}

		public void DeleteOrder()
		{
			EnsureOrderExists();
			Order = null;
			deactivateAction = DeactivateOrderAction.Deleted;
		}

		public bool HasOrder => Order != null;
		public bool IsRejected => deactivateAction == DeactivateOrderAction.Rejected;
		public bool IsDeleted => deactivateAction == DeactivateOrderAction.Deleted;

		private void EnsureOrderExists()
		{
			if (!HasOrder)
				throw new InvalidOperationException();
		}

		private void EnsureOrderDoesNotExist()
		{
			if (HasOrder)
				throw new InvalidOperationException();
		}

		private void EnsureWasNotDeactivated()
		{
			if (deactivateAction > DeactivateOrderAction.None)
				throw new InvalidOperationException();
		}
	}

	internal enum DeactivateOrderAction
	{
		None,
		Rejected,
		Deleted	
	}
}