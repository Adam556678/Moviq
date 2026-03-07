using TheaterService.Data;
using TheaterService.DTOs.PricingDtos;
using TheaterService.Models;

namespace TheaterService.GraphQL
{
    public class PricingMutation
    {

        // --------------- Hall Pricing ---------------
        public async Task AddHallPricing(
            AddHallPricingDto pricingDto,
            [Service] IPricingRepository pricingRepository
        )
        {
            // Map DTO to Pricing entity
            var pricing = new HallPricing
            {
                HallType = pricingDto.HallType,
                Multiplier = pricingDto.Multiplier
            };

            try
            {
                await pricingRepository.AddHallPricing(pricing);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add hall pricing: {e.Message}");
            }
        }

        public async Task UpdateHallPricing(
            Guid id,
            UpdateHallPricingDto pricingDto,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.UpdateHallPricing(id, pricingDto);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't update hall pricing: {e.Message}");
            }
        }

        public async Task RemoveHallPricing(
            Guid id,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.RemoveHallPricing(id);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't remove hall pricing: {e.Message}");
            }
        }

        // ---------- SeatPricing ----------

        public async Task AddSeatPricing(
            AddSeatPricingDto seatPricingDto,
            [Service] IPricingRepository pricingRepository)
        {

            // Map DTO to pricing entity
            var pricing = new SeatPricing
            {
                SeatType = seatPricingDto.SeatType,
                BasePrice = seatPricingDto.BasePrice
            };

            try
            {
                await pricingRepository.AddSeatPricing(pricing);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add seat pricing: {e.Message}");
            }
        }

        public async Task UpdateSeatPricing(
            Guid id,
            UpdateSeatPricingDto seatPricingDto,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.UpdateSeatPricing(id, seatPricingDto);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't update seat pricing: {e.Message}");
            }
        }

        public async Task RemoveSeatPricing(
            Guid id,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.RemoveSeatPricing(id);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't remove seat pricing: {e.Message}");
            }
        }

        // ---------- TimePricing ----------

        public async Task AddTimePricing(
            AddTimePricingDto timePricingDto,
            [Service] IPricingRepository pricingRepository)
        {

            var pricing = new TimePricing{
                StartHour = timePricingDto.StartHour,
                EndHour = timePricingDto.EndHour,
                Multiplier = timePricingDto.Multiplier
            };

            try
            {
                await pricingRepository.AddTimePricing(pricing);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add time pricing: {e.Message}");
            }
        }

        public async Task UpdateTimePricing(
            Guid id,
            UpdateTimePricingDto timePricingDto,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.UpdateTimePricing(id, timePricingDto);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't update time pricing: {e.Message}");
            }
        }

        public async Task RemoveTimePricing(
            Guid id,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.RemoveTimePricing(id);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't remove time pricing: {e.Message}");
            }
        }
    }
}