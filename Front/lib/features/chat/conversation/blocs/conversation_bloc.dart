import 'dart:async';

import 'package:chatapp_frontend/core/api/conversation_api_service.dart';
import 'package:chatapp_frontend/core/common/events/chat_event.dart';
import 'package:chatapp_frontend/core/hubs/hub_service.dart';
import 'package:chatapp_frontend/domain/entities/conversations/single_conversation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

sealed class ConversationEvent {}

class InitializeConversation extends ConversationEvent {}

class ConversationReceiveMessage extends ConversationEvent {
  ConversationReceiveMessage({
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

class ConversationReceiverDelivered extends ConversationEvent {
  ConversationReceiverDelivered({
    required this.receiverId,
    required this.messageId,
  });
  final String receiverId;
  final int messageId;
}

class ConversationReceiverRead extends ConversationEvent {
  ConversationReceiverRead({required this.receiverId});
  final String receiverId;
}

class ConversationStartedTyping extends ConversationEvent {
  ConversationStartedTyping({required this.typerId});
  final String typerId;
}

class ConversationStoppedTyping extends ConversationEvent {
  ConversationStoppedTyping({required this.typerId});
  final String typerId;
}

class ConversationJoinedGroupChat extends ConversationEvent {
  ConversationJoinedGroupChat({required this.userId, required this.groupId});
  final String userId;
  final int groupId;
}

class ConversationLeftGroupChat extends ConversationEvent {
  ConversationLeftGroupChat({required this.userId, required this.groupId});
  final String userId;
  final int groupId;
}

class ConversationChangedNickname extends ConversationEvent {
  ConversationChangedNickname({
    required this.userId,
    required this.groupId,
    required this.newNickname,
  });
  final int groupId;
  final String userId;
  final String newNickname;
}

sealed class ConversationState {}

class Loading extends ConversationState {}

class Loaded extends ConversationState {
  Loaded({required this.conversation});

  final SingleConversation conversation;
}

class Error extends ConversationState {
  Error({required this.errorMessage});

  final String errorMessage;
}

class ConversationBloc extends Bloc<ConversationEvent, ConversationState> {
  ConversationBloc({
    required HubService hubService,
    required ConversationApiService conversationApiService,
    required this.conversationId,
  })  : _hubService = hubService,
        _conversationApiService = conversationApiService,
        super(Loading()) {
    _handleEventStream();
    on<InitializeConversation>(_initializeConversations);
  }

  final int conversationId;
  final HubService _hubService;
  final ConversationApiService _conversationApiService;
  StreamSubscription<ChatEvent>? _chatEventSubscription;

  Future<void> _initializeConversations(
    InitializeConversation event,
    Emitter<ConversationState> emit,
  ) async {
    final result =
        await _conversationApiService.getConversation(conversationId);
    result.isSuccess
        ? emit(
            Loaded(conversation: result.data!),
          )
        : emit(
            Error(errorMessage: result.errorMessage!),
          );
  }

  void _handleEventStream() {
    _chatEventSubscription = _hubService.eventStream.listen(
      (event) {
        if (event is ReceiveMessage) {
          add(
            ConversationReceiveMessage(
              senderId: event.senderId,
              message: event.message,
              conversationId: event.conversationId,
              sentAt: event.sentAt,
            ),
          );
        } else if (event is ReceiverDelivered) {
          add(
            ConversationReceiverDelivered(
              receiverId: event.receiverId,
              messageId: event.messageId,
            ),
          );
        } else if (event is ReceiverRead) {
          add(ConversationReceiverRead(receiverId: event.receiverId));
        } else if (event is StartedTyping) {
          add(ConversationStartedTyping(typerId: event.typerId));
        } else if (event is StoppedTyping) {
          add(ConversationStoppedTyping(typerId: event.typerId));
        } else if (event is JoinedGroupChat) {
          add(
            ConversationJoinedGroupChat(
              userId: event.userId,
              groupId: event.groupId,
            ),
          );
        } else if (event is LeftGroupChat) {
          add(
            ConversationLeftGroupChat(
              userId: event.userId,
              groupId: event.groupId,
            ),
          );
        } else if (event is ChangedNickname) {
          add(
            ConversationChangedNickname(
              userId: event.userId,
              groupId: event.groupId,
              newNickname: event.newNickname,
            ),
          );
        }
      },
    );
  }

  @override
  Future<void> close() async {
    await _chatEventSubscription!.cancel();
    await super.close();
  }
}
