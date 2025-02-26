import 'package:chatapp_frontend/core/enums/conversation_type.dart';
import 'package:chatapp_frontend/data/dto/conversations/single_conversation_dto.dart';

import '../messages/conversation_message.dart';
import '../users/chat_user.dart';

class SingleConversation {
  SingleConversation({
    required this.id,
    required this.type,
    required this.members,
    required this.currentUser,
    required this.messages,
  });

  factory SingleConversation.fromDto(SingleConversationDto dto) =>
      SingleConversation(
        id: dto.id,
        type: dto.type,
        members: dto.members.map(ChatUser.fromDto).toList(),
        currentUser: ChatUser.fromDto(dto.currentUser),
        messages: dto.messages.map(ConversationMessage.fromDto).toList(),
      );

  final int id;
  final ConversationType type;
  final List<ChatUser> members;
  final ChatUser currentUser;
  final List<ConversationMessage> messages;

  SingleConversation copyWith({
    int? id,
    ConversationType? type,
    List<ChatUser>? members,
    ChatUser? currentUser,
    List<ConversationMessage>? messages,
  }) {
    return SingleConversation(
      id: id ?? this.id,
      type: type ?? this.type,
      members: members ?? this.members,
      currentUser: currentUser ?? this.currentUser,
      messages: messages ?? this.messages,
    );
  }
}
