// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'conversation_message_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ConversationMessageDto _$ConversationMessageDtoFromJson(
        Map<String, dynamic> json) =>
    ConversationMessageDto(
      id: (json['id'] as num).toInt(),
      receivers: (json['receivers'] as List<dynamic>)
          .map((e) => MessageReceiverDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      content: json['content'] as String,
      senderId: json['senderId'] as String,
      sentAt: DateTime.parse(json['sentAt'] as String),
    );

Map<String, dynamic> _$ConversationMessageDtoToJson(
        ConversationMessageDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'receivers': instance.receivers,
      'content': instance.content,
      'senderId': instance.senderId,
      'sentAt': instance.sentAt.toIso8601String(),
    };
