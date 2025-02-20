import 'package:flutter_bloc/flutter_bloc.dart';

sealed class ConversationListEvent {}

class InitializeList extends ConversationListEvent {}

sealed class ConversationListState {}

class LoadingData extends ConversationListState {}

class ConversationListBloc
    extends Bloc<ConversationListEvent, ConversationListState> {
  ConversationListBloc() : super(LoadingData());
}
