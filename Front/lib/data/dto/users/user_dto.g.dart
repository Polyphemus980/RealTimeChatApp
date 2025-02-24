// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserDto _$UserDtoFromJson(Map<String, dynamic> json) => UserDto(
      id: json['id'] as String,
      displayName: json['displayName'] as String,
      groupData: json['groupData'] == null
          ? null
          : UserGroupDataDto.fromJson(
              json['groupData'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$UserDtoToJson(UserDto instance) => <String, dynamic>{
      'id': instance.id,
      'displayName': instance.displayName,
      'groupData': instance.groupData,
    };
