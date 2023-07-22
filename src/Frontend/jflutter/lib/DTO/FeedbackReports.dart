// To parse this JSON data, do
//
//     final feedbackReports = feedbackReportsFromMap(jsonString);

import 'dart:convert';

class FeedbackReports {
  FeedbackReports({
    required this.reports,
  });

  List<Report> reports;

  FeedbackReports copyWith({
    required List<Report> reports,
  }) =>
      FeedbackReports(
        reports: reports ?? this.reports,
      );

  factory FeedbackReports.fromJson(String str) => FeedbackReports.fromMap(json.decode(str));

  String toJson() => json.encode(toMap());

  factory FeedbackReports.fromMap(Map<String, dynamic> json) => FeedbackReports(
    reports: json["Reports"] = List<Report>.from(json["Reports"].map((x) => Report.fromMap(x))),
  );

  Map<String, dynamic> toMap() => {
    "Reports": reports == null ? null : List<dynamic>.from(reports.map((x) => x.toMap())),
  };
}

class Report {
  Report({
    required this.teacherName,
    required this.totalFeedbacks,
    required this.validFeedbacks,
    required this.invalidFeedbacks,
    required this.positiveValidFeedbacks,
    required this.negativeValidFeedbacks,
    required this.neutralValidFeedbacks,
    required this.positiveTotalFeedbacks,
    required this.negativeTotalFeedbacks,
    required this.neutralTotalFeedbacks,
    required this.feedbacksWithBenefits,
  });

  String teacherName;
  int totalFeedbacks;
  int validFeedbacks;
  int invalidFeedbacks;
  int positiveValidFeedbacks;
  int negativeValidFeedbacks;
  int neutralValidFeedbacks;
  int positiveTotalFeedbacks;
  int negativeTotalFeedbacks;
  int neutralTotalFeedbacks;
  List<String> feedbacksWithBenefits;

  Report copyWith({
    required String teacherName,
    required int totalFeedbacks,
    required int validFeedbacks,
    required int invalidFeedbacks,
    required int positiveValidFeedbacks,
    required int negativeValidFeedbacks,
    required int neutralValidFeedbacks,
    required int positiveTotalFeedbacks,
    required int negativeTotalFeedbacks,
    required int neutralTotalFeedbacks,
    required List<String> feedbacksWithBenefits,
  }) =>
      Report(
        teacherName: teacherName ?? this.teacherName,
        totalFeedbacks: totalFeedbacks ?? this.totalFeedbacks,
        validFeedbacks: validFeedbacks ?? this.validFeedbacks,
        invalidFeedbacks: invalidFeedbacks ?? this.invalidFeedbacks,
        positiveValidFeedbacks: positiveValidFeedbacks ?? this.positiveValidFeedbacks,
        negativeValidFeedbacks: negativeValidFeedbacks ?? this.negativeValidFeedbacks,
        neutralValidFeedbacks: neutralValidFeedbacks ?? this.neutralValidFeedbacks,
        positiveTotalFeedbacks: positiveTotalFeedbacks ?? this.positiveTotalFeedbacks,
        negativeTotalFeedbacks: negativeTotalFeedbacks ?? this.negativeTotalFeedbacks,
        neutralTotalFeedbacks: neutralTotalFeedbacks ?? this.neutralTotalFeedbacks,
        feedbacksWithBenefits: feedbacksWithBenefits ?? this.feedbacksWithBenefits,
      );

  factory Report.fromJson(String str) => Report.fromMap(json.decode(str));

  String toJson() => json.encode(toMap());

  factory Report.fromMap(Map<String, dynamic> json) => Report(
    teacherName: json["TeacherName"],
    totalFeedbacks: json["TotalFeedbacks"],
    validFeedbacks: json["ValidFeedbacks"],
    invalidFeedbacks: json["InvalidFeedbacks"],
    positiveValidFeedbacks: json["PositiveValidFeedbacks"],
    negativeValidFeedbacks: json["NegativeValidFeedbacks"],
    neutralValidFeedbacks: json["NeutralValidFeedbacks"],
    positiveTotalFeedbacks: json["PositiveTotalFeedbacks"],
    negativeTotalFeedbacks: json["NegativeTotalFeedbacks"],
    neutralTotalFeedbacks: json["NeutralTotalFeedbacks"],
    feedbacksWithBenefits: json["FeedbacksWithBenefits"] =  List<String>.from(json["FeedbacksWithBenefits"].map((x) => x)),
  );

  Map<String, dynamic> toMap() => {
    "TeacherName": teacherName,
    "TotalFeedbacks": totalFeedbacks,
    "ValidFeedbacks": validFeedbacks,
    "InvalidFeedbacks": invalidFeedbacks,
    "PositiveValidFeedbacks": positiveValidFeedbacks,
    "NegativeValidFeedbacks": negativeValidFeedbacks,
    "NeutralValidFeedbacks": neutralValidFeedbacks,
    "PositiveTotalFeedbacks": positiveTotalFeedbacks,
    "NegativeTotalFeedbacks": negativeTotalFeedbacks,
    "NeutralTotalFeedbacks": neutralTotalFeedbacks,
    "FeedbacksWithBenefits": feedbacksWithBenefits == null ? null : List<dynamic>.from(feedbacksWithBenefits.map((x) => x)),
  };

  Map<String, double> GetPie1(){
    Map<String, double> pieMap = {
      "Valid Positive": positiveValidFeedbacks.toDouble() ,
      "Valid Negative": negativeValidFeedbacks.toDouble(),
      "Valid Neutral": neutralValidFeedbacks.toDouble() ,
    };
    return pieMap;
  }

  Map<String, double> GetPie2(){
    Map<String, double> pieMap = {
      "Total Positive": positiveTotalFeedbacks.toDouble() ,
      "Total Negative": negativeTotalFeedbacks.toDouble(),
      "Total Neutral": neutralTotalFeedbacks.toDouble() ,
    };
    return pieMap;
  }


}
