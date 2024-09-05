using LearnAPI.Helper;
using LearnAPI.Modal;

namespace LearnAPI.Service
{
    public interface IRiverService
    {
        Task<List<River>> Getall();
        Task<River> Getbycode(int id);
        Task<APIResponse> Remove(int id);
        Task<APIResponse> Create(River data);
        Task<APIResponse> Update(River data, int id);
    }
}
