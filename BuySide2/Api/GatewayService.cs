using System;

namespace Gateway.Model.Api
{
	public class GatewayService
	{
		private readonly IGatewayRepository repository;

		internal GatewayService(IGatewayRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException(nameof(repository));
			this.repository = repository;
		}

		public void AddBuySide(object step)
		{
			var buySide = repository.Get();
			buySide.Add(step);
			repository.Save(buySide);
		}
	}

	internal interface IGatewayRepository
	{
		BuySide Get();
		void Save(BuySide buySide);
	}
}