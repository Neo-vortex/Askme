import 'dart:convert';

import 'Answer.dart';
import 'Lecture.dart';

class Question {
  Question({
    required this.id,
    required this.question,
    required this.lecture,
    required this.answers,
  });

  int id;
  String question;
  Lecture lecture;
  List<Answer> answers;

  Question copyWith({
    required int id,
    required String question,
    required Lecture lecture,
    required List<Answer> answers,
  }) =>
      Question(
        id: id ?? this.id,
        question: question ?? this.question,
        lecture: lecture ?? this.lecture,
        answers: answers ?? this.answers,
      );

  factory Question.fromRawJson(String str) => Question.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory Question.fromJson(Map<String, dynamic> json) => Question(
    id: json["_id"],
    question: json["question"],
    lecture: Lecture.fromJson(json["lecture"]),
    answers: List<Answer>.from(json["answers"].map((x) => Answer.fromJson(x))),
  );

  Map<String, dynamic> toJson() => {
    "_id": id,
    "question": question,
    "lecture": lecture.toJson(),
    "answers": List<dynamic>.from(answers.map((x) => x.toJson())),
  };
}
