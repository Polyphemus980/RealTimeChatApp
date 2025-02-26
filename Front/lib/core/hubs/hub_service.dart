import 'dart:async';

import 'package:chatapp_frontend/core/common/events/chat_event.dart';
import 'package:chatapp_frontend/core/result/result.dart';
import 'package:chatapp_frontend/core/result/unit.dart';
import 'package:signalr_core/signalr_core.dart';

import '../auth/auth_token_service.dart';

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
        if (arguments != null && arguments.length == 3) {
          final senderId = arguments[0] as String;
          final message = arguments[1] as String;
          final conversationId = arguments[2] as int;
          _eventStreamController.add(
            ReceiveMessage(
              senderId: senderId,
              message: message,
              conversationId: conversationId,
            ),
          );
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

  Future<Result<Unit>> sendMessage(int conversationId, String content) async {
    try {
      await _connection?.invoke('sendMessage', args: [conversationId, content]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> markAsDelivered(String senderId, int messageId) async {
    try {
      await _connection?.invoke('MarkAsDelivered', args: [senderId, messageId]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> markAsRead(String senderId) async {
    try {
      await _connection?.invoke('MarkAsRead', args: [senderId]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> startedTyping(int conversationId) async {
    try {
      await _connection?.invoke('StartedTyping', args: [conversationId]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> stoppedTyping(int conversationId) async {
    try {
      await _connection?.invoke('StoppedTyping', args: [conversationId]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> joinGroupChat(int groupId) async {
    try {
      await _connection?.invoke('JoinGroupChat', args: [groupId]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> leaveGroupChat(int groupId) async {
    try {
      await _connection?.invoke('LeaveGroupChat', args: [groupId]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<Result<Unit>> changeNickname(
    int conversationId,
    String newNickname,
  ) async {
    try {
      await _connection
          ?.invoke('ChangeNickname', args: [conversationId, newNickname]);
      return Result<Unit>.success(Unit());
    } catch (err) {
      return Result<Unit>.failure(err.toString());
    }
  }

  Future<void> _disconnect() async {
    if (_connection != null) {
      await _connection!.stop();
      _connection = null;
    }
  }
}
