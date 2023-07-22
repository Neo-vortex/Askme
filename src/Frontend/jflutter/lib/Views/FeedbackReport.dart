


import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:jflutter/Utilities/Utilities.dart';
import 'package:jflutter/Views/FeedbackDetailsPage.dart';
import '../DTO/Common.dart';
import '../DTO/FeedbackReports.dart';
import '../Utilities/Consts.dart';
import '../Utilities/REST_Service.dart';
import 'package:pie_chart/pie_chart.dart';


class FeedbackReportPage extends StatefulWidget {
  const FeedbackReportPage({Key? key}) : super(key: key);
  @override
  State<FeedbackReportPage> createState() => _MyStatefulWidgetState ();
}


void getFeedbacks() async{
  Common data = await  REST_Service.REST_GETFEEDBACKREPORT();
  if (data.statusCode == 200) {
    EasyLoading.showSuccess("Refreshed");
    InfoRepo.feedbackReport =  FeedbackReports.fromJson (data.body);
  } else {
    EasyLoading.showError(data.description);
  }
}

class _MyStatefulWidgetState extends State<FeedbackReportPage> {
  @override
  Widget build(BuildContext context)  {


    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      home: Scaffold(
          appBar: AppBar(title:  Text("View Teachers Feedback Report"),
            actions: <Widget>[
              PopupMenuButton<String>(
                onSelected: handleClick,
                itemBuilder: (BuildContext context) {
                  return {  'Refresh'}.map((String choice) {
                    return PopupMenuItem<String>(
                      value: choice,
                      child: Text(choice),
                    );
                  }).toList();
                },
              ),
            ],),
          body:
            Container(
              alignment: Alignment.center,
              padding: const EdgeInsets.all(10),
              child:   ListView.builder
                (
                  scrollDirection: Axis.vertical,
                  shrinkWrap: true,
                  itemCount: InfoRepo.feedbackReport?.reports?.length,
                  itemBuilder: (BuildContext ctxt, int index) {
                    return Card(
                      elevation: 5,
                      child: ListTile(
                        onTap: () {
                          InfoRepo.CurrentFeedback = index;
                          showFeedbackDetails();
                        },
                        leading: const Icon(
                            Icons.feedback_rounded
                        ),
                        title: Text(InfoRepo.feedbackReport?.reports[index]?.teacherName ?? "null"),
                        subtitle:  Text("Total : ${InfoRepo.feedbackReport?.reports[index]?.feedbacksWithBenefits.length}" ),

                      ),
                    );
                  }
              ),),
      ),
    );
  }
  void handleClick(String value) async {
    getFeedbacks();
        setState(() {
        });
  }
  void showFeedbackDetails() {
    pushPage(context, const FeedbackDetails());
  }

}

