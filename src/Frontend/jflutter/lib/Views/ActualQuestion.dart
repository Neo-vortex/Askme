import 'package:flutter/foundation.dart' show Key;
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import '../Enteties/User.dart';
import '../Utilities/REST_Service.dart';


TextEditingController answearString = TextEditingController();
class ActualQuestion extends StatefulWidget {
  const ActualQuestion({Key? key}) : super(key: key);

  @override
  State<ActualQuestion> createState() => _MyStatefulWidgetState();
}


class _MyStatefulWidgetState extends State<ActualQuestion> {


  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:   Text("Question"),),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  Text(InfoRepo.user!.questions.firstWhere((element) => element.id == InfoRepo.CurrentQ).question),),


                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:   ListView.builder
                    (
                      scrollDirection: Axis.vertical,
                      shrinkWrap: true,
                      itemCount: InfoRepo.user!.questions.firstWhere((element) => element.id == InfoRepo.CurrentQ).answers.length,
                      itemBuilder: (BuildContext ctxt, int index) {
                        return Card(
                          elevation: 5,
                          child:Text(InfoRepo.user!.questions.firstWhere((element) => element.id == InfoRepo.CurrentQ).answers[index].answear),
                        );
                      }
                  ),),
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    maxLines: 20,
                    controller: answearString,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Answer',
                    ),
                  ),),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                    child: const Text('Send Answer'),
                    onPressed: () {
                      sendAnswer();
                    },
                  ),
                ),
              ],
            )),
      ),
    );


  }
  void sendAnswer() async{
    if (answearString.text.isEmpty  ) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Sending Question...');
      Common data = await  REST_Service.REST_ANSWERQ(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, answearString.text,InfoRepo.CurrentQ.toString());
      if (data.statusCode == 200) {
        EasyLoading.showSuccess(data.description);

        Common data2 =  await REST_Service.REST_GETUSER(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, InfoRepo.user!.personalCode.toString());
        if (data.statusCode == 200) {
          EasyLoading.showSuccess("Refreshed");
          InfoRepo.user =   User.fromRawJson( data2.body);
          setState(() {});
        } else {
          EasyLoading.showError(data.description);
        }

        setState(() {
          answearString.text = "";
        });
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}