import 'package:chatapp_frontend/core/enums/message_status.dart';
import 'package:chatapp_frontend/data/dto/messages/message_receiver_dto.dart';

class MessageReceiver {
  MessageReceiver({required this.userId, required this.status});

  factory MessageReceiver.fromDto(MessageReceiverDto dto) =>
      MessageReceiver(userId: dto.userId, status: dto.status);

  final String userId;
  final MessageStatus status;
}
