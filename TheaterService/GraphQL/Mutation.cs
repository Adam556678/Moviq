using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.GraphQL
{
    public class Mutation
    {
        public async Task<Hall> AddHall(
            AddHallDto input,
            [Service] IHallRepository hallRepository
        )
        {
            // map DTO to a Hall entity
            var hall = new Hall
            {
                Name = input.Name,
                NumRows = input.NumRows,
                NumColumns = input.NumColumns,
                HallType = input.HallType
            };

            // Initialize Hall seats
            if (input.AutoSeatInit)
                hall.InitializeSeats();

            try
            {
                await hallRepository.AddHallAsync(hall);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Couldn't add hall: {e.Message}");
            }

            await hallRepository.SaveChangesAsync();
            return hall;
        }

        public async Task<Seat> AddSeat(
            AddSeatDto input,
            [Service] IHallRepository hallRepository
        )
        {
            // Map dto to Seat entity
            var seat = new Seat
            {
                HallId = input.HallId,
                Row = input.Row,
                Column = input.Column,
                SeatType = input.SeatType
            };

            try
            {
                await hallRepository.AddSeatAsync(seat);
            }
            catch (Exception e)
            {
                throw new GraphQLException($"Coudln't add seat: {e.Message}");
            }

            await hallRepository.SaveChangesAsync();
            return seat;
        }

        public async Task<bool> DeleteHall(
            Guid HallId,
            [Service] IHallRepository hallRepository
        )
        {
            try
            {
                await hallRepository.DeleteHall(HallId);
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }

            await hallRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSeat(
            Guid seatId,
            [Service] IHallRepository hallRepository
        )
        {
            try
            {
                await hallRepository.DeleteSeat(seatId);
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }

            await hallRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Hall> UpdateHall(
            Guid id,
            UpdateHallDto input,
            [Service] IHallRepository hallRepository
        )
        {
            try
            {
                var hall = await hallRepository.EditHall(input, id);
                await hallRepository.SaveChangesAsync();
                return hall;
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }

        public async Task<Seat> UpdateSeat(
            Guid id,
            UpdateSeatDto input,
            [Service] IHallRepository hallRepository
        )
        {
            try
            {
                var seat = await hallRepository.EditSeat(input, id);
                await hallRepository.SaveChangesAsync();
                return seat;
            }
            catch (Exception e)
            {
                throw new GraphQLException(e.Message);
            }
        }
    }
}