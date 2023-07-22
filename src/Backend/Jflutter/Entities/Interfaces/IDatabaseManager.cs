using Jflutter.Entities.Enums;

namespace Jflutter.Entities.Interfaces;

public interface IDatabaseManager
{
    public Task<bool> CheckUsernamePassword(string username, string password);
    
    public  Task<User?> GetUser(string username);
    
    public Task<ICollection<User>?> GetTeachers();

    public Task<Tuple<Exception?, User?>> Signup(string username, string password , long initationCode);


    public Task<bool> AddInvitationcode(long code, Rule rule, int validityCount);
    
    
    public  Task<Tuple<Exception?, Module?>> AddModule(string name, string description, long teacherId, long moduleId );

    public Task<ICollection<User?>> GetAllUsers();
    
    public  Task<ICollection<Module>> GetAllModules();
    
    public  Task<User?> GetUser(long PersonId);
    
    
    public Task<Module?> GetModule(long moduleId);
    
    
    public  Task<Tuple<Exception?, Lecture?>> AddLecture(string lectureMaterial, string secretCode, long moduleId, long teacherId );
    
    
    public Task<Tuple<Exception?, bool>> AddStudentToModule(long studentId, long moduleId);
    
    public Task<ICollection<User>?> GetAllStudents();

     public Task<User?> GetTeacherByModuleId(string moduleId);
    
    public Task<Tuple<Exception?, bool>> SetPresent(long lectureId, long studentId, long activationCode);

    public  Task<Tuple<Exception?,bool>>  AskQuestion(string username, string question, long lectureId, long moduleId);
   public  Task<Tuple<Exception? , bool>> AnswearQuestion(string username, string reply, long questionId);
    public Task<Tuple<Exception? ,bool>>  MakeFeedback(string username, long moduleId, string feedback);
    public Task <Module?> GetModuleByLecture(long lectureId);

    public Task<List<Feedback>> GetFeedbacks();
}