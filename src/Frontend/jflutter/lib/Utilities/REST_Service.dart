import 'package:http/http.dart' as http;
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Views/LoginPage.dart';
import '../Enteties/Sentiment.dart';
import 'Consts.dart';

class  REST_Service {
  static Future<Common> REST_LOGIN(String username, String password) async {
    var respond = (await http
        .get(Uri.parse(
        "${Const.baseuri}/Login?username=$username&password=$password"))).body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_SIGNUP(String username, String password,
      String invitationCode) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/Signup?username=$username&password=$password&invitationCode=$invitationCode")))
        .body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_GETUSER(String username, String password,
      String personalID) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/GetUser?username=$username&password=$password&personalID=$personalID")))
        .body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_GETMODULE(String ModuleID) async {
    var respond = (await http
        .get(Uri.parse("${Const.baseuri}/GetModule?id=$ModuleID"))).body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_GETALLTEACHERS() async {
    var respond = (await http
        .get(Uri.parse("${Const.baseuri}/Teachers"))).body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_ADDMODULE(String username, String password,
      String moduleID, String teacherID, String moduleDiscription,
      String moudlename) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/AddModule?username=$username&password=$password&moduleID=$moduleID&teadcherID=$teacherID&moduleDescription=$moduleDiscription&moduleName=$moudlename")))
        .body;
    return commonFromJson(respond);
  }


  static Future<Common> REST_GETALLSTUDENTS(String username,
      String password) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/GetAllStudents?username=$username&password=$password"))).body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_ADDSTUDENT(String string, String password,
      String text, String string2) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/AddStudentToModule?username=$string&password=$password&studentID=$text&moduleId=$string2")))
        .body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_ADDLECTURE(String string, String password,
      String lectureMaterial, String lectureCode, String moduleID) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/AddLecture?username=$string&password=$password&moduleID=$moduleID&lectureMaterial=$lectureMaterial&secretCode=$lectureCode")))
        .body;
    return commonFromJson(respond);
  }

  static Future<Common> REST_ADDINVITATIONCODE(String username, String password, String invitationCode, int rule , int validationCount) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/AddInvitationcode?username=$username&password=$password&code=$invitationCode&rule=$rule&validationCount=$validationCount")))
        .body;
    return commonFromJson(respond);
  }

  static   Future<Common> REST_FINDTEACHERFORMODULE(String moduleID) async {

    var respond = (await http
            .get(Uri.parse("${Const
            .baseuri}/FindTeacherForModule?moduleId=$moduleID")))
        .body;
    return commonFromJson(respond);

  }
static    Future<Sentiment> REST_GETSENTIMENT(String text) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/GetSentiment?text=$text")))
        .body;
    return Sentiment.fromRawJson(respond);
  }

  static   Future<Common>  REST_SENDFEEDBACK(String username, String password, String feedback, String moduleID) async {
    var respond = (await http
            .get(Uri.parse("${Const
        .baseuri}/MakeFeedback?username=$username&password=$password&feedback=$feedback&moduleID=$moduleID")))
        .body;
    return commonFromJson(respond);
  }

  static  Future<Common>  REST_SETPRESENT(String username, String password, String lectureID, String activationCode) async {
    var respond = (await http
            .get(Uri.parse("${Const
            .baseuri}/SetPresent?username=$username&password=$password&lectureID=$lectureID&activationCode=$activationCode")))
        .body;
    return commonFromJson(respond);
  }

  static  Future<Common>  REST_ASKQ(String username, String password, String Q, String lectureID , String moduleID) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/AskaQuestion?username=$username&password=$password&question=$Q&lectureID=$lectureID&moduleID=$moduleID")))
        .body;
    return commonFromJson(respond);
  }

  static  Future<Common>  REST_GETMODULEBYLECTURE(String lectureID) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/GetModuleByLecture?lectureId=$lectureID")))
        .body;
    return commonFromJson(respond);
  }

  static  Future<Common>   REST_ANSWERQ(String username, String password, String answer, String Qid) async {
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/Answear?username=$username&password=$password&reply=$answer&questionID=$Qid")))
        .body;
    return commonFromJson(respond);
  }

  static  Future<Common> REST_GETFEEDBACKREPORT() async{
    var respond = (await http
        .get(Uri.parse("${Const
        .baseuri}/GetfeedbackReport")))
        .body;
    return commonFromJson(respond);
  }

}

