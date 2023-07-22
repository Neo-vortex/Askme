using System.Text;
using Jflutter.Entities;
using Jflutter.Entities.DTO;
using Jflutter.Entities.Enums;
using Jflutter.Entities.Interfaces;
using Jflutter.Services.DataAccess;
using Jflutter.Utilities;
using Microsoft.AspNetCore.Mvc;
using MLsentiment;
using VaderSharp;

namespace Jflutter.Services.Controllers;

[Route("[controller]/[action]")]
public class ApplicationService : Controller
{
    private readonly ILogger<ApplicationService> _logger;
    private readonly IDatabaseManager _databaseManager;

    public ApplicationService(ILogger<ApplicationService> logger , IDatabaseManager databaseManager )
    {
        _logger = logger;
        _databaseManager = databaseManager;
    }



    [HttpGet]
    public async Task<ActionResult<Common>> GetfeedbackReport()
    {
        var temp = new StringBuilder();
        var result =  new FeedbackReports();
        var teachers = await _databaseManager.GetTeachers();
        foreach (var teacher in teachers)
        {
            var freport = new FeedbackReport();
            
            var feedbacks = (await _databaseManager.GetFeedbacks())
                .Where(feedback => feedback.Teacher.PersonalCode == teacher.PersonalCode).ToList();

            freport.TeacherName = teacher.Firstname + "   " + teacher.Lastname + "   " + teacher.PersonalCode;
            freport.TotalFeedbacks = feedbacks.Count;
            freport.FeedbacksWithBenefits = new List<string>();
            foreach (var feedback in feedbacks)
            {
                temp.Clear();
                temp.Append(feedback.Student.Firstname + "   " + feedback.Student.Lastname + "   " + feedback.Student.PersonalCode + " says on Module " + feedback.Module.ModuleName + ":" + Environment.NewLine);
                temp.Append(feedback.FeedbackString + Environment.NewLine);
                var analyzer = new SentimentIntensityAnalyzer();
                var VadersentimentResult = analyzer.PolarityScores(feedback.FeedbackString);
                var sampleData1 = new SentimentModel.ModelInput()
                {
                    Col0 = feedback.FeedbackString
                };
                var MLsentimentResult = SentimentModel.Predict(sampleData1);
                temp.Append("Feedback Sentiment: " + "Ml : " + (MLsentimentResult.Score[1] -  MLsentimentResult.Score[0]) + " Vader : " + VadersentimentResult.Compound + Environment.NewLine);
                temp.Append("Deduced Sentiment: " + feedback.Flavour + Environment.NewLine);
                temp.Append("Feedback Validity: " + feedback.Credibility + Environment.NewLine);
                temp.Append("Student Info : " + feedback.StudentInfo + Environment.NewLine);
                freport.FeedbacksWithBenefits.Add(temp.ToString());
            }

            freport.ValidFeedbacks = feedbacks.Count(feedback => feedback.Credibility == Credibility.VALID);
            freport.InvalidFeedbacks = feedbacks.Count(feedback => feedback.Credibility == Credibility.INVALID);
            freport.NegativeValidFeedbacks = feedbacks.Count(feedback =>
                feedback.Credibility == Credibility.VALID && feedback.Flavour == Flavour.NEGETIVE);
            freport.PositiveValidFeedbacks = feedbacks.Count(feedback =>
                feedback.Credibility == Credibility.VALID && feedback.Flavour == Flavour.POSITIVE);
            freport.NeutralValidFeedbacks= feedbacks.Count(feedback =>
                feedback.Credibility == Credibility.VALID && feedback.Flavour == Flavour.NEUTRAL);
            freport.NegativeTotalFeedbacks = feedbacks.Count(feedback =>
                feedback.Flavour == Flavour.NEGETIVE);
            freport.PositiveTotalFeedbacks = feedbacks.Count(feedback =>
                feedback.Flavour == Flavour.POSITIVE);
            freport.NeutralTotalFeedbacks = feedbacks.Count(feedback =>
                feedback.Flavour == Flavour.NEUTRAL);
            result.Reports.Add(freport);
        }

        return new Common()
        {
            Description = "Teacher Feedback Report",
            Message = "Report",
            Body = result.AsJson(),
            StatusCode = 200
        };
    }




    [HttpGet]
    
    public  async  Task<ActionResult<Common>> GetModuleByLecture([FromQuery] long lectureId)
    {
        var module = await _databaseManager.GetModuleByLecture(lectureId);
        if (module == null)
        {
            return new Common()
            {

                Message = "module info not found",
                Description = "module info not fetched, no such a module",
                StatusCode = 404,
                Body = string.Empty
            }; 
        }

        return new Common()
        {

            Message = "module info",
            Description = "module info fetched",
            StatusCode = 200,
            Body = module.AsJson()
        };
    }

