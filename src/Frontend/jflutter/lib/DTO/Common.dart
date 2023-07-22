// To parse this JSON data, do
//
//     final common = commonFromJson(jsonString);

import 'dart:convert';

Common commonFromJson(String str) => Common.fromJson(json.decode(str));

String commonToJson(Common data) => json.encode(data.toJson());

class Common {
  Common({
    required this.statusCode,
    required this.message,
    required this.description,
    required this.body,
  });

  int statusCode;
  String message;
  String description;
  String body;

  Common copyWith({
    required int statusCode,
    required String message,
    required String description,
    required String body,
  }) =>
      Common(
        statusCode: statusCode ?? this.statusCode,
        message: message ?? this.message,
        description: description ?? this.description,
        body: body ?? this.body,
      );

  factory Common.fromJson(Map<String, dynamic> json) => Common(
    statusCode: json["statusCode"],
    message: json["message"],
    description: json["description"],
    body: json["body"],
  );

  Map<String, dynamic> toJson() => {
    "StatusCode": statusCode,
    "Message": message,
    "Description": description,
    "Body": body,
  };
}
