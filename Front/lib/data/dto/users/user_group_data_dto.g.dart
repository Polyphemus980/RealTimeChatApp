// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_group_data_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserGroupDataDto _$UserGroupDataDtoFromJson(Map<String, dynamic> json) =>
    UserGroupDataDto(
      nickname: json['nickname'] as String?,
      isAdmin: json['isAdmin'] as bool,
    );

Map<String, dynamic> _$UserGroupDataDtoToJson(UserGroupDataDto instance) =>
    <String, dynamic>{
      'nickname': instance.nickname,
      'isAdmin': instance.isAdmin,
    };
