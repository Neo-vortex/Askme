using System.Collections.Concurrent;
using Jflutter.Entities;
using Jflutter.Entities.Enums;
using Jflutter.Entities.Exceptions;
using Jflutter.Entities.Interfaces;

using Microsoft.EntityFrameworkCore;
using MLcredibility;
using MLsentiment;

namespace Jflutter.Services.DataAccess;

public sealed class DatabaseManager : DbContext , IDatabaseManager
{
    public readonly ILoggerFactory MyLoggerFactory;   
    public DbSet<Question> Questions { get; set; }

    public  DbSet<User?> Users { get; set; }
    
    public  DbSet<Presence> Presences { get; set; }

    public  DbSet<Module>  Modules { get; set; }
    
    public  DbSet<Lecture> Lectures { get; set; }
    public  DbSet<InvitationCode> InvitationCodes { get; set; }
    
    public  DbSet<Feedback> Feedbacks { get; set; }
    public ManualResetEventSlim _holder { get; set; } =  new ManualResetEventSlim(false);
    public ConcurrentQueue<Task> Tasks { get; set; }= new ConcurrentQueue<Task>();
    
    public  DatabaseManager() : base()
    {
        MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        StartJobExecutor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().Navigation(e => e.Modules).AutoInclude();
        modelBuilder.Entity<User>().Navigation(e => e.Questions).AutoInclude();
        modelBuilder.Entity<Module>().Navigation(e => e.Lectures).AutoInclude();
        modelBuilder.Entity<Question>().Navigation(e => e.answers).AutoInclude();
        modelBuilder.Entity<Feedback>().Navigation(e => e.Student).AutoInclude();
        modelBuilder.Entity<Feedback>().Navigation(e => e.Teacher).AutoInclude();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        optionsBuilder.UseSqlite("Data Source=Application.db");
     
    }

    private void JobExecutor()
    {
        while (true)
        {
            _holder.Wait();
            while (Tasks.TryDequeue(out var task))
            {
                try
                {
                    task.Start();
                    task.Wait();
                }
                catch(Exception e )
                {
                    //ignore
                }

            }
            _holder.Reset();
        }
        // ReSharper disable once FunctionNeverReturns
    }
    
    private  void StartJobExecutor()
    {
        var thread = new Thread(JobExecutor)
        {
            IsBackground = true
        };
        thread.Start();
    }

    private Task<T> AddJob<T>(Task<T> job)
    {
        Tasks.Enqueue(job);
        _holder.Set();
        return job;
    }
    
    public Task<bool> CheckUsernamePassword(string username, string password)
    {
        return AddJob(new Task<bool>(() =>
        {
            try
            {
                return Users.Where(usr => usr.PersonalCode == long.Parse(username) && usr.Password == password).Any();
            }
            catch (Exception e )
            {
                return false;
            }

        }));
    }

    public Task<User?> GetUser(string username)
    {
        return AddJob(new Task<User?>(() =>
        {
            try
            {
                return Users.Where(usr => usr.PersonalCode == long.Parse(username)).SingleOrDefault();
            }
            catch (Exception )
            {
                return null;
            }

        }));
    }

    public Task<ICollection<User>?> GetTeachers()
    {
        return AddJob(new Task<ICollection<User>?>(() =>
        {
            try
            {
                return Users.Where(usr => usr.Rule == Rule.TEACHER).ToList();
            }
            catch (Exception )
            {
                return null;
            }

        }));
    }

