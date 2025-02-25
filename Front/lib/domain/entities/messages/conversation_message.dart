import 'package:chatapp_frontend/data/dto/messages/conversation_message_dto.dart';

import 'message_receiver.dart';

class ConversationMessage {
  ConversationMessage({
    required this.id,
    required this.receivers,
    required this.content,
    required this.senderId,
    required this.sentAt,
  });

  factory ConversationMessage.fromDto(ConversationMessageDto dto) =>
      ConversationMessage(
        id: dto.id,
        receivers: dto.receivers.map(MessageReceiver.fromDto).toList(),
        content: dto.content,
        senderId: dto.senderId,
        sentAt: dto.sentAt,
      );
  final int id;
  final List<MessageReceiver> receivers;
  final String content;
  final String senderId;
  final DateTime sentAt;
}
