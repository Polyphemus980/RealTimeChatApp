// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'conversation_list_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ConversationListDto _$ConversationListDtoFromJson(Map<String, dynamic> json) =>
    ConversationListDto(
      id: (json['id'] as num).toInt(),
      type: $enumDecode(_$ConversationTypeEnumMap, json['type']),
      members: (json['members'] as List<dynamic>)
          .map((e) => UserDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      lastMessages: (json['lastMessages'] as List<dynamic>)
          .map((e) => LastMessageDto.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$ConversationListDtoToJson(
        ConversationListDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'type': _$ConversationTypeEnumMap[instance.type]!,
      'members': instance.members,
      'lastMessages': instance.lastMessages,
    };

const _$ConversationTypeEnumMap = {
  ConversationType.group: 'group',
  ConversationType.direct: 'direct',
};
