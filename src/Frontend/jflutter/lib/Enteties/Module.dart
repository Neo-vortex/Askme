// To parse this JSON data, do
//
//     final module = moduleFromJson(jsonString);

import 'dart:convert';
import 'Lecture.dart';
class Module {
  Module({
    required this.id,
    required this.moduleId,
    required this.moduleName,
    required this.moduleDescription,
    required this.lectures,
    required this.createdAt,
  });

  int id;
  int moduleId;
  String moduleName;
  String moduleDescription;
  List<Lecture> lectures;
  DateTime createdAt;

  Future<Module> copyWith({
    required int id,
    required int moduleId,
    required String moduleName,
    required String moduleDescription,
    required List<Lecture> lectures,
    required DateTime createdAt,
  }) async =>
      Module(
        id: id ?? this.id,
        moduleId: moduleId ?? this.moduleId,
        moduleName: moduleName ?? this.moduleName,
        moduleDescription: moduleDescription ?? this.moduleDescription,
        lectures: lectures ?? this.lectures,
        createdAt: createdAt ?? this.createdAt,
      );

  factory Module.fromRawJson(String str) => Module.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory Module.fromJson(Map<String, dynamic> json) => Module(
    id: json["_id"],
    moduleId: json["ModuleID"],
    moduleName: json["ModuleName"],
    moduleDescription: json["ModuleDescription"],
    lectures: List<Lecture>.from(json["Lectures"].map((x) => Lecture.fromJson(x))),
    createdAt: DateTime.parse(json["CreatedAt"]),
  );

  Map<String, dynamic> toJson() => {
    "_id": id,
    "ModuleID": moduleId,
    "ModuleName": moduleName,
    "ModuleDescription": moduleDescription,
    "Lectures": List<dynamic>.from(lectures.map((x) => x.toJson())),
    "CreatedAt": createdAt.toIso8601String(),
  };
}
