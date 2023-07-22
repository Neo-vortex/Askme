import 'dart:convert';

import 'package:flutter/foundation.dart' show Key, kDebugMode;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:jflutter/Views/ModulesPage.dart';
import '../Utilities/Utilities.dart';
import '../Utilities/REST_Service.dart';
import '../Enteties/User.dart';
import 'SignupPage.dart';

TextEditingController modulename = TextEditingController();
TextEditingController moduledescription = TextEditingController();
TextEditingController moduleID = TextEditingController();
class AddModulePage extends StatefulWidget {
  const AddModulePage({Key? key}) : super(key: key);

  @override
  State<AddModulePage> createState() => _MyStatefulWidgetState();
}

Future<void> login(BuildContext context) async {
  EasyLoading.show(status: 'Logging in...');
  Common data =  await REST_Service.REST_LOGIN(usernameController.text, passwordController.text);
  if (data.statusCode == 200) {
    EasyLoading.showSuccess(data.description);
    InfoRepo.user =   User.fromRawJson( data.body);
    pushPage(context, const ModulesPage());
  } else {
    EasyLoading.showError(data.description);
  }

/*


    EasyLoading.showProgress(0.3, status: 'downloading...');
*/

/*  EasyLoading.showInfo(data);*/

/*    EasyLoading.showError('Failed with Error');

    EasyLoading.showInfo('Useful Information.');

    EasyLoading.showToast('Toast');

    EasyLoading.dismiss();*/

  /*Alert( context: context  , title: "x.toString()", desc: data, type: AlertType.info ,useRootNavigator: true).show();*/
}

class _MyStatefulWidgetState extends State<AddModulePage> {
  late int selectedIndex;  //where I want to store the selected index

  List<DropdownMenuItem<String>> dropdownItems = [];
  String? SelectedTeacherName;
  int selectedTeacherID = -1;

  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:  const Text("Adding a New Module"),
            automaticallyImplyLeading: true),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    controller: modulename,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Module Name',
                    ),
                  ),),
                Container(
                    alignment: Alignment.center,
                    padding: const EdgeInsets.all(10),
                    child: TextField(
                      controller: moduledescription,
                      decoration: const InputDecoration(
                        border: OutlineInputBorder(),
                        labelText: 'Module Description',
                      ),
                    )),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    keyboardType: TextInputType.number,
                    controller: moduleID,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Module ID',
                    ),
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:  DropdownButtonHideUnderline(
                    child: DropdownButton<String>(
                      hint: const Text("Select a teacher"),
                      value: SelectedTeacherName,
                      isDense: true,
                      onChanged: (String? newValue) {
                        setState(() {
                          selectedTeacherID = int.parse(newValue!);
                          SelectedTeacherName = newValue.toString();
                        });
                      },
                      items: dropdownItems,
                    ),
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                    child: const Text('Add'),
                    onPressed: () {
                      addModule();
                    },
                  ),
                ),  Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                      child: const Text('Refresh Teacher List'),
                      onPressed:
                          () async {
                        var teachers = (await  REST_Service.REST_GETALLTEACHERS());
                        Iterable l = json.decode(teachers.body);
                        List<User> teachersList = List<User>.from(l.map((model)=> User.fromJson(model)));
                        dropdownItems.clear();
                        for (var i = 0; i < teachersList.length; i++) {
                          dropdownItems.add(DropdownMenuItem(
                            value: teachersList[i].personalCode.toString(),
                            child: Text ("${teachersList[i].firstname}  ${teachersList[i].lastname}") ,
                          ));
                          setState(() { });
                        }
                      }),
                ),
              ],
            )),
      ),
    );

   
  }
void addModule() async{
  if (modulename.text.isEmpty || moduledescription.text.isEmpty || moduleID.text.isEmpty || selectedTeacherID == -1) {
    EasyLoading.showError('Please fill all the fields');
  } else {
    EasyLoading.show(status: 'Adding...');
    Common data = await  REST_Service.REST_ADDMODULE(InfoRepo.user!.personalCode.toString(),InfoRepo.user!.password, moduleID.text ,selectedTeacherID.toString(),moduledescription.text, modulename.text);
    if (data.statusCode == 200) {
      EasyLoading.showSuccess(data.description);
    } else {
      EasyLoading.showError(data.description);
    }
  }
}
}