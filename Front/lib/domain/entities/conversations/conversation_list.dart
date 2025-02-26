import 'package:chatapp_frontend/core/enums/conversation_type.dart';
import 'package:chatapp_frontend/data/dto/conversations/conversation_list_dto.dart';

import '../messages/last_message.dart';
import '../users/chat_user.dart';

class ConversationList {
  ConversationList({
    required this.id,
    required this.type,
    required this.members,
    required this.currentUser,
    required this.lastMessage,
  });

  factory ConversationList.fromDto(ConversationListDto dto) => ConversationList(
        id: dto.id,
        type: dto.type,
        members: dto.members.map(ChatUser.fromDto).toList(),
        currentUser: ChatUser.fromDto(dto.currentUser),
        lastMessage: dto.lastMessage.map(LastMessage.fromDto).toList(),
      );

  final int id;
  final ConversationType type;
  final List<ChatUser> members;
  final ChatUser currentUser;
  final List<LastMessage> lastMessage;

  ConversationList copyWith({
    int? id,
    ConversationType? type,
    List<ChatUser>? members,
    ChatUser? currentUser,
    List<LastMessage>? lastMessage,
  }) {
    return ConversationList(
      id: id ?? this.id,
      type: type ?? this.type,
      members: members ?? this.members,
      currentUser: currentUser ?? this.currentUser,
      lastMessage: lastMessage ?? this.lastMessage,
    );
  }
}
