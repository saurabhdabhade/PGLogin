using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomById(Guid room_Id);
        Task<IEnumerable<Room>> GetAllRooms();
        Task<Room> AddRoom(RoomDTO room);
        Task<Room> UpdateRoom(Room room);
        Task<Room> DeleteRoom(Guid room_Id);
    }
}
