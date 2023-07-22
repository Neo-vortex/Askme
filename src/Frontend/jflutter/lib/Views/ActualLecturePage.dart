
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Enteties/Module.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:jflutter/Utilities/Utilities.dart';


import '../Utilities/Consts.dart';
import 'SetPresentPage.dart';

class ActualLecturePage extends StatefulWidget {
  const ActualLecturePage({Key? key}) : super(key: key);
  @override
  State<ActualLecturePage> createState() => _MyStatefulWidgetState ();
}

class _MyStatefulWidgetState extends State<ActualLecturePage> {
  @override
  Widget build(BuildContext context) {



    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      home: Scaffold(
        appBar: AppBar(title:  Text("Lecture Info for Lecture: ${InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).lectures.firstWhere((element) => element.id == InfoRepo.CurrentLecture).id}"),

          actions: <Widget>[

            PopupMenuButton<String>(
              onSelected: handleClick,
              itemBuilder: (BuildContext context) {
                if (InfoRepo.user!.rule == 2){
                  return {  'Set present for this lecture'}.map((String choice) {
                    return PopupMenuItem<String>(
                      value: choice,
                      child: Text(choice),
                    );
                  }).toList();
                }
                return {  ''}.map((String choice) {
                  return PopupMenuItem<String>(
                    value: choice,
                    child: Text(choice),
                  );
                }).toList();
              },
            ),
          ],),
        body:
        Column(
          children: <Widget>[
            Text(""),



            Text(InfoRepo.user!.modules.firstWhere((element) => element.moduleId == InfoRepo.CurrentModule).lectures.firstWhere((element) => element.id == InfoRepo.CurrentLecture).lectureMaterial)

          ],
        )

      ),
    );
  }
  void handleClick(String value) {
    if (InfoRepo.user!.rule == 2){
      pushPage(context, const SetPresentPage());
    }
  }

}
