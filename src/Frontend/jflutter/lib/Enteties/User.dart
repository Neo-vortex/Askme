// To parse this JSON data, do
//
//     final user = userFromJson(jsonString);

import 'dart:convert';
import 'Module.dart';
import 'Question.dart';
class User {
  User({
    required this.id,
    required this.personalCode,
    required this.rule,
    required this.password,
    required this.firstname,
    required this.lastname,
    required this.modules,
    required this.questions,
  });

  int id;
  int personalCode;
  int rule;
  String password;
  String firstname;
  String lastname;
  List<Module> modules;
  List<Question> questions;

  User copyWith({
    required int id,
    required int personalCode,
    required int rule,
    required String password,
    required String firstname,
    required String lastname,
     List<Module>? modules,
     List<Question>? questions,
  }) =>
      User(
        id: id ?? this.id,
        personalCode: personalCode ?? this.personalCode,
        rule: rule ?? this.rule,
        password: password ?? this.password,
        firstname: firstname ?? this.firstname,
        lastname: lastname ?? this.lastname,
        modules: modules ?? this.modules,
        questions: questions ?? this.questions,
      );

  factory User.fromRawJson(String str) => User.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory User.fromJson(Map<String, dynamic> json) {
    if (json["Modules"] == null){
      json["Modules"] = <dynamic>[];
    }
    if (json["Questions"]== null){
      json["Questions"] = <dynamic>[];
    }
   return User(
      id: json["_id"],
      personalCode: json["PersonalCode"],
      rule: json["Rule"],
      password: json["Password"],
      firstname: json["Firstname"],
      lastname: json["Lastname"],
      modules: List<Module>.from(json["Modules"].map((x) => Module.fromJson(x))),
      questions: List<Question>.from(json["Questions"].map((x) => Question.fromJson(x))),
    );
  }

  Map<String, dynamic> toJson() => {
    "_id": id,
    "PersonalCode": personalCode,
    "Rule": rule,
    "Password": password,
    "Firstname": firstname,
    "Lastname": lastname,
    "Modules": List<dynamic>.from(modules.map((x) => x.toJson())),
    "Questions": List<dynamic>.from(questions.map((x) => x.toJson())),
  };
}



