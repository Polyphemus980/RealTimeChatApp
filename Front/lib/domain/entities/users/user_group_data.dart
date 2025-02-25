import 'package:chatapp_frontend/data/dto/users/user_group_data_dto.dart';

class UserGroupData {
  UserGroupData({required this.nickname, required this.isAdmin});

  factory UserGroupData.fromDto(UserGroupDataDto dto) =>
      UserGroupData(nickname: dto.nickname, isAdmin: dto.isAdmin);
  final String? nickname;
  final bool isAdmin;
}
