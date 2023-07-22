import 'dart:convert';

class Answer {
  Answer({
    required this.id,
    required this.answear,
  });

  int id;
  String answear;

  Answer copyWith({
    required int id,
    required String answear,
  }) =>
      Answer(
        id: id ?? this.id,
        answear: answear ?? this.answear,
      );

  factory Answer.fromRawJson(String str) => Answer.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory Answer.fromJson(Map<String, dynamic> json) => Answer(
    id: json["_id"],
    answear: json["Answear"],
  );

  Map<String, dynamic> toJson() => {
    "_id": id,
    "Answear": answear,
  };
}