    public Task<Tuple<Exception?, User?>> Signup(string username, string password , long initationCode)
    {
        return AddJob(new Task<Tuple<Exception?, User?>>(() =>
        {
            try
            {
                var userExists = Users.Where(usr => usr.PersonalCode == long.Parse(username)).Any();
                if (userExists)
                {
                    return new Tuple<Exception, User>(new UserAlreadyExistsException("User already exists"), new User())!;
                }
                var invitationCodeExists = InvitationCodes.Where(code => code.Code == initationCode).Any();
                if (!invitationCodeExists)
                {
                    return new Tuple<Exception, User>(new InvitationCodeNotFoundException("Invitation code not found"), new User())!;
                }

                var invitationCodes = InvitationCodes.Where(code => code.Code == initationCode).ToList();
                var invitationCode = invitationCodes.Single();
                var user = new User()
                {
                    PersonalCode = long.Parse(username),
                    Password = password,
                    Rule = invitationCode.Rule,
                    Firstname = FakePeopleInfo.UserInfoDB.Single(usr => usr.PersonalCode == long.Parse(username)).Firstname,
                    Lastname =  FakePeopleInfo.UserInfoDB.Single(usr => usr.PersonalCode == long.Parse(username)).Lastname,
                    Modules = new List<Module>(),
                    Questions = new List<Question>()
                };
                Users.Add(user);
                invitationCode.ValidityCount -= 1 ;
                SaveChanges();
                return new Tuple<Exception?, User?>(null, user);
            }
            catch (Exception e )
            {
                return new Tuple<Exception?, User?>(e,null);
            }

        }));
    }

    public Task<bool> AddInvitationcode(long code, Rule rule, int validityCount)
    {
        return AddJob(new Task<bool>(() =>
        {
            var invitationCodeExists = InvitationCodes.Where(c => c.Code == code).Any();
            if (invitationCodeExists)
            {
                return false;
            }

            InvitationCodes.Add(new InvitationCode()
            {
                Code = code,
                Rule = rule,
                ValidityCount = validityCount,
            });
            SaveChanges();
            return true;
        }));
    }

    public Task<Tuple<Exception?, Module?>> AddModule(string name, string description, long teacherId, long moduleId)
    {
        return AddJob(new Task< Tuple<Exception,Module>>(() =>
        {
            if (Users.Where(usr => usr.Rule == Rule.TEACHER)
                .Any(tch => tch.Modules.Any(lct => lct.ModuleID == moduleId)))
            {
                return new Tuple<Exception?, Module?>(new ModuleAlreadyExistsException("A module with the same ID already exists"),
                    null)!;
            }
            var teacherExists = Users.Where(usr => usr.PersonalCode == teacherId).Any();

            if (!teacherExists)
            {
                return new Tuple<Exception?, Module?>(new UserNotFoundException("No such a teacher was found"),
                    null)!;
            }
            var module = new Module()
            {
                CreatedAt = DateTime.Now,
                Lectures = new List<Lecture>(),
                ModuleDescription = description,
                ModuleName = name,
                ModuleID = moduleId
            };

           var admins =  Users.Where(usr => usr.Rule == Rule.ADMIN) .ToList();
           foreach (var admin in admins)
           {
               admin.Modules.Add(module);
               SaveChanges();
           }
           var teacher = Users.Where(usr => usr.PersonalCode == teacherId).Single();
           teacher! .Modules.Add( module); 
           SaveChanges();
           return new Tuple<Exception?, Module?>(null, module)!;
        }))!;
    }

    public Task<ICollection<User?>> GetAllUsers()
    {
        return AddJob(new Task<ICollection<User?>>(() =>
        {
            return Users.ToList();  
        }));
    }

    public Task<ICollection<Module>> GetAllModules()
    {
        return AddJob(new Task<ICollection<Module>>(() =>
        {
            return  Users.Where(usr => usr.Rule == Rule.TEACHER).SelectMany(teacher => teacher.Modules).ToList();
        }));
    }

    public Task<User?> GetUser(long PersonId)
    {
        return AddJob(new Task<User?>(() =>
        {
            return Users.Where(usr => usr.PersonalCode == PersonId).SingleOrDefault();
        }));
    }

    public Task<Module?> GetModule(long moduleId)
    {
        return AddJob(new Task<Module?>(() =>
        {
           return Users.SelectMany(usr => usr.Modules).Where(mod => mod.ModuleID == moduleId).SingleOrDefault();
        }));
    }

