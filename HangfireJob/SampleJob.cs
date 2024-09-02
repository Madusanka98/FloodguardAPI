using Hangfire;
using LearnAPI.Service;

namespace LearnAPI.HangfireJob
{
    public class SampleJob
    {
        private readonly IRiverStationService _riverStation;
        private readonly IHistoryDataService _service;

        public SampleJob(IRiverStationService riverStation, IHistoryDataService service)
        {
            _riverStation = riverStation;
            _service = service;
        }

        [DisableConcurrentExecution(3600)] // Prevent concurrent execution for 1 hour
        public async Task Execute()
        {
            try
            {
                var riverStations = await _riverStation.GetallActive();
                if (riverStations != null)
                {
                    var data = await _service.GetDataMain1(riverStations, "7");
                    await _service.SaveHistoryData(data);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }

}
