import 'package:chatapp_frontend/data/dto/messages/last_message_dto.dart';

class LastMessage {
  LastMessage({
    required this.id,
    required this.content,
    required this.sentAt,
    required this.senderName,
  });

  factory LastMessage.fromDto(LastMessageDto dto) => LastMessage(
        id: dto.id,
        content: dto.content,
        sentAt: dto.sentAt,
        senderName: dto.senderName,
      );

  final int id;
  final String content;
  final DateTime sentAt;
  final String senderName;

  LastMessage copyWith({
    int? id,
    String? content,
    DateTime? sentAt,
    String? senderName,
  }) {
    return LastMessage(
      id: id ?? this.id,
      content: content ?? this.content,
      sentAt: sentAt ?? this.sentAt,
      senderName: senderName ?? this.senderName,
    );
  }
}
