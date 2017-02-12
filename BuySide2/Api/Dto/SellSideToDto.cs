using System.Collections.Generic;

namespace Gateway.Model.Api
{
	internal class SellSideToDto
	{
		private SellSideDto sellSideDto;

		public SellSideDto Map(SellSide sellSide, SellSideDto dto)
		{
			sellSideDto = dto;
			MapSellSide(sellSide);
			foreach (var broker in sellSide.Brokers)
				MapBroker(broker);
			return dto;
		}

		private void MapSellSide(SellSide sellSide)
		{
			sellSideDto.Id = sellSide.Id;
			sellSideDto.IsCancelled = sellSide.IsCancelled;
			sellSideDto.Brokers = new List<BrokerDto>();
		}

		private void MapBroker(Broker broker)
		{
			var dto = new BrokerDto();
			dto.Id = broker.Id;
			dto.State = BrokerStateDto.Pending;
			if (broker.IsRejected)
				dto.State = BrokerStateDto.Rejected;
			else if (broker.IsDeleted)
				dto.State = BrokerStateDto.Deleted;

			if (broker.HasOrder)
			{
				dto.State = BrokerStateDto.Accepted;
				dto.Allocation = broker.Order.Allocation;
				dto.CancelRejected = broker.Order.IsCancelRejected;
			}
			sellSideDto.Brokers.Add(dto);
		}
	}
}