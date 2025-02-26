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
      currentUser:
          UserDto.fromJson(json['currentUser'] as Map<String, dynamic>),
      lastMessage: json['lastMessage'] == null
          ? null
          : LastMessageDto.fromJson(
              json['lastMessage'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$ConversationListDtoToJson(
        ConversationListDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'type': _$ConversationTypeEnumMap[instance.type]!,
      'members': instance.members,
      'currentUser': instance.currentUser,
      'lastMessage': instance.lastMessage,
    };

const _$ConversationTypeEnumMap = {
  ConversationType.group: 'group',
  ConversationType.direct: 'direct',
};
