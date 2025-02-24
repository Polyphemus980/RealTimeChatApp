// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'single_conversation_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

SingleConversationDto _$SingleConversationDtoFromJson(
        Map<String, dynamic> json) =>
    SingleConversationDto(
      id: (json['id'] as num).toInt(),
      type: $enumDecode(_$ConversationTypeEnumMap, json['type']),
      members: (json['members'] as List<dynamic>)
          .map((e) => UserDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      messages: (json['messages'] as List<dynamic>)
          .map(
              (e) => ConversationMessageDto.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$SingleConversationDtoToJson(
        SingleConversationDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'type': _$ConversationTypeEnumMap[instance.type]!,
      'members': instance.members,
      'messages': instance.messages,
    };

const _$ConversationTypeEnumMap = {
  ConversationType.group: 'group',
  ConversationType.direct: 'direct',
};
