import 'dart:async';

import 'package:chatapp_frontend/chat_event.dart';
import 'package:signalr_core/signalr_core.dart';

import 'auth_token_service.dart';

class HubService {
  HubService({required AuthTokenService tokenService}) {
    tokenService.tokenStream.listen(updateToken);
  }

  static const _hubUrl = 'http://10.0.2.2:5244/chat';
  String? _token;
  HubConnection? _connection;
  final StreamController<ChatEvent> _eventStreamController =
      StreamController.broadcast();

  Stream<ChatEvent> get eventStream => _eventStreamController.stream;

  Future<void> updateToken(String? token) async {
    if (token == _token) {
      return;
    }

    _token = token;

    if (token == null) {
      await _disconnect();
      return;
    }

    if (_connection?.state == HubConnectionState.connected) {
      await _connection!.stop();
    }

    await _connect();
  }

  Future<void> _connect() async {
    _connection = HubConnectionBuilder()
        .withUrl(
          _hubUrl,
          HttpConnectionOptions(
            accessTokenFactory: () async => _token,
          ),
        )
        .withAutomaticReconnect()
        .build();

    _setupListeners();
    await _connection!.start();
  }

  void _setupListeners() {
    if (_connection == null) {
      return;
    }
    _connection!
      ..on('ReceiveMessage', (arguments) {
        if (arguments != null && arguments.length == 2) {
          final senderId = arguments[0] as String;
          final message = arguments[1] as String;
          _eventStreamController
              .add(ReceiveMessage(senderId: senderId, message: message));
        }
      })
      ..on('ReceiverDelivered', (arguments) {
        if (arguments != null && arguments.length == 2) {
          final receiverId = arguments[0] as String;
          final messageId = arguments[1] as int;
          _eventStreamController.add(
            ReceiverDelivered(receiverId: receiverId, messageId: messageId),
          );
        }
      })
      ..on('ReceiverRead', (arguments) {
        if (arguments != null && arguments.isNotEmpty) {
          final receiverId = arguments[0] as String;
          _eventStreamController.add(ReceiverRead(receiverId: receiverId));
        }
      })
      ..on('StartedTyping', (arguments) {
        if (arguments != null && arguments.isNotEmpty) {
          final typerId = arguments[0] as String;
          _eventStreamController.add(StartedTyping(typerId: typerId));
        }
      })
      ..on('StoppedTyping', (arguments) {
        if (arguments != null && arguments.isNotEmpty) {
          final typerId = arguments[0] as String;
          _eventStreamController.add(StoppedTyping(typerId: typerId));
        }
      })
      ..on('JoinedGroupChat', (arguments) {
        if (arguments != null && arguments.length == 2) {
          final userId = arguments[0] as String;
          final groupId = arguments[1] as int;
          _eventStreamController.add(
            JoinedGroupChat(userId: userId, groupId: groupId),
          );
        }
      })
      ..on('LeftGroupChat', (arguments) {
        if (arguments != null && arguments.length == 2) {
          final userId = arguments[0] as String;
          final groupId = arguments[1] as int;
          _eventStreamController.add(
            LeftGroupChat(userId: userId, groupId: groupId),
          );
        }
      })
      ..on('ChangedNickname', (arguments) {
        if (arguments != null && arguments.length == 3) {
          final userId = arguments[0] as String;
          final newNickname = arguments[1] as String;
          final groupId = arguments[2] as int;
          _eventStreamController.add(
            ChangedNickname(
              userId: userId,
              newNickname: newNickname,
              groupId: groupId,
            ),
          );
        }
      });
  }

  Future<void> _disconnect() async {
    if (_connection != null) {
      await _connection!.stop();
      _connection = null;
    }
  }
}
