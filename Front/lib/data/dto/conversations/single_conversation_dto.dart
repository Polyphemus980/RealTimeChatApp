import 'package:chatapp_frontend/core/enums/conversation_type.dart';
import 'package:chatapp_frontend/data/dto/users/user_dto.dart';
import 'package:json_annotation/json_annotation.dart';

import '../messages/conversation_message_dto.dart';

part 'single_conversation_dto.g.dart';

@JsonSerializable()
class SingleConversationDto {
  SingleConversationDto({
    required this.id,
    required this.type,
    required this.members,
    required this.messages,
  });

  factory SingleConversationDto.fromJson(Map<String, dynamic> json) =>
      _$SingleConversationDtoFromJson(json);

  Map<String, dynamic> toJson() => _$SingleConversationDtoToJson(this);

  final int id;
  final ConversationType type;
  final List<UserDto> members;
  final List<ConversationMessageDto> messages;
}
