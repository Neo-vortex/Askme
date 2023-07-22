
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Enteties/Module.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:jflutter/Utilities/Utilities.dart';
import 'package:jflutter/Views/ActualQuestion.dart';
import 'package:jflutter/Views/AddInvitationCode.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import '../DTO/Common.dart';
import '../Enteties/User.dart';
import '../Enteties/userrule.dart';
import '../Utilities/REST_Service.dart';
import 'AddModulePage.dart';
import 'AddStudnetPage.dart';
import 'LecturesListPage.dart';

class QuestionPage extends StatefulWidget {
  const QuestionPage({Key? key}) : super(key: key);

  @override
  State<QuestionPage> createState() => _MyStatefulWidgetState();
}
class _MyStatefulWidgetState extends State<QuestionPage> {


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        automaticallyImplyLeading: true,
        title: const Text('Questions'),
      ),
      body: Center(
        child: Column(
          children:  <Widget>[
            ListView.builder
              (
                scrollDirection: Axis.vertical,
                shrinkWrap: true,
                itemCount: InfoRepo.user!.questions.length,
                itemBuilder: (BuildContext ctxt, int index) {
                  return Card(
                    elevation: 5,
                    child: ListTile(
                      onTap: () {
                        InfoRepo.CurrentQ =InfoRepo.user!.questions[index].id;
                        showQ();
                      },
                      title: Text(InfoRepo.user!.questions[index].question),
                      subtitle: Text( "Was asked on Lecture id : ${InfoRepo.user!.questions[index].lecture.id}"),
                      trailing:  FloatingActionButton(
                        onPressed: () {
                          InfoRepo.CurrentQ =InfoRepo.user!.questions[index].id;
                         Qinfo();
                        },
                        heroTag: null,
                        child: const Icon(
                            Icons.info
                        ),
                      ) ,
                    ),
                  );
                }
            ),
          ],
        ),
      ),
    );
  }

  void showQ() {

pushPage(context, const ActualQuestion());
  }

  void Qinfo() async {

    var result = await REST_Service.REST_GETMODULEBYLECTURE(InfoRepo.user!.questions.firstWhere((element) => element.id == InfoRepo.CurrentQ).lecture.id.toString());
    var module  = Module.fromRawJson(result.body);
    Alert(
      context: context,
      title: "Question info",
      desc: "Module name: ${module.moduleName}\nModule ID: ${module.moduleId}\nLecture ID : ${InfoRepo.user!.questions.firstWhere((element) => element.id == InfoRepo.CurrentQ).lecture.id}",
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
  }


}

