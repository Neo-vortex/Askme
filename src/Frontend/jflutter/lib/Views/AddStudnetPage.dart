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
class AddStudentPage extends StatefulWidget {
  const AddStudentPage({Key? key}) : super(key: key);

  @override
  State<AddStudentPage> createState() => _MyStatefulWidgetState();
}


class _MyStatefulWidgetState extends State<AddStudentPage> {
  late int selectedIndex;  //where I want to store the selected index

  List<DropdownMenuItem<String>> dropdownItems = [];
  String? SelectedStudentName;
  int selectedStudentID = -1;

  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:   Text("Adding a New student to module ${InfoRepo.CurrentModule}"),),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.all(10),
                  child:  DropdownButtonHideUnderline(
                    child: DropdownButton<String>(
                      hint: const Text("Select a Student"),
                      value: SelectedStudentName,
                      isDense: true,
                      onChanged: (String? newValue) {
                        setState(() {
                          selectedStudentID = int.parse(newValue!);
                          SelectedStudentName = newValue.toString();
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
                      addStudent();
                    },
                  ),
                ),  Container(
                  padding: const EdgeInsets.all(10),
                  child:RaisedButton(
                      child: const Text('Refresh Student List'),
                      onPressed:
                          () async {
                        var students = (await  REST_Service.REST_GETALLSTUDENTS(InfoRepo.user!.personalCode.toString() , InfoRepo.user!.password));
                        Iterable l = json.decode(students.body);
                        List<User> studentsList = List<User>.from(l.map((model)=> User.fromJson(model)));
                        dropdownItems.clear();
                        for (var i = 0; i < studentsList.length; i++) {
                          dropdownItems.add(DropdownMenuItem(
                            value: studentsList[i].personalCode.toString(),
                            child: Text ("${studentsList[i].firstname}  ${studentsList[i].lastname}") ,
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
  void addStudent() async{
    if (SelectedStudentName == null) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Adding...');
      Common data = await  REST_Service.REST_ADDSTUDENT(InfoRepo.user!.personalCode.toString(),InfoRepo.user!.password, selectedStudentID.toString() , InfoRepo.CurrentModule.toString());
      if (data.statusCode == 200) {
        EasyLoading.showSuccess(data.description);
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}