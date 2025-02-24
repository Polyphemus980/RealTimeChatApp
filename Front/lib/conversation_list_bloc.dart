import 'package:flutter_bloc/flutter_bloc.dart';

sealed class ConversationListEvent {}

class InitializeMessages extends ConversationListEvent {}

class ReceivedNewMessage extends ConversationListEvent {
  ReceivedNewMessage({required this.message});
  final String message;
}

class MessageDelivered extends ConversationListEvent {
  MessageDelivered({required this.messageId});

  final int messageId;
}

class MessagesRead extends ConversationListEvent {}

class StartedTyping extends ConversationListEvent {}

class StoppedTyping extends ConversationListEvent {}

class JoinedGroupChat extends ConversationListEvent {
  JoinedGroupChat({required this.joiningUserId});

  final String joiningUserId;
}

class LeftGroupChat extends ConversationListEvent {
  LeftGroupChat({required this.leavingUserId});

  final String leavingUserId;
}

class ChangedNickname extends ConversationListEvent {
  ChangedNickname({required this.newNickname, required this.changingUserId});

  final String newNickname;
  final String changingUserId;
}

sealed class ConversationListState {}

class LoadingData extends ConversationListState {}

class Loaded extends ConversationListState {}

class ConversationListBloc
    extends Bloc<ConversationListEvent, ConversationListState> {
  ConversationListBloc() : super(LoadingData());
}
