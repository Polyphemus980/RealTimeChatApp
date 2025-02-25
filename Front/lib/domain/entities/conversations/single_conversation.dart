import 'package:chatapp_frontend/core/enums/conversation_type.dart';
import 'package:chatapp_frontend/data/dto/conversations/single_conversation_dto.dart';

import '../messages/conversation_message.dart';
import '../users/chat_user.dart';

class SingleConversation {
  SingleConversation({
    required this.id,
    required this.type,
    required this.members,
    required this.messages,
  });

  factory SingleConversation.fromDto(SingleConversationDto dto) =>
      SingleConversation(
        id: dto.id,
        type: dto.type,
        members: dto.members.map(ChatUser.fromDto).toList(),
        messages: dto.messages.map(ConversationMessage.fromDto).toList(),
      );
  final int id;
  final ConversationType type;
  final List<ChatUser> members;
  final List<ConversationMessage> messages;
}
