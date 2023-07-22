import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_easyloading/flutter_easyloading.dart';
import 'package:jflutter/Utilities/InfoRepo.dart';
import 'package:pie_chart/pie_chart.dart';
import '../Utilities/Consts.dart';


class FeedbackDetails extends StatefulWidget {
  const FeedbackDetails({Key? key}) : super(key: key);
  @override
  State<FeedbackDetails> createState() => _MyStatefulWidgetState ();
}




class _MyStatefulWidgetState extends State<FeedbackDetails> {
  @override
  Widget build(BuildContext context)  {


    return MaterialApp(
      builder: EasyLoading.init(),
      theme: ThemeData( primarySwatch: Colors.purple),
      home: Scaffold(
        appBar: AppBar(title:  Text("Teacher Full Feedback Report"),
          actions: const <Widget>[
          ],),
        body:
        Padding(
            padding: const EdgeInsets.all(10),
            child: ListView(
              children: <Widget>[
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:     PieChart(dataMap: InfoRepo?.feedbackReport?.reports[InfoRepo.CurrentFeedback]?.GetPie2() ??  <String, double>{}),),
                Container(
                    alignment: Alignment.center,
                    padding: const EdgeInsets.all(10),
                    child:     const Card(elevation:5.0,child:Text("The pie chart above shows how many feedbacks are Positive/Negative/Neutral after we processed the Text using Machine Learning Algorithms and Vader Sentiment Intensity Analyzer "))),

                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:     PieChart(dataMap: InfoRepo?.feedbackReport?.reports[InfoRepo.CurrentFeedback]?.GetPie1() ??  <String, double>{}),),
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:     const Card(elevation:5.0,child:Text("The pie chart above shows how many feedbacks are actually valid after we use Naive Bayes Classifier and train our model. Our model is trained to be able to check the validity of the Feedback according to all 4 student grades, lecture attendance, lab attendance and questions asked through our application."))),
                Container(
                    alignment: Alignment.centerLeft,
                    padding: const EdgeInsets.all(10),
                    child: const Text(
                      'All Feedbacks Left By Students with Details:',
                      style: TextStyle(fontSize: 15),
                    )),
                Container(
                  alignment: Alignment.center,
                  padding: const EdgeInsets.all(10),
                  child:   ListView.builder
                    (
                      scrollDirection: Axis.vertical,
                      shrinkWrap: true,
                      itemCount: InfoRepo.feedbackReport?.reports[InfoRepo.CurrentFeedback]?.feedbacksWithBenefits.length,
                      itemBuilder: (BuildContext ctxt, int index) {
                        return Card(
                          elevation: 5,
                          child:Text(InfoRepo.feedbackReport?.reports[InfoRepo.CurrentFeedback]?.feedbacksWithBenefits![index] ?? "null"),
                        );
                      }
                  ),),

              ],
            ))
,
      ),
    );
  }


}

