
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Enteties/Module.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:jflutter/Utilities/Utilities.dart';
import 'package:jflutter/Views/AddInvitationCode.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import '../DTO/Common.dart';
import '../DTO/FeedbackReports.dart';
import '../Enteties/User.dart';
import '../Enteties/userrule.dart';
import '../Utilities/REST_Service.dart';
import 'AddModulePage.dart';
import 'AddStudnetPage.dart';
import 'FeedbackReport.dart';
import 'LecturesListPage.dart';
import 'QuestionList.dart';

class ModulesPage extends StatefulWidget {
  const ModulesPage({Key? key}) : super(key: key);

  @override
  State<ModulesPage> createState() => _MyStatefulWidgetState();
}
class _MyStatefulWidgetState extends State<ModulesPage> {


  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: true,
          title: const Text('Modules'),
          actions: <Widget>[
            PopupMenuButton<String>(
              onSelected: handleClick,
              itemBuilder: (BuildContext context) {
                if (InfoRepo.user!.rule == 0) {
                  return {'Add New Module', 'Generate New Invitation Code', 'Refresh', 'Show my info'}.map((String choice) {
                    return PopupMenuItem<String>(
                      value: choice,
                      child: Text(choice),
                    );
                  }).toList();
                } else if (InfoRepo.user!.rule == 1) {
                  return { 'Refresh' , 'Show my info'}.map((String choice) {
                    return PopupMenuItem<String>(
                      value: choice,
                      child: Text(choice),
                    );
                  }).toList();
                } else if (InfoRepo.user!.rule == 2){
                  return { 'Refresh' , 'Show my info' }.map((String choice) {
                    return PopupMenuItem<String>(
                      value: choice,
                      child: Text(choice),
                    );
                  }).toList();

                }else{
                  return {  'Add New Module', 'Generate New Invitation Code', 'Refresh', 'Make Feedback About a Teacher' , 'Show my info'}.map((String choice) {
                    return PopupMenuItem<String>(
                      value: choice,
                      child: Text(choice),
                    );
                  }).toList();
                }

              },
            ),
          ],
        ),
      body: Center(
        child: Column(
          children:  <Widget>[
            ListView.builder
              (
                scrollDirection: Axis.vertical,
                shrinkWrap: true,
                itemCount: InfoRepo.user!.modules.length,
                itemBuilder: (BuildContext ctxt, int index) {
                  return Card(
                    elevation: 5,
                    child: ListTile(
                        onTap: () {
                        InfoRepo.CurrentModule =InfoRepo.user!.modules[index].moduleId;
                        showmodule();
                        },
                      leading: FloatingActionButton(
                        onPressed: () {
                          Alert(
                            context: context,
                            title: InfoRepo.user!.modules[index].moduleName,
                            desc: "${InfoRepo.user!.modules[index].moduleDescription}\nModule ID:${InfoRepo.user!.modules[index].moduleId}\nCreated at :${InfoRepo.user!.modules[index].createdAt}",
                            buttons: [
                              DialogButton(
                                onPressed: () {
                                  Navigator.pop(context);
                                },
                                color: Colors.red,
                                child: const Text(
                                  "OK",
                                  style: TextStyle(color: Colors.white, fontSize: 20),
                                ),
                              )
                            ],
                          ).show();

                        },
                        heroTag: null,
                        child: const Icon(
                            Icons.info
                        ),
                      ),
                      title: Text(InfoRepo.user!.modules[index].moduleName),
                      subtitle: Text("${InfoRepo.user!.modules[index].moduleDescription} ${InfoRepo.user!.modules[index].moduleId}"),
                      trailing: InfoRepo.user!.rule == 0 || InfoRepo.user!.rule == 1  ? FloatingActionButton(
                        onPressed: () {
                          addStudent(InfoRepo.user!.modules[index]);
                        },
                        heroTag: null,
                        child: const Icon(
                            Icons.add
                        ),
                      ) : null,
                    ),
                  );
                }
            ),
          ],
        ),
      ),
      floatingActionButton: Column(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
          if (InfoRepo.user!.rule !=0)  FloatingActionButton(
              onPressed: () {
              pushPage(context, const QuestionPage());
              },
              heroTag: null,
              child: const Icon(
                  Icons.message
              ),
            ),
            const SizedBox(
              height: 10,
            ),

          if (InfoRepo.user!.rule == 0)  FloatingActionButton(
              onPressed: () => {
                showReportFeedback()
              },
              heroTag: null,
              child: const Icon(
                  Icons.feedback
              ),
            )
          ]
      ),
    );
  }

  Future<void> handleClick(String value) async {
    switch (value){
      case 'Show my info':
        Alert(
          context: context,
          type: AlertType.info,
          title: "Your Info",
          desc: "First name: ${InfoRepo.user!.firstname}\nLast name: ${InfoRepo.user!.lastname}\nPersonal ID: ${InfoRepo.user!.personalCode}\nRole: ${ UserRule.values [InfoRepo.user!.rule]}",
          buttons: [
            DialogButton(
              onPressed: () => Navigator.pop(context),
              width: 120,
              child: const Text(
                "OK",
                style: TextStyle(color: Colors.white, fontSize: 20),
              ),
            )
          ],
        ).show();
        break;
      case 'Refresh':
        Common data =  await REST_Service.REST_GETUSER(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, InfoRepo.user!.personalCode.toString());
        if (data.statusCode == 200) {
          Alert(
            context: context,
            type: AlertType.success,
            title: "Success",
            desc: "Refresh Successful",
            buttons: [
              DialogButton(
                onPressed: () => Navigator.pop(context),
                width: 120,
                child: const Text(
                  "OK",
                  style: TextStyle(color: Colors.white, fontSize: 20),
                ),
              )
            ],
          ).show();
          InfoRepo.user =   User.fromRawJson( data.body);
          setState(() {});
        } else {
          EasyLoading.showError(data.description);
        }
        break;
        case 'Add New Module':
          if (InfoRepo.user!.rule != 0){
            Alert(
              context: context,
              type: AlertType.error,
              title: "Access Denied",
              desc: "You are not allowed to add new modules",
              buttons: [
                DialogButton(
                  onPressed: () => Navigator.pop(context),
                  width: 120,
                  child: const Text(
                    "OK",
                    style: TextStyle(color: Colors.white, fontSize: 20),
                  ),
                )
              ],
            ).show();
          }
          else {
            Navigator.push(context, MaterialPageRoute(builder: (context) => AddModulePage()));
          }
          break;
      case 'Generate New Invitation Code':
        if (InfoRepo.user!.rule != 0){
          Alert(
            context: context,
            type: AlertType.error,
            title: "Access Denied",
            desc: "You are not allowed to add new invitation code",
            buttons: [
              DialogButton(
                onPressed: () => Navigator.pop(context),
                width: 120,
                child: const Text(
                  "OK",
                  style: TextStyle(color: Colors.white, fontSize: 20),
                ),
              )
            ],
          ).show();
        }
        else {
         pushPage(context, const AddInvitationCode());
        }
        break;
      case  'Make Feedback About a Teacher':
        if (InfoRepo.user!.rule != 3){
          Alert(
            context: context,
            type: AlertType.error,
            title: "Access Denied",
            desc: "Only students can make feedback",
            buttons: [
              DialogButton(
                onPressed: () => Navigator.pop(context),
                width: 120,
                child: const Text(
                  "OK",
                  style: TextStyle(color: Colors.white, fontSize: 20),
                ),
              )
            ],
          ).show();
        }
        else {
          Navigator.pushNamed(context, '/feedback');
        }
        break;
    }
  }

  void addStudent(Module module) {
    InfoRepo.CurrentModule = module.moduleId;
    pushPage(context, const AddStudentPage());
  }

  void showmodule(){
     pushPage(context, const LecturesListPage());
  }

  showReportFeedback() async {
    Common data = await  REST_Service.REST_GETFEEDBACKREPORT();
    if (data.statusCode == 200) {
      EasyLoading.showSuccess("Refreshed");
      InfoRepo.feedbackReport =  FeedbackReports.fromJson (data.body);
      pushPage(context, const FeedbackReportPage());
    } else {
      EasyLoading.showError(data.description);
    }
  }
}
