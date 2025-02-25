import 'dart:async';

import 'package:chatapp_frontend/core/api/conversation_api_service.dart';
import 'package:chatapp_frontend/core/common/events/chat_event.dart';
import 'package:chatapp_frontend/core/hubs/hub_service.dart';
import 'package:chatapp_frontend/data/dto/conversations/conversation_list_dto.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

sealed class ConversationListEvent {}

class InitializeConversations extends ConversationListEvent {}

class ReceivedMessage extends ConversationListEvent {
  ReceivedMessage({
    required this.senderId,
    required this.message,
    required this.conversationId,
  });
  final String senderId;
  final int conversationId;
  final String message;
}

class JoinedGroup extends ConversationListEvent {
  JoinedGroup({required this.joiningUserId, required this.groupId});

  final String joiningUserId;
  final int groupId;
}

class LeftGroup extends ConversationListEvent {
  LeftGroup({required this.leavingUserId, required this.groupId});

  final String leavingUserId;
  final int groupId;
}

sealed class ConversationListState {}

class LoadingData extends ConversationListState {}

class Loaded extends ConversationListState {
  Loaded({required this.conversationList});

  final ConversationListDto conversationList;
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
  }
  final HubService _hubService;
  final ConversationApiService _conversationApiService;
  StreamSubscription<ChatEvent>? _chatEventSubscription;

  void _handleEventStream() {
    _chatEventSubscription = _hubService.eventStream.listen(
      (event) {
        if (event is ReceiveMessage) {
          add(
            ReceivedMessage(
              senderId: event.senderId,
              message: event.message,
              conversationId: event.conversationId,
            ),
          );
        } else if (event is JoinedGroupChat) {
          add(
            JoinedGroup(
              joiningUserId: event.userId,
              groupId: event.groupId,
            ),
          );
        } else if (event is LeftGroupChat) {
          add(
            LeftGroup(
              leavingUserId: event.userId,
              groupId: event.groupId,
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
