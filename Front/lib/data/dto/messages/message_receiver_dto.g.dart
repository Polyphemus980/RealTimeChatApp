// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'message_receiver_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MessageReceiverDto _$MessageReceiverDtoFromJson(Map<String, dynamic> json) =>
    MessageReceiverDto(
      userId: json['userId'] as String,
      status: $enumDecode(_$MessageStatusEnumMap, json['status']),
    );

Map<String, dynamic> _$MessageReceiverDtoToJson(MessageReceiverDto instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'status': _$MessageStatusEnumMap[instance.status]!,
    };

const _$MessageStatusEnumMap = {
  MessageStatus.sent: 'sent',
  MessageStatus.delivered: 'delivered',
  MessageStatus.read: 'read',
};
