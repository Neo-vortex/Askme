import 'package:flutter/foundation.dart' show Key;
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import '../Utilities/REST_Service.dart';

TextEditingController secretCode = TextEditingController();
class SetPresentPage extends StatefulWidget {
  const SetPresentPage({Key? key}) : super(key: key);

  @override
  State<SetPresentPage> createState() => _MyStatefulWidgetState();
}


class _MyStatefulWidgetState extends State<SetPresentPage> {


  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:   Text("Set Present for Lecture ${InfoRepo.CurrentLecture}"),),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  height: 100,
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    maxLines: 20,
                    controller: secretCode,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Secret Code',
                    ),
                  ),),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                    child: const Text('Set Present'),
                    onPressed: () {
                      addfeedback();
                    },
                  ),
                ),
              ],
            )),
      ),
    );


  }
  void addfeedback() async{
    if (secretCode.text.isEmpty  ) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Setting present...');
      Common data = await  REST_Service.REST_SETPRESENT(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, InfoRepo.CurrentLecture.toString(), secretCode.text);
      if (data.statusCode == 200) {
        EasyLoading.showSuccess(data.description);
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}