using FloodguardAPI.Service;
using Hangfire;
using LearnAPI.Service;

namespace LearnAPI.HangfireJob
{
    public class SampleJob
    {
        private readonly IRiverStationService _riverStation;
        private readonly IHistoryDataService _service;
        private readonly IPredictResultService _predictResult;

        public SampleJob(IRiverStationService riverStation, IHistoryDataService service, IPredictResultService predictResult)
        {
            _riverStation = riverStation;
            _service = service;
            _predictResult = predictResult;
        }

        //[DisableConcurrentExecution(3600)] // Prevent concurrent execution for 1 hour
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

        public async Task ExecuteEveryThreeHours()
        {
            try
            {
                var riverStations = await _riverStation.GetallActive();
                if (riverStations != null)
                {
                    // Fetch data
                    var data1 = await _service.GetDataMain2(riverStations, "7");
                    var data2 = await _service.GetDataMain2(riverStations, "3");
                    var data3 = await _service.GetDataMain2(riverStations, "1");
                    var data4 = await _service.GetDataMain2(riverStations, "0");

                    // Combine all data
                    var data = data1.Concat(data2).Concat(data3).Concat(data4).ToList();

                    // Save the combined data
                    await _predictResult.Create(data);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }

}
