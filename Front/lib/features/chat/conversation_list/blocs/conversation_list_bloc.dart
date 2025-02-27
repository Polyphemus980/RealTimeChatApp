import 'dart:async';

import 'package:chatapp_frontend/core/api/conversation_api_service.dart';
import 'package:chatapp_frontend/core/common/events/chat_event.dart';
import 'package:chatapp_frontend/core/hubs/hub_service.dart';
import 'package:chatapp_frontend/domain/entities/conversations/conversation_list.dart';
import 'package:chatapp_frontend/domain/entities/messages/last_message.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

sealed class ConversationListEvent {}

class InitializeConversations extends ConversationListEvent {}

class ReceivedMessage extends ConversationListEvent {
  ReceivedMessage({
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

sealed class ConversationListState {}

class LoadingData extends ConversationListState {}

class Loaded extends ConversationListState {
  Loaded({required this.conversationList});

  final List<ConversationList> conversationList;
}

class Error extends ConversationListState {
  Error({required this.errorMessage});

  final String errorMessage;
}

class ConversationListBloc
    extends Bloc<ConversationListEvent, ConversationListState> {
  ConversationListBloc({
    required HubService hubService,
    required ConversationApiService conversationApiService,
  })  : _hubService = hubService,
        _conversationApiService = conversationApiService,
        super(LoadingData()) {
    _handleEventStream();
    on<InitializeConversations>(_initializeConversations);
    on<ReceivedMessage>(_receiveMessage);
  }
  final HubService _hubService;
  final ConversationApiService _conversationApiService;
  StreamSubscription<ChatEvent>? _chatEventSubscription;

  Future<void> _receiveMessage(
    ReceivedMessage event,
    Emitter<ConversationListState> emit,
  ) async {
    if (state is! Loaded) {
      return;
    }
    final loadedState = state as Loaded;

    final modifiedConversation = loadedState.conversationList
        .firstWhere((c) => c.id == event.conversationId);

    modifiedConversation.copyWith(
      lastMessage: LastMessage(
        content: event.message,
        sentAt: event.sentAt,
        senderName: modifiedConversation.members
            .where((m) => m.id == event.senderId)
            .first
            .displayName,
      ),
    );

    final newConversations = [
      modifiedConversation,
      ...loadedState.conversationList
          .where((c) => c.id != event.conversationId),
    ];
    emit(Loaded(conversationList: newConversations));
  }

  void _handleEventStream() {
    _chatEventSubscription = _hubService.eventStream.listen(
      (event) {
        if (event is ReceiveMessage) {
          add(
            ReceivedMessage(
              senderId: event.senderId,
              message: event.message,
              conversationId: event.conversationId,
              sentAt: event.sentAt,
            ),
          );
        }
      },
    );
  }

  Future<void> _initializeConversations(
    InitializeConversations event,
    Emitter<ConversationListState> emit,
  ) async {
    final result = await _conversationApiService.getConversations();
    result.isSuccess
        ? emit(
            Loaded(conversationList: result.data!),
          )
        : emit(
            Error(errorMessage: result.errorMessage!),
          );
  }

  @override
  Future<void> close() async {
    await _chatEventSubscription!.cancel();
    await super.close();
  }
}
