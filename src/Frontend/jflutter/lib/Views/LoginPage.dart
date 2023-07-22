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

TextEditingController usernameController = TextEditingController();
TextEditingController passwordController = TextEditingController();

class LoginPage extends StatefulWidget {
  const LoginPage({Key? key}) : super(key: key);

  @override
  State<LoginPage> createState() => _MyStatefulWidgetState();
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

class _MyStatefulWidgetState extends State<LoginPage> {

  @override
  Widget build(BuildContext context) {
    return Padding(
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
                  'Sign in',
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
/*            Container(
              padding: const EdgeInsets.all(10),
              child: TextField(
                obscureText: true,
                controller: inviteController,
                decoration: const InputDecoration(
                  border: OutlineInputBorder(),
                  labelText: 'Invitation Code',
                ),
              ),
            ),*/
            Container(
              height: 10,
              padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
            ),
            Container(
                height: 50,
                padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: ElevatedButton(
                  child: const Text('Login'),
                  onPressed: () {
                    if (kDebugMode) {
                      print(usernameController.text);
                    }
                    if (kDebugMode) {
                      print(passwordController.text);
                    }
                    login(context);
                  },
                )
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                const Text('Does not have account?'),
                TextButton(
                  child: const Text(
                    'Sign up',
                    style: TextStyle(fontSize: 20),
                  ),
                  onPressed: () {
                        pushPage(context, const SignupPage());
                    //signup screen
                  },
                )
              ],
            ),
          ],
        ));
  }

}