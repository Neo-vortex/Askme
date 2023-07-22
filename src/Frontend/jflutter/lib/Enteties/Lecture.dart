import 'dart:convert';
class Lecture {
  Lecture({
    required this.id,
    required this.lectureMaterial,
    required this.secretCode,
    required this.lectureDate,
  });

  int id;
  String lectureMaterial;
  int secretCode;
  DateTime lectureDate;

  Lecture copyWith({
    required int id,
    required String lectureMaterial,
    required int secretCode,
    required DateTime lectureDate,
  }) =>
      Lecture(
        id: id ?? this.id,
        lectureMaterial: lectureMaterial ?? this.lectureMaterial,
        secretCode: secretCode ?? this.secretCode,
        lectureDate: lectureDate ?? this.lectureDate,
      );

  factory Lecture.fromRawJson(String str) => Lecture.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory Lecture.fromJson(Map<String, dynamic> json) => Lecture(
    id: json["_id"],
    lectureMaterial: json["LectureMaterial"],
    secretCode: json["SecretCode"],
    lectureDate: DateTime.parse(json["LectureDate"]),
  );

  Map<String, dynamic> toJson() => {
    "_id": id,
    "LectureMaterial": lectureMaterial,
    "SecretCode": secretCode,
    "LectureDate": lectureDate.toIso8601String(),
  };
}
