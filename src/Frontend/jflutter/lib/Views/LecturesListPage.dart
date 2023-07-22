
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Enteties/Module.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:jflutter/Utilities/Utilities.dart';
import 'package:jflutter/Views/ActualLecturePage.dart';
import 'package:jflutter/Views/FeedbackPage.dart';
import 'package:jflutter/Views/LoginPage.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import '../Utilities/Utilities.dart';
import '../DTO/Common.dart';
import '../Enteties/User.dart';
import '../Enteties/userrule.dart';
import '../Utilities/REST_Service.dart';
import 'AddLecturePage.dart';
import 'AddModulePage.dart';
import 'AskQpage.dart';

class LecturesListPage extends StatefulWidget {
  const LecturesListPage({Key? key}) : super(key: key);

  @override
  State<LecturesListPage> createState() => _MyStatefulWidgetState();
}
class _MyStatefulWidgetState extends State<LecturesListPage> {


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title:  Text('Lectures for ${InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).moduleName}'),
        actions: <Widget>[
          PopupMenuButton<String>(
            onSelected: handleClick,
            itemBuilder: (BuildContext context) {
              if (InfoRepo.user!.rule == 2){
                return {  'Refresh', 'Show Teacher info' , 'Make Feedback About a Teacher'}.map((String choice) {
                  return PopupMenuItem<String>(
                    value: choice,
                    child: Text(choice),
                  );
                }).toList();
              } else if (InfoRepo.user!.rule == 1){
                return {  'Refresh', 'Add new Lecture'}.map((String choice) {
                  return PopupMenuItem<String>(
                    value: choice,
                    child: Text(choice),
                  );
                }).toList();
              }else{
                return {'Refresh'}.map((String choice) {
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
                itemCount: InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).lectures.length,
                itemBuilder: (BuildContext ctxt, int index) {
                  return Card(
                    elevation: 5,
                    child: ListTile(
                      onTap: () {
                        InfoRepo.CurrentLecture =InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).lectures[index].id;
                       pushPage(context, const ActualLecturePage());
                      },
                      leading:  const Icon(Icons.book),
                      title: Text((index +1).toString()),
                      subtitle: Text(InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).lectures[index].lectureDate.toString()),
                      trailing: InfoRepo.user!.rule == 2 ? FloatingActionButton(
                        onPressed: () {
                          InfoRepo.CurrentLecture =InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).lectures[index].id;
                          askquestion(InfoRepo.user!.modules[index]);
                        },
                        heroTag: null,
                        child: const Icon(
                            Icons.question_answer_sharp
                        ),
                      ) : const Icon(Icons.abc),
                    ),
                  );
                }
            ),
          ],
        ),
      ),
    );
  }

  Future<void> handleClick(String value) async {
    switch (value){
      case 'Refresh':
        Common data =  await REST_Service.REST_GETUSER(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, InfoRepo.user!.personalCode.toString());
        if (data.statusCode == 200) {
          EasyLoading.showSuccess("Refreshed");
          InfoRepo.user =   User.fromRawJson( data.body);
          setState(() {});
        } else {
          EasyLoading.showError(data.description);
        }
        break;
      case 'Add new Lecture':
    pushPage(context, const AddLecturePage());
        break;
      case 'Show Teacher info':
        Common data =  await REST_Service.REST_FINDTEACHERFORMODULE(InfoRepo.CurrentModule.toString());
        if (data.statusCode == 200) {
          Alert(
            context: context,
            type: AlertType.info,
            title: "Teacher info",
            desc: data.body,
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
          setState(() {});
        } else {
          EasyLoading.showError(data.description);
        }
        break;
      case 'Make Feedback About a Teacher':
          pushPage(context, const FeedbackPage());

        break;
    }
  }

  void askquestion(Module module) {
   pushPage(context, const AskQpage());
  }

}