    public Task<Tuple<Exception?, Lecture?>> AddLecture(string lectureMaterial, string secretCode, long moduleId, long teacherId)
    {
        return AddJob(new Task<Tuple<Exception?, Lecture?>>(() =>
        {
            var teacherExists = Users.Where(usr => usr.PersonalCode == teacherId).Any();
            if (!teacherExists)
            {
                return new Tuple<Exception?, Lecture?>(new UserNotFoundException("No such a teacher was found"), null)!;
            }
            var moduleExists = Users.Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId)).Any();
            if (!moduleExists)
            {
                return new Tuple<Exception?, Lecture?>(new ModuleNotFoundException("No such a module was found"), null)!;
            }

            var lecture = new Lecture()
            {
                LectureDate = DateTime.Now,
                LectureMaterial = lectureMaterial,
                SecretCode = long.Parse(secretCode)
            };

            var teacher = Users.Where(usr => (usr.Modules.Any(mod => mod.ModuleID == moduleId)) && usr.Rule == Rule.TEACHER).ToList();
            teacher.First().Modules.Where(mod => mod.ModuleID == moduleId).First().Lectures.Add(lecture);
            SaveChanges();
            return new Tuple<Exception?, Lecture?>(null, lecture);
        }));
    }

    public Task<Tuple<Exception?, bool>> AddStudentToModule(long studentId, long moduleId)
    {
        return AddJob(new Task<Tuple<Exception?, bool>>(() =>
        {
            var studentExists = Users.Where(usr => usr.PersonalCode == studentId).Any();
            if (!studentExists)
            {
                return new Tuple<Exception?, bool>(new UserNotFoundException("No such a student was found"), false)!;
            }
            var moduleExists = Users.Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId)).Any();
            if (!moduleExists)
            {
                return new Tuple<Exception?, bool>(new ModuleNotFoundException("No such a module was found"), false)!;
            }
            var student = Users.Where(usr => usr.PersonalCode == studentId).Single();
            if (student != null && student.Modules.Any(mod => mod.ModuleID == moduleId))
            {
                return new Tuple<Exception?, bool>(new UserAlreadyExistsException("Student already has this module") , false);
            }
            
            var module = Users.Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId)).First().Modules.Where(mod => mod.ModuleID == moduleId).Single();
            student?.Modules.Add(module);
            SaveChanges();
            return new Tuple<Exception?, bool>(null, true);
        }));
    }

    public Task<ICollection<User>?> GetAllStudents()
    {
        return AddJob(new Task<ICollection<User>?>(() =>
        {
            return Users.Where(usr => usr.Rule == Rule.STUDENT).ToList()!;
        }));
    }

    public Task<User?> GetTeacherByModuleId(string moduleId)
    {
        return AddJob(new Task<User?>(() =>
        {
            return Users.Where(usr => usr.Rule == Rule.TEACHER).AsQueryable().Where(tch => tch.Modules.Any(mod => mod.ModuleID == long.Parse(moduleId) )).SingleOrDefault();
        }));
    }

    public Task<Tuple<Exception?, bool>> SetPresent(long lectureId, long studentId, long activationCode)
    {
        return AddJob(new Task<Tuple<Exception?, bool>>(() =>
        {

                var studentExists = Users.Where(usr => (usr.PersonalCode == studentId) && (usr.Rule == Rule.STUDENT)).Any();
                if (!studentExists)
                {
                    return new Tuple<Exception?, bool>(new UserNotFoundException("No such a student was found"), false);
                }
                var alreadySetPresent = Presences.Where(prt => prt.LectureID == lectureId  && prt.Student.PersonalCode == studentId).Any();
                if (alreadySetPresent)
                {
                    return new Tuple<Exception?, bool>(new LectureAlreadySetPresentException("Student already set this lecture as present"), false);
                }
                var student = Users.Where(usr => usr.PersonalCode == studentId).Single();
                var lectureExist = student!.Modules.Where(mod => mod.Lectures.Any(lec => lec._id == lectureId)).Any();
                if (!lectureExist)
                {
                    return new Tuple<Exception?, bool>(new LectureNotFoundException("No such a lecture for this student"), false);
                }

                var lecture = Lectures.Where(lct => lct._id == lectureId).Single();
                if (lecture.LectureDate > DateTime.Now + TimeSpan.FromMinutes(20))
                {
                    return new Tuple<Exception?, bool>(new LectureActivationCodeExpException("20 min passed from this activation code"), false);
                }

                if (lecture.SecretCode != activationCode)
                {
                    return new Tuple<Exception?, bool>(new InvalidActivationCodeException("Wrong activation code"), false);
                }
                
                var presence = new Presence()
                {
                    LectureID = lectureId,
                    Student = student,
                };
                Presences.Add(presence);
                SaveChanges();
                return new Tuple<Exception?, bool>(null, true);
        }));
    }

    public Task<Tuple<Exception?, bool>> AskQuestion(string username, string question, long lectureId, long moduleId)
    {
        return AddJob(new Task<Tuple<Exception?, bool>>(() =>
        {
            var userExists = Users.Where(usr => usr.PersonalCode == long.Parse(username) && usr.Rule == Rule.STUDENT).Any();
            if (!userExists)
            {
                return new Tuple<Exception?, bool>(new UserNotFoundException("No such a user was found"), false);
            }
            var lectureExists = Lectures.Where(lct => lct._id == lectureId).Any();
            if (!lectureExists)
            {
                return new Tuple<Exception?, bool>(new LectureNotFoundException("No such a lecture was found"), false);
            }
            var moduleExists = Users.Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId) && usr.PersonalCode == long.Parse(username)).Any();
            if (!moduleExists)
            {
                return new Tuple<Exception?, bool>(new ModuleNotFoundException("No such a module was found for the specified teacher"), false);
            }
            var user = Users.Where(usr => usr.PersonalCode == long.Parse(username)).Single();
            var teacher = Users.Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId) && usr.Rule == Rule.TEACHER).Single();
            var lecture = Lectures.Where(lct => lct._id == lectureId).Single();
            var module = Modules.Where(mod => mod.ModuleID == moduleId).Single();
            var questionobj = new Question()
            {
                    answers = new List<Answer>(),
                    lecture = lecture,
                    question = question,
                    Module = module,
            };
            user!.Questions.Add(questionobj);
            teacher!.Questions.Add(questionobj);
            SaveChanges();
            return new Tuple<Exception?, bool>(null, true);
        }));
    }

    public Task<Tuple<Exception?, bool>> AnswearQuestion(string username, string reply, long questionId)
    {
        return AddJob(new Task<Tuple<Exception?, bool>>(() =>
        {
            var userExists = Users.Where(usr => usr.PersonalCode == long.Parse(username) && usr.Rule != Rule.ADMIN).Any();
            if (!userExists)
            {
                return new Tuple<Exception?, bool>(new UserNotFoundException("No such a user was found"), false);
            }
            var questionExists = Questions.Where(qst => qst._id == questionId).Any();
            if (!questionExists)
            {
                return new Tuple<Exception?, bool>(new QuestionNotFoundException("No such a question was found"), false);
            }
            var question = Questions.Where(qst => qst._id == questionId).Single();
            var answer = new Answer()
            {
                Answear = reply
            };
            question.answers.Add(answer);
            SaveChanges();
            return new Tuple<Exception?, bool>(null, true);
        }));
    }

    public Task<Tuple<Exception?, bool>> MakeFeedback(string username, long moduleId, string feedback)
    {
        return AddJob(new Task<Tuple<Exception?, bool>>(() =>
        {

            var feedbackExists = Feedbacks.Where(fd => fd.Module.ModuleID == moduleId && fd.Student.PersonalCode == long.Parse(username)).Any();
            if (feedbackExists)
            {
                return new Tuple<Exception?, bool>(new DuplicateWaitObjectException("feedback already exists"), false);
            }

            var userExists = Users.Where(usr => usr.PersonalCode == long.Parse(username) && usr.Rule != Rule.ADMIN).Any();
            if (!userExists)
            {
                return new Tuple<Exception?, bool>(new UserNotFoundException("No such a user was found"), false);
            }
            var moduleExists = Users.Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId)).Any();
            if (!moduleExists)
            {
                return new Tuple<Exception?, bool>(new ModuleNotFoundException("No such a module was found for the specified feedback"), false);
            }

            var teacher = Users
                .Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId) && usr.Rule == Rule.TEACHER).Single();
            
            var student = Users
                 .Where(usr => usr.Modules.Any(mod => mod.ModuleID == moduleId) && usr.Rule == Rule.STUDENT).Single();

            var buffer = Presences.Where(prs => prs.Student.PersonalCode == long.Parse(username));
            var present = student.Modules.Where(mod => mod.ModuleID == moduleId).First().Lectures.Count(lecture => buffer.Any(prs => prs.Student.PersonalCode == long.Parse(username) && prs.LectureID == lecture._id));

            var sampleData1 = new SentimentModel.ModelInput()
            {
                Col0 = feedback
            };
            var  sampleData2 = new StudentInfoModel.ModelInput()
            {
                Col0 = (float) FakePeopleInfo.UserInfoDB.First(usr => usr.PersonalCode == long.Parse(username))!.G1,
                Col1 = (float) FakePeopleInfo.UserInfoDB.First(usr => usr.PersonalCode == long.Parse(username))!.G2,
                Col2 =(float) FakePeopleInfo.UserInfoDB.First(usr => usr.PersonalCode == long.Parse(username))!.G3,
                Col3 = (float) FakePeopleInfo.UserInfoDB.First(usr => usr.PersonalCode == long.Parse(username))!.G4,
                Col4 = FakePeopleInfo.UserInfoDB.First(usr => usr.PersonalCode == long.Parse(username))!.LabAttendence,
                Col5 = present,
                Col6 = Users.Where(usr => usr.PersonalCode ==  long.Parse(username) ).Single()!.Questions.Where(q => q.Module.ModuleID == moduleId).Count()
            };
            var sentimentResult = SentimentModel.Predict(sampleData1);
            var feedbackResult = StudentInfoModel.Predict(sampleData2);
            
            var feedbackobj = new Feedback()
            {
                Module = Modules.Where(mod => mod.ModuleID == moduleId).Single(),
                Student = student!,
                Teacher = teacher!,
                FeedbackString = feedback,
                Credibility = feedbackResult.Score[0] < 0.5 ? Credibility.INVALID : Credibility.VALID,
            };
            if (sentimentResult.Score[1] -  sentimentResult.Score[0]  >= 0.3)
            {
                feedbackobj.Flavour = Flavour.POSITIVE;
            }
            if (sentimentResult.Score[1] -  sentimentResult.Score[0] < 0.3 &&  sentimentResult.Score[1] -  sentimentResult.Score[0]  > -0.3 )
            {
                feedbackobj.Flavour = Flavour.NEUTRAL;
            }
            if (sentimentResult.Score[1] -  sentimentResult.Score[0]  <= -0.3 )
            {
                feedbackobj.Flavour = Flavour.NEGETIVE;
            }
            feedbackobj.StudentInfo = $"Grade 1: {sampleData2.Col0}  Grade 2: {sampleData2.Col1}  Grade 3: {sampleData2.Col2}  Grade 4: {sampleData2.Col3}  Lab Attendence: {sampleData2.Col4}  Lectures Attended: {sampleData2.Col5} Questions Asked: {Users.Where(usr => usr.PersonalCode ==  long.Parse(username) ).Single()!.Questions.Count()}";
            Feedbacks.Add(feedbackobj);
            SaveChanges();

            return new Tuple<Exception?, bool>(null, true);
        }));
    }

    public Task<Module?> GetModuleByLecture(long lectureId)
    {
       return AddJob(new Task<Module?>(() =>
        {
            return Modules.Where(mod => mod.Lectures.Any(lec => lec._id == lectureId)).SingleOrDefault();
        }));
    }

    public Task<List<Feedback>> GetFeedbacks()
    {
        return AddJob(new Task<List<Feedback>>(() =>
        {
            return Feedbacks.ToList();
        }));
    }
}