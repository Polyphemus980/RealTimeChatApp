import 'package:json_annotation/json_annotation.dart';

part 'user_group_data_dto.g.dart';

@JsonSerializable()
class UserGroupDataDto {
  UserGroupDataDto({required this.nickname, required this.isAdmin});

  factory UserGroupDataDto.fromJson(Map<String, dynamic> json) =>
      _$UserGroupDataDtoFromJson(json);

  Map<String, dynamic> toJson() => _$UserGroupDataDtoToJson(this);

  final String? nickname;
  final bool isAdmin;
}
