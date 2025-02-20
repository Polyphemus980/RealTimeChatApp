import 'dart:async';

import 'package:signalr_core/signalr_core.dart';

import 'auth_token_service.dart';

class HubService {
  HubService({required AuthTokenService tokenService}) {
    tokenService.tokenStream.listen(updateToken);
  }

  static const _hubUrl = 'http://10.0.2.2:5244/chat';
  String? _token;
  HubConnection? _connection;
  final StreamController<String> _eventStreamController =
      StreamController.broadcast();

  Stream<String> get eventStream => _eventStreamController.stream;

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
    // TODO Add listeners here
  }

  Future<void> _disconnect() async {
    if (_connection != null) {
      await _connection!.stop();
      _connection = null;
    }
  }
}
