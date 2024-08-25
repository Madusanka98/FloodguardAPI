using AutoMapper;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Container
{
    public class RiverService : IRiverService
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<RiverService> logger;
        public RiverService(LearndataContext context, IMapper mapper, ILogger<RiverService> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<APIResponse> Create(River data)
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");
                //TblRiver _customer = this.mapper.Map<River, TblRiver>(data);
                TblRiver tblRiver = new TblRiver();
                tblRiver.Name = data.Name;
                tblRiver.Isactive = true;
                await this.context.TblRivers.AddAsync(tblRiver);
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

        public async Task<List<River>> Getall()
        {
            List<River> _response = new List<River>();
            //River River = new River();
            var _data = await this.context.TblRivers.ToListAsync();
            if (_data != null)
            {
                foreach (var item in _data)
                {
                    River River = new River
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Isactive = item.Isactive
                    };
                    _response.Add(River);
                }

            }
            return _response;
        }

        public async Task<River> Getbycode(int id)
        {
            River _response = new River();
            var _data = await this.context.TblRivers.FindAsync(id);
            if (_data != null)
            {
                _response.Id = _data.Id;
                _response.Name = _data.Name;
                _response.Isactive = _data.Isactive;
            }
            return _response;
        }

        public async Task<APIResponse> Remove(int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblRivers.FindAsync(id);
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

        public async Task<APIResponse> Update(River data, int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblRivers.FindAsync(id);
                if (_customer != null)
                {
                    _customer.Name = data.Name;
                    _customer.Isactive = data.Isactive;
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
