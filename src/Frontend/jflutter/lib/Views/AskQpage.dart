import 'package:flutter/foundation.dart' show Key;
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import '../Utilities/REST_Service.dart';

TextEditingController Qstring = TextEditingController();
class AskQpage extends StatefulWidget {
  const AskQpage({Key? key}) : super(key: key);

  @override
  State<AskQpage> createState() => _MyStatefulWidgetState();
}


class _MyStatefulWidgetState extends State<AskQpage> {


  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:   Text("Question form module  ${InfoRepo.CurrentModule} and lecture  ${InfoRepo.CurrentLecture}"),),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  height: 500,
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    maxLines: 20,
                    controller: Qstring,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Question',
                    ),
                  ),),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                    child: const Text('Send Question'),
                    onPressed: () {
                      AskQ();
                    },
                  ),
                ),
              ],
            )),
      ),
    );


  }
  void AskQ() async{
    if (Qstring.text.isEmpty  ) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Sending Question...');
      Common data = await  REST_Service.REST_ASKQ(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, Qstring.text, InfoRepo.CurrentLecture.toString(), InfoRepo.CurrentModule.toString());
      if (data.statusCode == 200) {
        EasyLoading.showSuccess(data.description);
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}