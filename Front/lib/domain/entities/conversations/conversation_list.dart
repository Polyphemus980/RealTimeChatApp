import 'package:chatapp_frontend/core/enums/conversation_type.dart';
import 'package:chatapp_frontend/data/dto/conversations/conversation_list_dto.dart';

import '../messages/last_message.dart';
import '../users/chat_user.dart';

class ConversationList {
  ConversationList({
    required this.id,
    required this.type,
    required this.members,
    required this.lastMessages,
  });

  factory ConversationList.fromDto(ConversationListDto dto) => ConversationList(
        id: dto.id,
        type: dto.type,
        members: dto.members.map(ChatUser.fromDto).toList(),
        lastMessages: dto.lastMessages.map(LastMessage.fromDto).toList(),
      );

  final int id;
  final ConversationType type;
  final List<ChatUser> members;
  final List<LastMessage> lastMessages;
}
