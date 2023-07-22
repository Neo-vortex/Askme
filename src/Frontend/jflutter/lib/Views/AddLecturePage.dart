import 'package:flutter/foundation.dart' show Key;
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import '../Utilities/REST_Service.dart';

TextEditingController lectureMaterial = TextEditingController();
TextEditingController lectureSecret = TextEditingController();
class AddLecturePage extends StatefulWidget {
  const AddLecturePage({Key? key}) : super(key: key);

  @override
  State<AddLecturePage> createState() => _MyStatefulWidgetState();
}


class _MyStatefulWidgetState extends State<AddLecturePage> {


  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:   Text("Adding a New lecture to module ${InfoRepo.CurrentModule}"),),
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
                    controller: lectureMaterial,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Lecture Material',
                    ),
                  ),),
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    controller: lectureSecret,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Lecture Code',
                    ),
                  ),),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                    child: const Text('Add'),
                    onPressed: () {
                      addLecture();
                    },
                  ),
                ),
              ],
            )),
      ),
    );


  }
  void addLecture() async{
    if (lectureMaterial.text.isEmpty || lectureSecret.text.isEmpty ) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Adding...');
      Common data = await  REST_Service.REST_ADDLECTURE(InfoRepo.user!.personalCode.toString(),InfoRepo.user!.password, lectureMaterial.text , lectureSecret.text, InfoRepo.CurrentModule.toString());
      if (data.statusCode == 200) {
        EasyLoading.showSuccess(data.description);
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}