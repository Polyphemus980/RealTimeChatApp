import 'dart:async';

class AuthTokenService {
  String? _token;
  final StreamController<String?> _tokenStreamController =
      StreamController.broadcast();

  String? get token => _token;
  Stream<String?> get tokenStream => _tokenStreamController.stream;

  void updateToken(String? token) {
    if (token == _token) {
      return;
    }
    _token = token;
    _tokenStreamController.add(_token);
  }
}
