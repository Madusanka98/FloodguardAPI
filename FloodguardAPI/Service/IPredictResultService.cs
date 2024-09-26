
using FloodguardAPI.Modal;
using LearnAPI.Helper;
using LearnAPI.Modal;

namespace FloodguardAPI.Service
{
    public interface IPredictResultService
    {
        Task<APIResponse> Create(List<PredictResult> data);
        Task<List<PredictResultDto>> Getall(int configTime);
    }
}
