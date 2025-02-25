import 'package:chatapp_frontend/core/api/conversation_api_service.dart';
import 'package:chatapp_frontend/core/common/widgets/app_scaffold.dart';
import 'package:chatapp_frontend/core/dependency_injection/get_it_di.dart';
import 'package:chatapp_frontend/core/hubs/hub_service.dart';
import 'package:chatapp_frontend/features/chat/conversation_list/blocs/conversation_list_bloc.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class ConversationListScreen extends StatelessWidget {
  const ConversationListScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider<ConversationListBloc>(
      create: (context) => ConversationListBloc(
        hubService: getIt.get<HubService>(),
        conversationApiService: getIt.get<ConversationApiService>(),
      ),
      child: const ConversationList(),
    );
  }
}

class ConversationList extends StatelessWidget {
  const ConversationList({super.key});

  @override
  Widget build(BuildContext context) {
    return AppScaffold(
      title: 'Conversations',
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 32),
        child: BlocBuilder<ConversationListBloc, ConversationListState>(
          builder: (context, state) {
            return const Scaffold();
          },
        ),
      ),
    );
  }
}
