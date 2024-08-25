using AutoMapper;
using Azure;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Irony.Parsing;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LearnAPI.Container
{
    public class HistoryDataService : IHistoryDataService
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<HistoryDataService> logger;

        public HistoryDataService(LearndataContext context, IMapper mapper, ILogger<HistoryDataService> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;

        }

        public HistoryDataService()
        {
        }

        public async Task<APIResponse> Create(HistoryData data)
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");
                //TblHistoryData _customer = this.mapper.Map<HistoryData, TblHistoryData>(data);
                TblHistoryData tblHistoryData = new TblHistoryData();
                tblHistoryData.RainfallData = data.RainfallData;
                tblHistoryData.Date = data.Date;
                tblHistoryData.RiverHeight = data.RiverHeight;
                tblHistoryData.RiverStationId = data.RiverStationId;
                tblHistoryData.Isactive = true;
                await this.context.TblHistoryDatas.AddAsync(tblHistoryData);
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

        public async Task<List<HistoryData>> Getall()
        {
            List<HistoryData> _response = new List<HistoryData>();
            //HistoryData HistoryData = new HistoryData();
            var _data = await this.context.TblHistoryDatas.ToListAsync();
            if (_data != null)
            {
                foreach (var item in _data)
                {
                    HistoryData historyData = new HistoryData
                    {
                        Id = item.Id,
                        RainfallData = item.RainfallData,
                        Date = item.Date,
                        RiverHeight = item.RiverHeight,
                        RiverStationId = item.RiverStationId,
                        Isactive = item.Isactive

                    };
                    _response.Add(historyData);
                }

            }
            return _response;
        }

        public async Task<HistoryData> Getbycode(int id)
        {
            HistoryData _response = new HistoryData();
            var _data = await this.context.TblHistoryDatas.FindAsync(id);
            if (_data != null)
            {
                _response.Id = _data.Id;
                _response.RainfallData = _data.RainfallData;
                _response.Date = _data.Date;
                _response.RiverHeight = _data.RiverHeight;
                _response.RiverStationId = _data.RiverStationId;
                _response.Isactive = _data.Isactive;
            }
            return _response;
        }

        public async Task<APIResponse> Remove(int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblHistoryDatas.FindAsync(id);
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

        public async Task<APIResponse> Update(HistoryData data, int id)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblHistoryDatas.FindAsync(id);
                if (_customer != null)
                {
                    _customer.RainfallData = data.RainfallData;
                    _customer.Date = data.Date;
                    _customer.RiverHeight = data.RiverHeight;
                    _customer.RiverStationId = data.RiverStationId;
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

        public async Task<List<CurrentPredict>> GetDataMain(List<RiverStation> stations, string configHours)
        {
            try
            {
                List<CurrentPredict> currentPredicts = new List<CurrentPredict>();


                string apiKey = "255b0873711e0735ea8809f1d485c3c4";
                DateTimeOffset dateTime = DateTimeOffset.UtcNow.AddDays(-1);
                long timestamp = dateTime.ToUnixTimeSeconds();
                //string units = "metric";

                foreach (RiverStation station in stations)
                {


                    //string url = $"https://api.openweathermap.org/data/2.5/forecast?lat={station.Latitude}&lon={station.Longitude}&appid={apiKey}&units={units}";
                    //string url = $"https://api.openweathermap.org/data/2.5/onecall/timemachine?lat={station.Latitude}&lon={station.Longitude}&dt={timestamp}&appid={apiKey}";
                    string url1 = "https://floodms.navy.lk/api/riverdata";
                    string modelURL = "http://127.0.0.1:8000/forecast/"+ station.Name;
                    //using HttpClient client = new HttpClient();
                    //HttpResponseMessage response = await client.GetAsync(url);


                    //double currentRainfall = GetRainfallData(station.Latitude, station.Longitude, url);
                    double currentRainfall = GetForecastRainfallData(Convert.ToDouble(station.Latitude), Convert.ToDouble(station.Longitude), apiKey);
                    double currentRiverHeight = await GetWaterLevels(url1, station.Name);
                    //List<double> results = await PostDataAndGetResponse(modelURL, currentRiverHeight, currentRainfall).Result;

                    List<double> results = PostDataAndGetResponse(modelURL, currentRiverHeight, currentRainfall).Result;
                    if (results.Any())
                    {
                        foreach (double result in results)
                        {
                            CurrentPredict currentPredict = new CurrentPredict
                            {
                                DateRange = DateTime.Now.ToString(), // Convert dateTime string to DateTime object
                                ConfigTime = 0,
                                River = "Kalu Ganga",
                                StationName = station.Name,
                                Rainfall = currentRainfall.ToString(), // Convert rain to double
                                RiverHight = result.ToString(),
                                stationId = station.Id
                            };

                            currentPredicts.Add(currentPredict);
                        }
                        
                    }
                    /*CurrentPredict currentPredict = new CurrentPredict
                    {
                        DateRange = startDate + " - " + date.AddHours(12).ToString(), // Convert dateTime string to DateTime object
                        ConfigTime = flag,
                        River = riverName,
                        StationName = stationName,
                        Rainfall = Convert.ToDouble(rain).ToString(), // Convert rain to double
                        RiverHight = "5",
                        stationId = stationId
                    };

                    currentPredicts.Add(currentPredict);*/

                    /*string responseData = await response.Content.ReadAsStringAsync();
                    var stationPredicts = ParseRainfallData(responseData, station.Name, station.Id, station.River.Name, configHours);
                    currentPredicts.AddRange(stationPredicts);*/

                }

                return currentPredicts;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<CurrentPredict>> GetDataMain2(List<RiverStation> stations, string configHours)
        {
            try
            {
                List<CurrentPredict> currentPredicts = new List<CurrentPredict>();


                string apiKey = "255b0873711e0735ea8809f1d485c3c4";
                DateTimeOffset dateTime = DateTimeOffset.UtcNow.AddDays(-1);
                long timestamp = dateTime.ToUnixTimeSeconds();
                //string units = "metric";

                foreach (RiverStation station in stations)
                {


                    //string url = $"https://api.openweathermap.org/data/2.5/forecast?lat={station.Latitude}&lon={station.Longitude}&appid={apiKey}&units={units}";
                    //string url = $"https://api.openweathermap.org/data/2.5/onecall/timemachine?lat={station.Latitude}&lon={station.Longitude}&dt={timestamp}&appid={apiKey}";
                    string url1 = "https://floodms.navy.lk/api/riverdata";
                    string modelURL = "http://127.0.0.1:8000/forecast/" + station.Name;
                    //using HttpClient client = new HttpClient();
                    //HttpResponseMessage response = await client.GetAsync(url);


                    //double currentRainfall = GetRainfallData(station.Latitude, station.Longitude, url);
                    double currentRainfall = GetForecastRainfallData(Convert.ToDouble(station.Latitude), Convert.ToDouble(station.Longitude), apiKey);
                    double currentRiverHeight = await GetWaterLevels(url1, station.Name);
                    //List<double> results = await PostDataAndGetResponse(modelURL, currentRiverHeight, currentRainfall).Result;

                    List<double> results = PostDataAndGetResponse(modelURL, currentRiverHeight, currentRainfall).Result;
                    if (results.Any())
                    {
                        foreach (double result in results)
                        {
                            CurrentPredict currentPredict = new CurrentPredict
                            {
                                DateRange = DateTime.Now.ToString(), // Convert dateTime string to DateTime object
                                ConfigTime = 0,
                                River = "Kalu Ganga",
                                StationName = station.Name,
                                Rainfall = currentRainfall.ToString(), // Convert rain to double
                                RiverHight = result.ToString(),
                                stationId = station.Id
                            };

                            currentPredicts.Add(currentPredict);
                        }

                    }
                    /*CurrentPredict currentPredict = new CurrentPredict
                    {
                        DateRange = startDate + " - " + date.AddHours(12).ToString(), // Convert dateTime string to DateTime object
                        ConfigTime = flag,
                        River = riverName,
                        StationName = stationName,
                        Rainfall = Convert.ToDouble(rain).ToString(), // Convert rain to double
                        RiverHight = "5",
                        stationId = stationId
                    };

                    currentPredicts.Add(currentPredict);*/

                    /*string responseData = await response.Content.ReadAsStringAsync();
                    var stationPredicts = ParseRainfallData(responseData, station.Name, station.Id, station.River.Name, configHours);
                    currentPredicts.AddRange(stationPredicts);*/

                }

                return currentPredicts;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<CurrentPredict>> GetDataMain1(List<RiverStation> stations, string configHours)
        {
            try
            {
                List<CurrentPredict> currentPredicts = new List<CurrentPredict>();


                string apiKey = "255b0873711e0735ea8809f1d485c3c4";
                string units = "metric";

                foreach (RiverStation station in stations)
                {
                    string url = $"https://api.openweathermap.org/data/2.5/forecast?lat={station.Latitude}&lon={station.Longitude}&appid={apiKey}&units={units}";

                    using HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var stationPredicts = await ParseRainfallData(responseData, station.Name, station.Id, station.River.Name, configHours);
                        currentPredicts.AddRange(stationPredicts);
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching weather data for {station.Name}");
                    }
                }

                return currentPredicts;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<CurrentPredict>> ParseRainfallData(string jsonResponse, string stationName, int stationId, string riverName, string configHours)
        {
            /*new Station { Name = "Putupaula", Latitude = 6.6828m, Longitude = 80.3992m },
            new Station { Name = "Ellagawa", Latitude = 6.9344m, Longitude = 80.6093m },
            new Station { Name = "Rathnapura", Latitude = 6.6958m, Longitude = 80.4037m },
            new Station { Name = "Magura", Latitude = 6.8259m, Longitude = 80.4993m },
            new Station { Name = "Kalawellawa", Latitude = 6.8725m, Longitude = 80.4472m }*/
            List<CurrentPredict> currentPredicts = new List<CurrentPredict>();
            int flag = 0;
            double rainfall = 0;
            var json = JObject.Parse(jsonResponse);
            var forecasts = json["list"];
            Console.WriteLine($"Rainfall data for {stationName}:");
            string startDate = "";
            string endDate = "";
            string url1 = "https://floodms.navy.lk/api/riverdata";

            foreach (var forecast in forecasts)
            {
                string dateTime = forecast["dt_txt"].ToString();
                DateTime date = DateTime.Parse(dateTime);

                // Filter forecasts to start from 9:00 AM for each day
                if (date.TimeOfDay == new TimeSpan(9, 00, 0) && configHours == "7")
                {
                    var rain = forecast["rain"]?["3h"]; // The "3h" field represents the rainfall volume for the last 3 hours.
                    startDate = date.ToString();
                    if (rain != null && flag == Int32.Parse(configHours))
                    {
                        string modelURL = "http://127.0.0.1:8000/forecast/" + stationName;
                        double currentRiverHeight = await GetWaterLevels(url1, stationName);
                        double results = PostDataAndGetResponse2(modelURL, currentRiverHeight, Convert.ToDouble(rain)).Result;
                        endDate = date.ToString();
                        CurrentPredict currentPredict = new CurrentPredict
                        {
                            DateRange = startDate + " - " + date.AddDays(1).ToString(), // Convert dateTime string to DateTime object
                            ConfigTime = flag,
                            River = riverName,
                            StationName = stationName,
                            Rainfall = Convert.ToDouble(rain).ToString(), // Convert rain to double
                            RiverHight = results.ToString(),
                            stationId = stationId
                        };
                        


                        currentPredicts.Add(currentPredict);
                        flag = 0;
                        startDate = endDate;
                        endDate = "";

                    }
                    else
                    {
                        Console.WriteLine($"DateTime: {dateTime}, Rainfall: No data");
                        flag = 0;
                    }
                }
                else if ((date.TimeOfDay == new TimeSpan(9, 00, 0) || date.TimeOfDay == new TimeSpan(21, 00, 0)) && configHours == "3")
                {
                    var rain = forecast["rain"]?["3h"]; // The "3h" field represents the rainfall volume for the last 3 hours.
                    startDate = date.ToString();
                    if (rain != null && flag == Int32.Parse(configHours))
                    {
                        endDate = date.ToString();
                        CurrentPredict currentPredict = new CurrentPredict
                        {
                            DateRange = startDate + " - " + date.AddHours(12).ToString(), // Convert dateTime string to DateTime object
                            ConfigTime = flag,
                            River = riverName,
                            StationName = stationName,
                            Rainfall = Convert.ToDouble(rain).ToString(), // Convert rain to double
                            RiverHight = "5",
                            stationId = stationId
                        };

                        currentPredicts.Add(currentPredict);
                        flag = 0;
                        startDate = endDate;
                        endDate = "";

                    }
                    else
                    {
                        Console.WriteLine($"DateTime: {dateTime}, Rainfall: No data");
                        flag = 0;
                    }
                }
                else if ((date.TimeOfDay == new TimeSpan(9, 00, 0) || date.TimeOfDay == new TimeSpan(3, 00, 0) ||
                    date.TimeOfDay == new TimeSpan(21, 00, 0) || date.TimeOfDay == new TimeSpan(15, 00, 0))
                    && configHours == "1")
                {
                    var rain = forecast["rain"]?["3h"]; // The "3h" field represents the rainfall volume for the last 3 hours.
                    startDate = date.ToString();
                    if (rain != null && flag == Int32.Parse(configHours))
                    {
                        endDate = date.ToString();
                        CurrentPredict currentPredict = new CurrentPredict
                        {
                            DateRange = startDate + " - " + date.AddHours(6).ToString(), // Convert dateTime string to DateTime object
                            ConfigTime = flag,
                            River = riverName,
                            StationName = stationName,
                            Rainfall = Convert.ToDouble(rain).ToString(), // Convert rain to double
                            RiverHight = "5",
                            stationId = stationId
                        };

                        currentPredicts.Add(currentPredict);
                        flag = 0;
                        startDate = endDate;
                        endDate = "";

                    }
                    else
                    {
                        Console.WriteLine($"DateTime: {dateTime}, Rainfall: No data");
                        flag = 0;
                    }
                }
                else if ((date.TimeOfDay == new TimeSpan(3, 00, 0) || date.TimeOfDay == new TimeSpan(6, 00, 0) ||
                    date.TimeOfDay == new TimeSpan(9, 00, 0) || date.TimeOfDay == new TimeSpan(12, 00, 0) ||
                    date.TimeOfDay == new TimeSpan(15, 00, 0) || date.TimeOfDay == new TimeSpan(18, 00, 0) ||
                    date.TimeOfDay == new TimeSpan(21, 00, 0) || date.TimeOfDay == new TimeSpan(00, 00, 0)
                    ) && configHours == "0")
                {
                    var rain = forecast["rain"]?["3h"]; // The "3h" field represents the rainfall volume for the last 3 hours.
                    startDate = date.ToString();
                    if (rain != null && flag == Int32.Parse(configHours))
                    {
                        endDate = date.ToString();
                        CurrentPredict currentPredict = new CurrentPredict
                        {
                            DateRange = startDate + " - " + date.AddHours(3).ToString(), // Convert dateTime string to DateTime object
                            ConfigTime = flag,
                            River = riverName,
                            StationName = stationName,
                            Rainfall = Convert.ToDouble(rain).ToString(), // Convert rain to double
                            RiverHight = "5",
                            stationId = stationId
                        };

                        currentPredicts.Add(currentPredict);
                        flag = 0;
                        startDate = endDate;
                        endDate = "";

                    }
                    else
                    {
                        Console.WriteLine($"DateTime: {dateTime}, Rainfall: No data");
                        flag = 0;
                    }
                }
                else
                {
                    var rain = forecast["rain"]?["3h"];

                    if (rain != null)
                    {
                        double val = Convert.ToDouble(rain); // Convert rain to double
                        rainfall = rainfall + val;
                    }
                    flag++;
                }
            }

            return currentPredicts;
        }

        public async Task<APIResponse> SaveHistoryData(List<CurrentPredict> data)
        {
            APIResponse response = new APIResponse();
            try
            {
                List<HistoryData> historyData = new List<HistoryData>();
                HistoryData hData = new HistoryData();
                if (data != null)
                {
                    foreach (CurrentPredict currentPredict in data)
                    {
                        string date = currentPredict.DateRange.Split("- ")[1];
                        hData.Date = Convert.ToDateTime(date);
                        hData.RiverStationId = currentPredict.stationId;
                        hData.RiverHeight = Convert.ToDouble(currentPredict.RiverHight);
                        hData.RainfallData = Convert.ToDouble(currentPredict.Rainfall);
                        hData.Isactive = true;

                        var res = await Create(hData);

                    }
                }


            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                this.logger.LogError(ex.Message, ex);
            }
            return response;
        }


        // Function to get water levels for specific stations
        public async Task<double> GetWaterLevels(string url,string station)
        {
            try
            {
                double waterLevel=0 ;
                var stations = new[] { "Putupaula", "Ellagawa", "Ratnapura", "Magura", "Kalawellawa" };

                using (var client = new HttpClient())
                {
                    // Send a GET request to the API
                    var response = await client.GetStringAsync(url);

                    // Loop through each station to extract water levels
                    //foreach (var station in stations)
                    //{
                        // Regex pattern to match the specific station's water level
                        string pattern = $@"Gauging Station : {station} .*?Water Level at 9:00 am : (\d+(\.\d+)?)";
                        var match = Regex.Match(response, pattern);

                        if (match.Success)
                        {
                            // Parse and store the water level as a double
                            waterLevel = double.Parse(match.Groups[1].Value);
                            //waterLevels[station] = waterLevel;
                        }
                    //}
                }

                return waterLevel;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        //public static double GetRainfallData(string lat, string lon, string url)
        //{
        //    // Get the Unix timestamp for 24 hours ago
        //    //DateTimeOffset dateTime = DateTimeOffset.UtcNow.AddDays(-1);
        //    //long timestamp = dateTime.ToUnixTimeSeconds();

        //    // Construct the API request URL
        //    //string url = $"https://api.openweathermap.org/data/2.5/onecall/timemachine?lat={lat}&lon={lon}&dt={timestamp}&appid={apiKey}";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        // Send the request and get the response
        //        HttpResponseMessage response = client.GetAsync(url).Result;
        //        response.EnsureSuccessStatusCode();

        //        string responseBody = response.Content.ReadAsStringAsync().Result;

        //        // Parse the JSON response
        //        JObject json = JObject.Parse(responseBody);

        //        // Extract rainfall data from the hourly records
        //        double totalRainfall = 0.0;
        //        foreach (var hour in json["hourly"])
        //        {
        //            if (hour["rain"] != null && hour["rain"]["1h"] != null)
        //            {
        //                totalRainfall += (double)hour["rain"]["1h"];
        //            }
        //        }

        //        return totalRainfall;
        //    }

        public static double GetForecastRainfallData(double lat, double lon, string apiKey)
        {
            string url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;

                // Parse the JSON response
                JObject json = JObject.Parse(responseBody);

                // Calculate total rainfall for the next 24 hours
                double totalRainfall = 0.0;
                DateTimeOffset now = DateTimeOffset.UtcNow;

                foreach (var forecast in json["list"])
                {
                    DateTimeOffset forecastTime = DateTimeOffset.Parse(forecast["dt_txt"].ToString());
                    if (forecastTime <= now.AddHours(24))
                    {
                        if (forecast["rain"] != null && forecast["rain"]["3h"] != null)
                        {
                            totalRainfall += (double)forecast["rain"]["3h"];
                        }
                    }
                }

                return totalRainfall;
            }

        }

        public async Task<List<double>> PostDataAndGetResponse(string url, double currentRiverHeight, double currentRainfall)
        {
            // Create a list to store the results
            List<double> resultList = new List<double>();

            // Use an anonymous object to create the JSON data
            var postData = new
            {
                current_height = currentRiverHeight,
                current_rainfall = currentRainfall
            };

            // Serialize the object to a JSON string
            string json = JsonConvert.SerializeObject(postData);

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    //string result = await response.Content.ReadAsStringAsync();

                    //// Parse the JSON response (assuming it returns the same data as in the request)
                    //var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, double>>(result);

                    //resultList.Add(jsonResponse["predictions"]);

                    string result = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to an object with a Predictions list
                    var jsonResponse = JsonConvert.DeserializeObject<PredictionResponse>(result);

                    // Return the list of predictions
                    return jsonResponse.Predictions;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    Console.WriteLine("Content: " + await response.Content.ReadAsStringAsync());
                }
            }

            return resultList;
        }

        public class PredictionResponse
        {
            public List<double> Predictions { get; set; }
        }

        public async Task<double> PostDataAndGetResponse2(string url, double currentRiverHeight, double currentRainfall)
        {
            // Create a list to store the results
            double resultList = new double();

            // Use an anonymous object to create the JSON data
            var postData = new
            {
                current_height = currentRiverHeight,
                current_rainfall = currentRainfall
            };

            // Serialize the object to a JSON string
            string json = JsonConvert.SerializeObject(postData);

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response to an object with a Predictions list
                    var jsonResponse = JsonConvert.DeserializeObject<PredictionResponse>(result);

                    // Check if the Predictions list is not empty and return the first prediction
                    if (jsonResponse.Predictions != null && jsonResponse.Predictions.Count > 0)
                    {
                        return jsonResponse.Predictions[0];
                    }
                    else
                    {
                        throw new Exception("No predictions found in the response.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    Console.WriteLine("Content: " + await response.Content.ReadAsStringAsync());
                }
            }

            return resultList;
        }

    }
}
