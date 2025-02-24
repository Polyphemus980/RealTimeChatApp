import 'package:chatapp_frontend/dto/users/user_group_data_dto.dart';
import 'package:json_annotation/json_annotation.dart';

part 'user_dto.g.dart';

@JsonSerializable()
class UserDto {
  UserDto({
    required this.id,
    required this.displayName,
    required this.groupData,
  });
  factory UserDto.fromJson(Map<String, dynamic> json) =>
      _$UserDtoFromJson(json);

  Map<String, dynamic> toJson() => _$UserDtoToJson(this);

  final String id;
  final String displayName;
  final UserGroupDataDto? groupData;
}
