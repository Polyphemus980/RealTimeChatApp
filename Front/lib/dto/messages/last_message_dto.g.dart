// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'last_message_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LastMessageDto _$LastMessageDtoFromJson(Map<String, dynamic> json) =>
    LastMessageDto(
      id: (json['id'] as num).toInt(),
      content: json['content'] as String,
      sentAt: DateTime.parse(json['sentAt'] as String),
      senderName: json['senderName'] as String,
    );

Map<String, dynamic> _$LastMessageDtoToJson(LastMessageDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'content': instance.content,
      'sentAt': instance.sentAt.toIso8601String(),
      'senderName': instance.senderName,
    };
