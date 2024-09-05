using LearnAPI.Helper;
using LearnAPI.Modal;

namespace LearnAPI.Service
{
    public interface IRiverStationService
    {
        Task<List<RiverStation>> Getall();
        Task<RiverStation> Getbycode(int id);
        Task<APIResponse> Remove(int id);
        Task<APIResponse> Create(RiverStation data);
        Task<APIResponse> Update(RiverStation data, int id);

        Task<List<RiverStation>> GetallActive();
    }
}
