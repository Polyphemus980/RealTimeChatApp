import 'package:json_annotation/json_annotation.dart';

part 'last_message_dto.g.dart';

@JsonSerializable()
class LastMessageDto {
  LastMessageDto({
    required this.id,
    required this.content,
    required this.sentAt,
    required this.senderName,
  });

  factory LastMessageDto.fromJson(Map<String, dynamic> json) =>
      _$LastMessageDtoFromJson(json);

  Map<String, dynamic> toJson() => _$LastMessageDtoToJson(this);

  final int id;
  final String content;
  final DateTime sentAt;
  final String senderName;
}
