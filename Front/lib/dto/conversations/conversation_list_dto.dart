import 'package:chatapp_frontend/dto/messages/last_message_dto.dart';
import 'package:chatapp_frontend/dto/users/user_dto.dart';
import 'package:json_annotation/json_annotation.dart';

part 'conversation_list_dto.g.dart';

enum ConversationType {
  group,
  direct,
}

@JsonSerializable()
class ConversationListDto {
  ConversationListDto({
    required this.id,
    required this.type,
    required this.members,
    required this.lastMessage,
  });

  factory ConversationListDto.fromJson(Map<String, dynamic> json) =>
      _$ConversationListDtoFromJson(json);

  Map<String, dynamic> toJson() => _$ConversationListDtoToJson(this);

  final int id;
  final ConversationType type;
  final List<UserDto> members;
  final List<LastMessageDto> lastMessage;
}
