using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using FloodguardAPI.Repos.Models;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Container
{
    public class UserService : IUserService 
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        public UserService(LearndataContext learndata, IMapper mapper, IEmailService emailService)
        {
            this.context = learndata;
            this.mapper = mapper;
            this.emailService = emailService;
        }
        /*public async Task<APIResponse> ConfirmRegister(int userid, string username, string otptext)
        {
            APIResponse response = new APIResponse();
            bool otpresponse= await ValidateOTP(username,otptext);
            if (!otpresponse)
            {
                response.Result = "fail";
                response.Message = "Invalid OTP or Expired";
            }
            else
            {
                var _tempdata = await this.context.TblTempusers.FirstOrDefaultAsync(item => item.Id == userid);
                var _user = new TblUser()
                {
                    Username = username,
                    Name = _tempdata.Name,
                    Password = _tempdata.Password,
                    Email = _tempdata.Email,
                    Phone = _tempdata.Phone,
                    Failattempt = 0,
                    Isactive = true,
                    Islocked = false,
                    Role = "user"
                };
                await this.context.TblUsers.AddAsync(_user);
                await this.context.SaveChangesAsync();
                await UpdatePWDManager(username, _tempdata.Password);
                response.Result = "pass";
                response.Message = "Registered successfully.";
            }

            return response;
        }*/

       public async Task<APIResponse> UserRegisteration(UserRegister userRegister)
        {
            APIResponse response=new APIResponse();
            int userid = 0;
            bool isvalid = true;

            try
            {
                // duplicate user
                var _user = await this.context.TblUsers.Where(item => item.Username == userRegister.UserName).ToListAsync();
                if (_user.Count > 0)
                {
                    isvalid = false;
                    response.Result = "fail";
                    response.Message = "Duplicate username";
                }

                // duplicate Email
                var _useremail = await this.context.TblUsers.Where(item => item.Email == userRegister.Email).ToListAsync();
                if (_useremail.Count > 0)
                {
                    isvalid = false;
                    response.Result = "fail";
                    response.Message = "Duplicate Email";
                }


                if (userRegister != null && isvalid)
                {
                    string randomPassword = Generaterandomnumber();
                    var user = new TblUser()
                    {
                        Username = userRegister.UserName,
                        Name = userRegister.Name,
                        Email = userRegister.Email,
                        Password = randomPassword,
                        Phone = userRegister.Phone,
                        Isactive = true,
                        Role = userRegister.userType,
                        Islocked = false,
                        Failattempt = 0

                    };
                    await this.context.TblUsers.AddAsync(user);
                    await this.context.SaveChangesAsync();

                    // Get the inserted user's ID
                    userid = user.Id;

                    if (userRegister.RiverStations.Any())
                    {
                        List<TblRiverStationUsers> tblRiverStationUsers = new List<TblRiverStationUsers>();
                        foreach (var item in userRegister.RiverStations)
                        {
                            var riverStation = new TblRiverStationUsers()
                            {
                                RiverStationId = item.Id,
                                UserId = userid,
                                Isactive = true
                            };
                            tblRiverStationUsers.Add(riverStation);
                        }
                        await this.context.TblRiverStationUsers.AddRangeAsync(tblRiverStationUsers);
                        await this.context.SaveChangesAsync();
                    }
                    //userid = user.Id;
                    //string OTPText = Generaterandomnumber();
                    //await UpdateOtp(userRegister.UserName, OTPText, "register");
                    await SendOtpMail2(userRegister.Email, userRegister.Name,randomPassword,userRegister.UserName);
                    response.Result = "pass";
                    //response.Message = userid.ToString();
                }

            }catch(Exception ex)
            {
                response.Result = "fail";
            }

            return response;
           
        }

       public async Task<APIResponse> ResetPassword(string username, string oldpassword, string newpassword)
        {
            APIResponse response = new APIResponse();
            var _user = await this.context.TblUsers.FirstOrDefaultAsync(item => item.Username == username &&
            item.Password == oldpassword && item.Isactive == true);
            if(_user != null)
            {
                var _pwdhistory = await Validatepwdhistory(username, newpassword);
                if (_pwdhistory)
                {
                    response.Result = "fail";
                    response.Message = "Don't use the same password that used in last 3 transaction";
                }
                else
                {
                    _user.Password = newpassword;
                    await this.context.SaveChangesAsync();
                    await UpdatePWDManager(username, newpassword);
                    response.Result = "pass";
                    response.Message = "Password changed.";
                }
            }
            else
            {
                response.Result = "fail";
                response.Message = "Failed to validate old password.";
            }
            return response;
        }

       public async Task<APIResponse> ForgetPassword(string username)
        {
            APIResponse response = new APIResponse();
            var _user=await this.context.TblUsers.FirstOrDefaultAsync(item=>item.Username==username && item.Isactive==true);
            if (_user != null)
            {
                string otptext = Generaterandomnumber();
                await UpdateOtp(username, otptext,"forgetpassword");
                await SendOtpMail(_user.Email, otptext, _user.Name);
                response.Result = "pass";
                response.Message = "OTP sent";

            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid User";
            }
            return response;
        }

        public async Task<APIResponse> UpdatePassword(string username, string Password, string Otptext)
        {
            APIResponse response = new APIResponse();

            bool otpvalidation = await ValidateOTP(username, Otptext);
            if (otpvalidation)
            {
                bool pwdhistory=await Validatepwdhistory(username, Password);
                if(pwdhistory)
                {
                    response.Result = "fail";
                    response.Message = "Don't use the same password that used in last 3 transaction";
                }
                else
                {
                    var _user = await this.context.TblUsers.FirstOrDefaultAsync(item => item.Username == username && item.Isactive == true);
                    if (_user != null)
                    {
                        _user.Password = Password;
                        await this.context.SaveChangesAsync();
                        await UpdatePWDManager(username, Password);
                        response.Result = "pass";
                        response.Message = "Password changed";
                    }
                }
            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid OTP";
            }
            return response;
        }
        private async Task UpdateOtp(string username,string otptext,string otptype)
        {
            var _opt = new TblOtpManager()
            {
                Username = username,
                Otptext = otptext,
                Expiration = DateTime.Now.AddMinutes(30),
                Createddate = DateTime.Now,
                Otptype = otptype
            };
            await this.context.TblOtpManagers.AddAsync(_opt);
            await this.context.SaveChangesAsync();
        }

        private async Task<bool> ValidateOTP(string username,string OTPText)
        {
            bool response = false;
            var _data=await this.context.TblOtpManagers.FirstOrDefaultAsync(item=>item.Username== username 
            && item.Otptext==OTPText && item.Expiration>DateTime.Now);
            if (_data != null)
            {
                response = true;
            }
            return response;
        }

        private async Task UpdatePWDManager(string username, string password)
        {
            var _opt = new TblPwdManger()
            {
                Username = username,
                Password= password,
                ModifyDate=DateTime.Now
            };
            await this.context.TblPwdMangers.AddAsync(_opt);
            await this.context.SaveChangesAsync();
        }

        private string Generaterandomnumber()
        {
            Random random = new Random();
            string randomno = random.Next(0, 1000000).ToString("D6");
            return randomno;
        }


        private async Task SendOtpMail(string useremail, string OtpText, string Name)
        {
            var mailrequest = new Mailrequest();
            mailrequest.Email = useremail;
            mailrequest.Subject = "Reset Your Password";
            mailrequest.Emailbody = GenerateEmailBodyResendOTP(Name, OtpText);
            await this.emailService.SendEmail(mailrequest);

        }

        private string GenerateEmailBodyResendOTP(string name, string otptext)
        {
            string emailbody = "<div style='width:100%;background-color:grey'>";
            emailbody += "<p><b>Hi " + name + ", </b></p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>We received a request to reset your password. Please use the OTP below to reset your password: :<b>" + otptext + "</b></p>";
            emailbody += "<p>If you did not request a password reset, please ignore this email.</p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>Thank You.</p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>[This is an auto-generated e-mail. Please do not reply]</p>";
            emailbody += "</div>";

            return emailbody;
        }

        private async Task SendOtpMail2(string useremail,string Name,string password, string userName)
        {
            var mailrequest = new Mailrequest();
            mailrequest.Email = useremail;
            mailrequest.Subject = "Welcome to floodGUARD - Your Account Details";
            mailrequest.Emailbody = GenerateEmailBodyLogin(Name, password, userName);
            await this.emailService.SendEmail(mailrequest);

        }

        private string GenerateEmailBodyLogin(string name,string password, string userName)
        {
            string emailbody = "<div style='width:100%;'>";
            emailbody += "<p><b>Hi " + name + ",</b></h2>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>Welcome to floodGUARD!.</p>";
            emailbody += "<p>We are excited to inform you that your account has been successfully created by our admin team.</p>";
            emailbody += "<p>Below are your login details:</p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>User Name is :" + userName + "</p>";
            emailbody += "<p>Password is :" + password + "</p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>Please keep this information secure. To log in to your account, please visit our login page.</p>";
            emailbody += "<p>Once logged in, we recommend that you change your password to something more secure and easier for you to remember. You can do this by going to the rest password page.</p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>Thank you for joining us!</p>";
            emailbody += "<p>&nbsp;</p>";
            emailbody += "<p>[This is an auto-generated e-mail. Please do not reply]</p>";
            emailbody += "</div>";

            return emailbody;
        }

        private async Task<bool> Validatepwdhistory(string Username, string password)
        {
            bool response=false;
            var _pwd = await this.context.TblPwdMangers.Where(item => item.Username == Username).
                OrderByDescending(p => p.ModifyDate).Take(3).ToListAsync();
            if (_pwd.Count > 0)
            {
                var validate = _pwd.Where(o => o.Password == password);
                if (validate.Any())
                {
                    response = true;
                }
            }

            return response;

        }

        public async Task<APIResponse>UpdateStatus(string username, bool userstatus)
        {
            APIResponse response = new APIResponse();
            var _user = await this.context.TblUsers.FirstOrDefaultAsync(item => item.Username == username);
            if(_user != null)
            {
                _user.Isactive = userstatus;
                await this.context.SaveChangesAsync();
                response.Result = "pass";
                response.Message = "User Status changed";
            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid User";
            }
            return response;
        }

        public async Task<APIResponse> UpdateRole(string username, string userrole)
        {
            APIResponse response = new APIResponse();
            var _user = await this.context.TblUsers.FirstOrDefaultAsync(item => item.Username == username && item.Isactive == true);
            if (_user != null)
            {
                _user.Role = userrole;
                await this.context.SaveChangesAsync();
                response.Result = "pass";
                response.Message = "User Role changed";
            }
            else
            {
                response.Result = "fail";
                response.Message = "Invalid User";
            }
            return response;
        }

        public async Task<List<UserModel>> Getall()
        {
            List<UserModel> _response = new List<UserModel>();
            var _data = await this.context.TblUsers.ToListAsync();
            if (_data != null)
            {
                _response = this.mapper.Map<List<TblUser>, List<UserModel>>(_data);
            }
            return _response;
        }
        public async Task<UserModel> Getbycode(string code)
        {
            UserModel _response = new UserModel();
            var _data = await this.context.TblUsers.FindAsync(code);
            if (_data != null)
            {
                _response = this.mapper.Map<TblUser, UserModel>(_data);
            }
            return _response;
        }
    }
}
