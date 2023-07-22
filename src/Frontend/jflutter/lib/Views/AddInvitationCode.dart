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

TextEditingController invitationCode = TextEditingController();
TextEditingController validationCount = TextEditingController();
class AddInvitationCode extends StatefulWidget {
  const AddInvitationCode({Key? key}) : super(key: key);

  @override
  State<AddInvitationCode> createState() => _MyStatefulWidgetState();
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

class _MyStatefulWidgetState extends State<AddInvitationCode> {
  late int selectedIndex;  //where I want to store the selected index

  List<DropdownMenuItem<String>> dropdownItems = [];
  String? SelectedRule;

  @override
  Widget build(BuildContext context)  {

    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:  const Text("Adding a New Invitation Code"),
         ),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:  DropdownButton<String>(
                    hint: const Text("Select a Rule"),
                    value: SelectedRule,
                    icon: const Icon(Icons.arrow_downward),
                    elevation: 16,
                    style: const TextStyle(color: Colors.deepPurple),
                    underline: Container(
                      height: 2,
                      color: Colors.deepPurpleAccent,
                    ),
                    onChanged: (String? newValue) {
                      setState(() {
                        SelectedRule = newValue!;
                      });
                    },
                    items: <String>['Admin', 'Teacher', 'Student']
                        .map<DropdownMenuItem<String>>((String value) {
                      return DropdownMenuItem<String>(
                        value: value,
                        child: Text(value),
                      );
                    }).toList(),
                  ),),
                Container(
                    alignment: Alignment.center,
                    padding: const EdgeInsets.all(10),
                    child: TextField(
                      controller: invitationCode,
                      decoration: const InputDecoration(
                        border: OutlineInputBorder(),
                        labelText: 'Invitation Code',
                      ),
                    )),
                Container(
                  padding: const EdgeInsets.all(10),
                  child:  TextField(
                    keyboardType: TextInputType.number,
                    controller: validationCount,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Validation Count',
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
                ),
              ],
            )),
      ),
    );


  }
  void addModule() async{
    if (invitationCode.text.isEmpty || validationCount.text.isEmpty || SelectedRule == null ) {
      EasyLoading.showError('Please fill all the fields');
    } else {
      EasyLoading.show(status: 'Adding...');
      int rule = 0;
      if (SelectedRule == 'Admin') {
        rule = 0;
      } else if (SelectedRule == 'Teacher') {
        rule = 1;
      } else if (SelectedRule == 'Student') {
        rule = 2;
      }
      Common data = await  REST_Service.REST_ADDINVITATIONCODE(InfoRepo.user!.personalCode.toString(),InfoRepo.user!.password, invitationCode.text, rule ,  int.parse(validationCount.text ) );
      if (data.statusCode == 200){
        EasyLoading.showSuccess(data.description);
      } else {
        EasyLoading.showError(data.description);
      }
    }
  }
}