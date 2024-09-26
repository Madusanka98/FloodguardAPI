using AutoMapper;
using FloodguardAPI.Modal;
using FloodguardAPI.Repos.Models;
using FloodguardAPI.Service;
using LearnAPI.Container;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodguardAPI.Container
{
    public class PredictResultService : IPredictResultService
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<PredictResultService> logger;
        /*private readonly RiverService river;
        private readonly RiverStationService riverStation;*/
        public PredictResultService(LearndataContext context, IMapper mapper, ILogger<PredictResultService> logger/*, RiverService river, RiverStationService riverStation*/)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            /*this.river = river;
            this.riverStation = riverStation;*/
        }

        public async Task<APIResponse> Create(List<PredictResult> dataList)
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");

                // Create a list to store TblPredictResult objects
                List<TblPredictResult> predictResults = new List<TblPredictResult>();

                // Loop through each PredictResult in the incoming list
                foreach (var data in dataList)
                {
                    TblPredictResult predictResult = new TblPredictResult
                    {
                        RangeDate = data.RangeDate,
                        ConfigTime = data.ConfigTime,
                        RiverId = data.RiverId,
                        StationId = data.StationId,
                        Rainfall = data.Rainfall,
                        RiverHeight = data.RiverHeight,
                        Status = data.Status
                    };

                    // Add each TblPredictResult object to the list
                    predictResults.Add(predictResult);
                }

                // Use AddRangeAsync to add the list of results to the database context
                await this.context.TblPredictResults.AddRangeAsync(predictResults);
                await this.context.SaveChangesAsync();

                response.ResponseCode = 201;
                response.Result = "pass";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                this.logger.LogError(ex.Message, ex);
            }
            return response;
        }

        

        public async Task<List<PredictResultDto>> Getall(int configTime)
        {
            try
            {
                List<PredictResultDto> _response = new List<PredictResultDto>();
                List<TblPredictResult> _data = null;
                // Querying the database for records that match the specified ConfigTime
                if (configTime == 0)
                {
                    _data = await this.context.TblPredictResults.Where(p => p.ConfigTime == configTime).OrderByDescending(p => p.Id).Take(160).OrderBy(p => p.Id).ToListAsync();
                }else if (configTime == 1)
                {
                    _data = await this.context.TblPredictResults.Where(p => p.ConfigTime == configTime).OrderByDescending(p => p.Id).Take(80).OrderBy(p => p.Id).ToListAsync();
                }else if (configTime == 3)
                {
                    _data = await this.context.TblPredictResults.Where(p => p.ConfigTime == configTime).OrderByDescending(p => p.Id).Take(40).OrderBy(p => p.Id).ToListAsync();
                }else if (configTime == 7)
                {
                    _data = await this.context.TblPredictResults.Where(p => p.ConfigTime == configTime).OrderByDescending(p => p.Id).Take(20).OrderBy(p => p.Id).ToListAsync();
                }

                if (_data != null && _data.Count > 0)
                {
                    foreach (var item in _data)
                    {

                        var river = await this.context.TblRivers.FindAsync(item.RiverId);
                        var station = await this.context.TblRiverStations.FindAsync(item.StationId);
                        PredictResultDto predictResult = new PredictResultDto
                        {
                            Id = item.Id,
                            RangeDate = item.RangeDate,
                            ConfigTime = item.ConfigTime,
                            RiverName = river?.Name ?? "",
                            StationName = station?.Name ?? "",
                            Rainfall = item.Rainfall,
                            RiverHeight = item.RiverHeight("F2"),
                            Status = item.Status
                        };

                        _response.Add(predictResult);
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
