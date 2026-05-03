using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MydBContext _mydBContext;
        public RoomRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }

        public async Task<Room> AddRoom(RoomDTO roomDto)
        {
            var rooms = new Room
            {
                RoomNumber = roomDto.RoomNumber,
                RentPerMonth = roomDto.RentPerMonth,
                Status = roomDto.Status,
                Notes = roomDto.Notes,
                Address = roomDto.Address,
                Description = roomDto.Description,
                Seats = roomDto.Seats,
                ImagePath = roomDto.ImagePath,
            };

            await _mydBContext.rooms.AddAsync(rooms);
            await _mydBContext.SaveChangesAsync();
            return rooms;
        }

        public async Task<Room> DeleteRoom(Guid room_Id)
        {
            var result = await _mydBContext.rooms.FirstOrDefaultAsync(x => x.RoomId == room_Id);
            if (result != null)
            {
                _mydBContext.Remove(result);
                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _mydBContext.rooms.ToListAsync();
        }

        public async Task<Room> GetRoomById(Guid room_Id)
        {
            var result = await _mydBContext.rooms.FirstOrDefaultAsync(x => x.RoomId == room_Id);
            return result;
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            var result = await _mydBContext.rooms.FirstOrDefaultAsync(x => x.RoomId == room.RoomId);
            if (result != null)
            {
                /* result.RoomNumber = room.RoomNumber;
                 result.RentPerMonth = room.RentPerMonth;
                 result.Status = room.Status;
                 result.Notes = room.Notes;
                 result.Address = room.Address;
                 result.Description = room.Description;
                 result.Seats = room.Seats;
                 result.ImagePath = room.ImagePath;*/

                result = room;

                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }
    }
}
