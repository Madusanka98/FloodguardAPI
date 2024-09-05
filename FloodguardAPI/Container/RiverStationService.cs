using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using AutoMapper;
using LearnAPI.Repos;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Azure;

namespace LearnAPI.Container
{
    public class RiverStationService : IRiverStationService
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<RiverStationService> logger;
        private readonly IRiverService service;
        public RiverStationService(LearndataContext context, IMapper mapper, ILogger<RiverStationService> logger, IRiverService service)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.service = service;
        }

        public async Task<APIResponse> Create(RiverStation data)
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");
                //TblRiverStation _customer = this.mapper.Map<RiverStation, TblRiverStation>(data);
                TblRiverStation tblRiverStation = new TblRiverStation();
                tblRiverStation.Name = data.Name;
                tblRiverStation.RiverId = data.River.Id;
                tblRiverStation.Latitude = data.Latitude;
                tblRiverStation.Longitude = data.Longitude;
                tblRiverStation.Isactive = true;
                tblRiverStation.AlertLevel= data.AlertLevel;
                tblRiverStation.MajorLevel= data.MajorLevel;
                tblRiverStation.MinorLevel= data.MinorLevel;
                await this.context.TblRiverStations.AddAsync(tblRiverStation);
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

        public async Task<List<RiverStation>> Getall()
       {
            List<RiverStation> _response = new List<RiverStation>();
            //RiverStation riverStation = new RiverStation();
            var _data = await this.context.TblRiverStations.ToListAsync();
            if (_data != null)
            {
                foreach (var item in _data)
                {
                    RiverStation riverStation = new RiverStation
                    {
                        Id = item.Id,
                        River= await this.service.Getbycode(item.RiverId),
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                        Name = item.Name,
                        Isactive = item.Isactive,
                        AlertLevel = item.AlertLevel,
                        MinorLevel = item.MinorLevel,
                        MajorLevel = item.MajorLevel
                    };

                    _response.Add(riverStation);
                }
                
            }
            return _response;
        }

        /*public async Task<List<RiverStation>> GetallActive()
        {
            try
            {
                List<RiverStation> _response = new List<RiverStation>();
                //RiverStation riverStation = new RiverStation();
                var _data = await this.context.TblRiverStations.ToListAsync();
                if (_data != null)
                {
                    foreach (var item in _data)
                    {
                        if (item.Isactive == true)
                        {
                            RiverStation riverStation = new RiverStation
                            {
                                Id = item.Id,
                                River = await this.service.Getbycode(item.RiverId),
                                Latitude = item.Latitude,
                                Longitude = item.Longitude,
                                Name = item.Name,
                                Isactive = item.Isactive
                            };
                            _response.Add(riverStation);
                        }
                    }
                }
                return _response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }*/

        public async Task<List<RiverStation>> GetallActive()
        {
            try
            {
                List<RiverStation> _response = new List<RiverStation>();
                //RiverStation riverStation = new RiverStation();
                var _data = await this.context.TblRiverStations.Where(x => x.Isactive == true).ToListAsync();
                if (_data != null)
                {
                    foreach (var item in _data)
                    {
                        RiverStation riverStation = new RiverStation
                        {
                            Id = item.Id,
                            River = await this.service.Getbycode(item.RiverId),
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            Name = item.Name,
                            Isactive = item.Isactive,
                            AlertLevel = item.AlertLevel,
                            MinorLevel = item.MinorLevel,
                            MajorLevel = item.MajorLevel
                        };

                        _response.Add(riverStation);
                    }

                }
                return _response;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                throw new ApplicationException("An error occurred while fetching active river stations", ex);
            }
        }



        public async Task<RiverStation> Getbycode(int id)
        {
            RiverStation _response = new RiverStation();
            var _data = await this.context.TblRiverStations.FindAsync(id);
            if (_data != null)
            {
                _response.Id = _data.Id;
                _response.Latitude = _data.Latitude;
                _response.Longitude = _data.Longitude;
                _response.River= await this.service.Getbycode(_data.RiverId);
                _response.Name = _data.Name;
                _response.Isactive = _data.Isactive;
                _response.AlertLevel = _data.AlertLevel;
                _response.MajorLevel = _data.MajorLevel;
                _response.MinorLevel = _data.MinorLevel;
            }
            return _response;
        }

        public async Task<APIResponse> Remove(int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblRiverStations.FindAsync(id);
                if (_customer != null)
                {
                    //_customer.Name = data.Name;
                    _customer.Isactive = false;
                    //_customer.Id = data.Id;
                    await this.context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not found";
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> Update(RiverStation data, int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _response = await this.context.TblRiverStations.FindAsync(id);
                if (_response != null)
                {
                    _response.Name = data.Name;
                    _response.Longitude = data.Longitude;
                    _response.Latitude = data.Latitude;
                    _response.RiverId = data.River.Id;
                    _response.Isactive = data.Isactive;
                    _response.AlertLevel = data.AlertLevel;
                    _response.MajorLevel = data.MajorLevel;
                    _response.MinorLevel = data.MinorLevel;
                    //_customer.Id = data.Id;
                    await this.context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not found";
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }

        
    }
}
