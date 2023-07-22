
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Utilities/Consts.dart';
import 'package:rflutter_alert/rflutter_alert.dart';

import 'Views/LoginPage.dart';



void main() => runApp(const MyApp());

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      title: Const.title,
      home: Scaffold(
        appBar: AppBar(title:  Text(Const.title)),
        body: const LoginPage(),
      ),
    );
  }
}
