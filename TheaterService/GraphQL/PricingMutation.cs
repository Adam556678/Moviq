using HotChocolate.Authorization;
using TheaterService.Data;
using TheaterService.DTOs.PricingDtos;
using TheaterService.Models;

namespace TheaterService.GraphQL
{
    public class PricingMutation
    {

        // --------------- Hall Pricing ---------------
        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> AddHallPricing(
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
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add hall pricing: {e.Message}");
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> UpdateHallPricing(
            Guid id,
            UpdateHallPricingDto pricingDto,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.UpdateHallPricing(id, pricingDto);
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't update hall pricing: {e.Message}");
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> RemoveHallPricing(
            Guid id,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.RemoveHallPricing(id);
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't remove hall pricing: {e.Message}");
            }
        }

        // ---------- SeatPricing ----------

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> AddSeatPricing(
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
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add seat pricing: {e.Message}");
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> UpdateSeatPricing(
            Guid id,
            UpdateSeatPricingDto seatPricingDto,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.UpdateSeatPricing(id, seatPricingDto);
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't update seat pricing: {e.Message}");
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> RemoveSeatPricing(
            Guid id,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.RemoveSeatPricing(id);
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't remove seat pricing: {e.Message}");
            }
        }

        // ---------- TimePricing ----------

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> AddTimePricing(
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
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add time pricing: {e.Message}");
            }
        }
        
        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> UpdateTimePricing(
            Guid id,
            UpdateTimePricingDto timePricingDto,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.UpdateTimePricing(id, timePricingDto);
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't update time pricing: {e.Message}");
            }
        }

        [Authorize(Roles = new[] {"Admin"})]
        public async Task<bool> RemoveTimePricing(
            Guid id,
            [Service] IPricingRepository pricingRepository)
        {
            try
            {
                await pricingRepository.RemoveTimePricing(id);
                return true;
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't remove time pricing: {e.Message}");
            }
        }
    }
}