import 'package:chatapp_frontend/core/api/conversation_api_service.dart';
import 'package:chatapp_frontend/core/common/widgets/app_scaffold.dart';
import 'package:chatapp_frontend/core/dependency_injection/get_it_di.dart';
import 'package:chatapp_frontend/core/hubs/hub_service.dart';
import 'package:chatapp_frontend/domain/entities/messages/conversation_message.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../blocs/conversation_bloc.dart';

class ConversationScreen extends StatelessWidget {
  const ConversationScreen({super.key, required this.conversationId});
  final int conversationId;

  @override
  Widget build(BuildContext context) {
    return BlocProvider<ConversationBloc>(
      create: (context) => ConversationBloc(
        conversationId: conversationId,
        conversationApiService: getIt.get<ConversationApiService>(),
        hubService: getIt.get<HubService>(),
      ),
      child: const ConversationPage(),
    );
  }
}

class ConversationPage extends StatelessWidget {
  const ConversationPage({super.key});

  @override
  Widget build(BuildContext context) {
    return AppScaffold(
      title: 'Conversation',
      child: BlocBuilder<ConversationBloc, ConversationState>(
        builder: (context, state) {
          if (state is Loaded) {
            return ListView.builder(
              itemCount: state.conversation.messages.length,
              itemBuilder: (context, index) {
                return MessageTile(message: state.conversation.messages[index]);
              },
            );
          } else {
            return const SizedBox.shrink();
          }
        },
      ),
    );
  }
}

class MessageTile extends StatelessWidget {
  const MessageTile({super.key, required this.message});
  final ConversationMessage message;

  @override
  Widget build(BuildContext context) {
    return const Text('');
  }
}
