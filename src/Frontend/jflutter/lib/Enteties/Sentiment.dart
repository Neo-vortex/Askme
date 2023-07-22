// To parse this JSON data, do
//
//     final sentiment = sentimentFromJson(jsonString);

import 'dart:convert';

class Sentiment {
  Sentiment({
    required this.veder,
    required this.ml,
  });

  final double veder;
  final double ml;

  Sentiment copyWith({
    required double veder,
    required double ml,
  }) =>
      Sentiment(
        veder: veder ?? this.veder,
        ml: ml ?? this.ml,
      );

  factory Sentiment.fromRawJson(String str) => Sentiment.fromJson(json.decode(str));

  String toRawJson() => json.encode(toJson());

  factory Sentiment.fromJson(Map<String, dynamic> json) => Sentiment(
    veder: json["Veder"] == null ? null : json["Veder"].toDouble(),
    ml: json["ML"] == null ? null : json["ML"].toDouble(),
  );

  Map<String, dynamic> toJson() => {
    "Veder": veder,
    "ML": ml,
  };
}
