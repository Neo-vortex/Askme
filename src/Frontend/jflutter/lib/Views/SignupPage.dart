import 'package:flutter/foundation.dart' show Key, kDebugMode;
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/DTO/Common.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:rflutter_alert/rflutter_alert.dart';
import '../Utilities/REST_Service.dart';
import '../Enteties/User.dart';


TextEditingController usernameController = TextEditingController();
TextEditingController passwordController = TextEditingController();
TextEditingController passwordControllerConfirm = TextEditingController();
TextEditingController inviteController = TextEditingController();

class SignupPage extends StatefulWidget {
  const SignupPage({Key? key}) : super(key: key);

  @override
  State<SignupPage> createState() => _MyStatefulWidgetState();
}

Future<void> Signupfunction(BuildContext context) async {
  if (passwordController.text != passwordControllerConfirm.text) {
    Alert(
        context: context,
        title: "Password not match",
        desc: "Please check your password again",
        type: AlertType.error,
        useRootNavigator: true).show();
    return;
  }
  EasyLoading.show(status: 'Signing up...');
  Common data =  await REST_Service.REST_SIGNUP(usernameController.text, passwordController.text , inviteController.text);
  if (data.statusCode == 200) {
    EasyLoading.showSuccess(data.description);
    InfoRepo.user =   User.fromRawJson( data.body);
  } else {
    EasyLoading.showError(data.description);
  }
}

class _MyStatefulWidgetState extends State<SignupPage> {

  @override
  Widget build(BuildContext context) {


    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:  Text(Const.title)),
        body: Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                    alignment: Alignment.center,
                    padding: const EdgeInsets.all(10),
                    child: const Text(
                      'AskMe!',
                      style: TextStyle(
                          color: Colors.blue,
                          fontWeight: FontWeight.w500,
                          fontSize: 30),
                    )),
                Container(
                    alignment: Alignment.center,
                    padding: const EdgeInsets.all(10),
                    child: const Text(
                      'Sign up',
                      style: TextStyle(fontSize: 20),
                    )),
                Container(
                  padding: const EdgeInsets.all(10),
                  child: TextField(
                    controller: usernameController,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Username',
                    ),
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  child: TextField(
                    obscureText: true,
                    controller: passwordController,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Password',
                    ),
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  child: TextField(
                    obscureText: true,
                    controller: passwordControllerConfirm,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Password Confirm',
                    ),
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  child: TextField(
                    obscureText: true,
                    controller: inviteController,
                    decoration: const InputDecoration(
                      border: OutlineInputBorder(),
                      labelText: 'Invitation Code',
                    ),
                  ),
                ),
                Container(
                  height: 10,
                  padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                ),
                Container(
                    height: 50,
                    padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                    child: ElevatedButton(
                      child: const Text('Signup'),
                      onPressed: () {
                        if (kDebugMode) {
                          print(usernameController.text);
                        }
                        if (kDebugMode) {
                          print(passwordController.text);
                        }
                        Signupfunction(context);
                      },
                    )
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: <Widget>[
                    const Text('Need Help?'),
                    TextButton(
                      child: const Text(
                        'Click Here',
                        style: TextStyle(fontSize: 20),
                      ),
                      onPressed: () {
                        Alert( context: context  , title: "Signup", desc: "Your username will be you teacher/student personal code\n Get invitation code from your admin/teacher", type: AlertType.info ,useRootNavigator: true).show();
                      },
                    )
                  ],
                ),
              ],
            )),
      ),
    );
  }

}