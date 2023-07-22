import 'package:flutter/foundation.dart' show Key;
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import '../Utilities/REST_Service.dart';

TextEditingController feedbackString = TextEditingController();
class FeedbackPage extends StatefulWidget {
  const FeedbackPage({Key? key}) : super(key: key);

  @override
  State<FeedbackPage> createState() => _MyStatefulWidgetState();
}


class _MyStatefulWidgetState extends State<FeedbackPage> {


  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:   Text("Feedback for module  ${InfoRepo.CurrentModule}"),),
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
                    controller: feedbackString,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Feedback',
                    ),
                  ),),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                    child: const Text('Send Feedback'),
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
    if (feedbackString.text.isEmpty  ) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Sending feedback...');
      Common data = await  REST_Service.REST_SENDFEEDBACK(InfoRepo.user!.personalCode.toString(), InfoRepo.user!.password, feedbackString.text, InfoRepo.CurrentModule.toString());
      if (data.statusCode == 200) {
        EasyLoading.showSuccess(data.description);
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}