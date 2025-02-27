abstract class ChatEvent {}

class ReceiveMessage extends ChatEvent {
  ReceiveMessage({
    required this.senderId,
    required this.message,
    required this.conversationId,
    required this.sentAt,
  });
  final String senderId;
  final int conversationId;
  final String message;
  final DateTime sentAt;
}

class ReceiverDelivered extends ChatEvent {
  ReceiverDelivered({required this.receiverId, required this.messageId});
  final String receiverId;
  final int messageId;
}

class ReceiverRead extends ChatEvent {
  ReceiverRead({required this.receiverId});
  final String receiverId;
}

class StartedTyping extends ChatEvent {
  StartedTyping({required this.typerId});
  final String typerId;
}

class StoppedTyping extends ChatEvent {
  StoppedTyping({required this.typerId});
  final String typerId;
}

class JoinedGroupChat extends ChatEvent {
  JoinedGroupChat({required this.userId, required this.groupId});
  final String userId;
  final int groupId;
}

class LeftGroupChat extends ChatEvent {
  LeftGroupChat({required this.userId, required this.groupId});
  final String userId;
  final int groupId;
}

class ChangedNickname extends ChatEvent {
  ChangedNickname({
    required this.userId,
    required this.groupId,
    required this.newNickname,
  });
  final int groupId;
  final String userId;
  final String newNickname;
}
