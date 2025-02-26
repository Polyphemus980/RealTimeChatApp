import 'package:chatapp_frontend/data/dto/users/user_dto.dart';

import 'user_group_data.dart';

class ChatUser {
  ChatUser({
    required this.id,
    required this.displayName,
    required this.groupData,
  });

  factory ChatUser.fromDto(UserDto dto) => ChatUser(
        id: dto.id,
        displayName: dto.displayName,
        groupData: dto.groupData == null
            ? null
            : UserGroupData.fromDto(dto.groupData!),
      );

  final String id;
  final String displayName;
  final UserGroupData? groupData;

  ChatUser copyWith({
    String? id,
    String? displayName,
    UserGroupData? groupData,
  }) {
    return ChatUser(
      id: id ?? this.id,
      displayName: displayName ?? this.displayName,
      groupData: groupData ?? this.groupData,
    );
  }
}
