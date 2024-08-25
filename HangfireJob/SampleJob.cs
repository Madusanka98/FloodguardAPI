using LearnAPI.Service;

namespace LearnAPI.HangfireJob
{
    public class SampleJob
    {
        private readonly IRiverStationService _riverStation;
        private readonly IHistoryDataService service;

        public SampleJob(IRiverStationService riverStation, IHistoryDataService service)
        {
            _riverStation = riverStation;
            this.service = service;
             //Execute();
        }

        public async Task Execute()
        {
            var riverStations = await _riverStation.GetallActive();
            if (riverStations != null)
            {
                //var data = await this.service.GetDataMain(riverStations, "7");
                //var result = await this.service.SaveHistoryData(data);
                //return Ok(data);
            }
            else
            {
                return;
            }
        }
    }
}
