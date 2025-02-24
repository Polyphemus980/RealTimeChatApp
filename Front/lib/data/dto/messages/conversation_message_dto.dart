import 'package:json_annotation/json_annotation.dart';

import 'message_receiver_dto.dart';

part 'conversation_message_dto.g.dart';

@JsonSerializable()
class ConversationMessageDto {
  ConversationMessageDto({
    required this.id,
    required this.receivers,
    required this.content,
    required this.senderId,
    required this.sentAt,
  });

  factory ConversationMessageDto.fromJson(Map<String, dynamic> json) =>
      _$ConversationMessageDtoFromJson(json);

  Map<String, dynamic> toJson() => _$ConversationMessageDtoToJson(this);

  final int id;
  final List<MessageReceiverDto> receivers;
  final String content;
  final String senderId;
  final DateTime sentAt;
}
