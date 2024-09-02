using LearnAPI.Helper;
using LearnAPI.Modal;

namespace LearnAPI.Service
{
    public interface IHistoryDataService
    {
        Task<List<CurrentPredict>> Getall();
        Task<HistoryData> Getbycode(int id);
        Task<APIResponse> Remove(int id);
        Task<APIResponse> Create(HistoryData data);
        Task<APIResponse> Update(HistoryData data, int id);

        Task<List<CurrentPredict>> GetDataMain(List<RiverStation> riverStation, string configHours);

        Task<List<CurrentPredict>> GetDataMain1(List<RiverStation> stations, string configHours);

        Task<APIResponse> SaveHistoryData(List<CurrentPredict> data);

        //Task<Dictionary<string, double>> GetWaterLevels(string url);
    }
}