    [HttpGet]
    public async Task<ActionResult<Common>> MakeFeedback([FromQuery] string username, [FromQuery] string password,
        [FromQuery] string feedback, [FromQuery] long moduleID)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule != Rule.STUDENT)
        {
            return new Common
            {
                Message = "Failed to make feedback ",
                StatusCode = 403,
                Description = "You can not make feedback, you need to be a student",
                Body = string.Empty
            };
        }
        
        var result = await  _databaseManager.MakeFeedback(username, moduleID, feedback);
        if (result.Item1 != null)
        {
            return new Common
            {
                Message = "Make Feedback failed",
                StatusCode = 500,
                Description = result.Item1.Message,
                Body = string.Empty
            };
        }
        return new Common
        {
            Message = "Make Feedback successfully",
            StatusCode = 200,
            Description = "Make Feedback successfully",
            Body = result.Item2.AsJson()
        };
    }

    [HttpGet]
    public async Task<ActionResult<Common>> Answear([FromQuery] string username, [FromQuery] string password,
        [FromQuery] string reply, [FromQuery] long questionID)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule == Rule.ADMIN)
        {
            return new Common
            {
                Message = "Failed to Answear the question",
                StatusCode = 403,
                Description = "You can not Answear questions, you need to be a student/teacher",
                Body = string.Empty
            };
        }
        var result = await _databaseManager.AnswearQuestion(username , reply,questionID);
        if (result.Item1 != null)
        {
            return new Common
            {
                Message = "Question asking failed",
                StatusCode = 500,
                Description = result.Item1.Message,
                Body = string.Empty
            };
        }
        return new Common
        {
            Message = "Question asked successfully",
            StatusCode = 200,
            Description = "Question asked successfully",
            Body = result.Item2.AsJson()
        };
    }


    [HttpGet]
    
    public async  Task<ActionResult<Common>> AskaQuestion([FromQuery] string username , [FromQuery] string password  , [FromQuery] string question , [FromQuery] long lectureID , [FromQuery] long moduleID)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule != Rule.STUDENT)
        {
            return new Common
            {
                Message = "Failed to ask question",
                StatusCode = 403,
                Description = "You can not ask question, you need to be a student",
                Body = string.Empty
            };
        }
        var result = await  _databaseManager.AskQuestion(username ,question ,  lectureID , moduleID);
        if (result.Item1 != null)
        {
            return new Common
            {
                Message = "Question asking failed",
                StatusCode = 500,
                Description = result.Item1.Message,
                Body = string.Empty
            };
        }
        return new Common
        {
            Message = "Question asked successfully",
            StatusCode = 200,
            Description = "Question asked successfully",
            Body = result.Item2.AsJson()
        };
    }


    [HttpGet]
    public async Task<ActionResult<Common>> SetPresent([FromQuery] string username, [FromQuery] string password,
        [FromQuery] long lectureID , [FromQuery] long activationCode)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule != Rule.STUDENT)
        {
            return new Common
            {
                Message = "Failed to set present for a lecture",
                StatusCode = 403,
                Description = "You can not set present for a lecture, you need to be a student",
                Body = string.Empty
            };
        }
        var result = await _databaseManager.SetPresent(lectureID, long.Parse(username), activationCode);
        if (result.Item2)
        {
            return new Common()
            {
                StatusCode = 200,
                Body = string.Empty,
                Description = "Session was successfully set to present",
                Message = "SetPresent was done"
            };
        }
        return new Common()
        {
            StatusCode = 500,
            Body = string.Empty,
            Description = result.Item1!.Message,
            Message = "SetPresent was failed"
        };
    }



    [HttpGet]
    public async  Task<ActionResult<Common>> GetSentiment([FromQuery] string text)
    {
        var sampleData = new SentimentModel.ModelInput()
        {
            Col0 = text,
        };
        var predictionResult = SentimentModel.Predict(sampleData);
        var MLresult = -predictionResult.Score[0] + predictionResult.Score[1];
        var analizer = new SentimentIntensityAnalyzer();
        var Vederresult =  analizer.PolarityScores(text);
        return new Common()
        {
            Description = "Text analyzed successfully",
            Message = "Text analyzed",
            StatusCode = 200,
            Body = new Sentiment
            {
                Veder = Vederresult.Compound,
                ML = MLresult
            }.AsJson()
        };
    }

    [HttpGet]
    public  async Task<ActionResult<Common>> FindTeacherForModule([FromQuery] string moduleId)
    {

        var teacher = await _databaseManager.GetTeacherByModuleId(moduleId);
        if (teacher == null)
        {
            return new Common()
            {
                Message = "Request Failed",
                Description = "No teacher found for this module",
                Body = string.Empty,
                StatusCode = 404
            };
        }
        
        return new Common()
        {
            Message = "Request Successful",
            Description = "Teacher found for this module",
            Body =   $"Name = {teacher.Firstname}, Lastname = {teacher.Lastname} , PersonalID ={teacher.PersonalCode} ",
            StatusCode = 200
        };
    }


    [HttpGet]

    public async Task<ActionResult<Common>> GetAllStudents([FromQuery] string username, [FromQuery] string password )
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule != Rule.ADMIN)
        {
            return new Common
            {
                Message = "Failed to fetch all students",
                StatusCode = 403,
                Description = "You can not fetch all student, you need to be a admin",
                Body = string.Empty
            };
        }
        var students = await _databaseManager.GetAllStudents();
        
        return new Common
        {
            Message = "Successfully fetched all students",
            StatusCode = 200,
            Description = "You have fetch all student",
            Body = students == null ? new List<User>().AsJson() : students.AsJson()
        };
    }


    [HttpGet]
    public  async Task<ActionResult<Common>> AddStudentToModule(  [FromQuery] string username, [FromQuery] string password , [FromQuery]  string studentId, [FromQuery] string moduleId)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule != Rule.ADMIN)
        {
            return new Common
            {
                Message = "Failed to add student to module",
                StatusCode = 403,
                Description = "You can not add student, you need to be a admin",
                Body = string.Empty
            };
        }
        var result = await _databaseManager.AddStudentToModule(long.Parse(studentId) , long.Parse(moduleId));
        if (!result.Item2)
        {
            return new Common
            {
                Message = "Failed to add student to module",
                StatusCode = 500,
                Description = result.Item1?.Message! ,
                Body = string.Empty
            };
        }
        return new Common()
        {
            Body = string.Empty,
            Message = "Successfully added ",
            Description = "Successfully added student to module",
            StatusCode = 200
        };
    }


    [HttpGet]
    public async Task<ActionResult<Common>> AddLecture([FromQuery] string username, [FromQuery] string password,
        [FromQuery] string moduleID, [FromQuery] string lectureMaterial, [FromQuery] string secretCode)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password !=(password) || user.Rule != Rule.TEACHER)
        {
            return new Common
            {
                Message = "Failed to add lecture",
                StatusCode = 403,
                Description = "You can not add lecture, you need to be a teacher"
            };
        }
        
        var result =
            await _databaseManager.AddLecture(lectureMaterial, secretCode, long.Parse(moduleID), long.Parse(username));

        if (result.Item1 != null)
        {
            return new Common()
            {
                StatusCode = 500,
                Message = "Failed to add lecture",
                Description = result.Item1.Message,
                Body = string.Empty
            };
        }

        return new Common()
        {
            StatusCode = 200,
            Message = "lecture added successfully",
            Description = "Lecture has been added to the module",
            Body = string.Empty
        };

    }



    [HttpGet]
    public  async  Task<ActionResult<Common>> GetModule([FromQuery] long id)
    {
        var module = await _databaseManager.GetModule(id);
        if (module == null)
        {
            return new Common()
            {
                Message = "Module not found",
                Body = string.Empty,
                Description = "No such a module was find with specified ID",
                StatusCode = 404
            };
        }
        return new Common()
        {
            Message = "Module found",
            Body = module.AsJson(),
            Description = "Module with specified ID was found",
            StatusCode = 200
        };
        }

    [HttpGet]
    public async Task<ActionResult<Common>> GetUser([FromQuery] string username, [FromQuery] string password,
        [FromQuery] long personalID)
    {
        var result = await _databaseManager.GetUser(username);
        if ((result is not { Rule: Rule.ADMIN } || result.Password != password) &&
            (result == null || result.Password != password || result.PersonalCode != personalID))
            return new Common()
            {
                Message = "Access Denied",
                Description = "You are not allowed to get this user",
                Body = string.Empty,
                StatusCode = 403
            };
        result = await _databaseManager.GetUser(personalID);

        if (result == null)
        {
            return new Common()
            {
                Message = "User not found",
                Body = string.Empty,
                Description = "User with specified username was not found",
                StatusCode = 404
            };
        }
        return new Common()
        {
            Message = "User found",
            Body = result.AsJson(),
            Description = "User with specified username was found",
            StatusCode = 200
        };
    }


    [HttpGet]
    public  async  Task<ActionResult<Common>> GetAllModules([FromQuery] string username, [FromQuery] string password)
    {
        if (username != Consts.ADMIN_USERNAME.ToString() || password != Consts.ADMIN_PASSWORD.ToString())
        {
            return new Common()
            {
                Message = "Modules fetching failed",
                StatusCode = 403,
                Description = "You need to be admin to perform this action",
                Body = string.Empty
            };
        }
        var result = await _databaseManager.GetAllModules();
        return new Common()
        {
            Message = "Modules fetched",
            Description = "All Modules in database",
            StatusCode = 200,
            Body = result.AsJson()
        };
    }

    [HttpGet]
    public async Task<ActionResult<Common>> GetAllUsers([FromQuery] string username, [FromQuery] string password)
    {
        if (username != Consts.ADMIN_USERNAME.ToString() || password != Consts.ADMIN_PASSWORD.ToString())
        {
            return new Common()
            {
                Message = "Users fetching failed",
                StatusCode = 403,
                Description = "You need to be admin to perform this action",
                Body = string.Empty
            };
        }
        var result = await _databaseManager.GetAllUsers();
        return new Common()
        {
            Message = "Users fetchd",
            Description = "All users in databse",
            StatusCode = 200,
            Body = result.AsJson()
        };
    }

    [HttpGet]
    public async Task<ActionResult<Common>> AddInvitationcode([FromQuery] string username, [FromQuery] string password  ,[FromQuery] long code, [FromQuery] Rule rule, [FromQuery] int validationCount)
    {

        var user = await _databaseManager.GetUser(username);
        var part1 = (user == null || user.Password != (password) || user.Rule != Rule.ADMIN);
        var part2 = (username != Consts.ADMIN_USERNAME.ToString() || password != Consts.ADMIN_PASSWORD.ToString());
        if (  part1 && part2 )
        {
            return new Common
            {
                Message = "Failed to add invitation code",
                StatusCode = 403,
                Description = "You can not add invitation code, you need to be a admin",
                Body = string.Empty
            };
        }
        var result = await _databaseManager.AddInvitationcode(code, rule, validationCount);
            if (result)
            {
                return new Common()
                {
                    StatusCode = 200,
                    Message = "Invitation code added successfully",
                    Description = "Invitation code added successfully",
                    Body = string.Empty
                };
            }
            return new Common()
            {
                StatusCode = 500,
                Message = "Invitation code adding failed",
                Description = "Can not add invitation code, probably already exists?",
                Body = string.Empty
            };
        }

    [HttpGet]
    public async Task<ActionResult<Common>> Signup([FromQuery] string username, [FromQuery] string password,
        [FromQuery] long invitationCode)
    { 
        var result =  await _databaseManager.Signup(username,password, invitationCode);
        if (result.Item1 != null)
        {
            return new Common()
            {
                StatusCode = 500,
                Description = result.Item1.Message,
                Message = "Signup Failed",
                Body = string.Empty
            };
        }
        return new Common()
        {
            StatusCode = 200,
            Description = "Signup Successful",
            Message = "You can now login from login screen",
            Body = result.Item2.AsJson()
        };
    }


    [HttpGet]
    public async  Task<ActionResult<Common>> Login([FromQuery] string username,[FromQuery] string password )
    {
        var result = await _databaseManager.CheckUsernamePassword(username, password);
        if (result)
        {
            return new Common()
            {
                StatusCode = 200,
                Message = "Login Successful",
                Description = "Welcome Back!",
                Body =   (await _databaseManager.GetUser(username)).AsJson()
            };
        }

        return new Common()
        {
            StatusCode = 403,
            Message = "Login Failed",
            Description = "Bad Username or Password",
            Body = string.Empty
        };
    }

    [HttpGet]
    public async Task<ActionResult<Common>> Teachers()
    {
        var result = await _databaseManager.GetTeachers();
        return new Common()
        {
            StatusCode = 200,
            Message = "Command Successful",
            Description = "List of Teachers",
            Body = result.AsJson()
        };
    }
    [HttpGet]
    public async Task<ActionResult<Common>> AddModule([FromQuery] string username,[FromQuery] string password, [FromQuery] int moduleID , [FromQuery] int teadcherID , [FromQuery] string moduleDescription , [FromQuery] string moduleName)
    {
        var user = await _databaseManager.GetUser(username);
        if (user == null || user.Password != password || user.Rule != Rule.ADMIN  )
        {
            return new Common()
            {
                StatusCode = 403,
                Message = "Add Module Failed",
                Description = "You are not allowed to perform this action",
                Body = string.Empty
            };
        }
        var result2 = await _databaseManager.AddModule(moduleName, moduleDescription, teadcherID, moduleID);
        if (result2.Item1 != null)
        {
            return new Common()
            {
                Message = "Adding module failed",
                Description = result2.Item1.Message,
                Body = string.Empty,
                StatusCode = 500
            };
        }
        return new Common()
        {
            StatusCode = 200,
            Message = "Command Successful",
            Description = "Module Added",
            Body = result2.Item2.AsJson()
        };
    }
}