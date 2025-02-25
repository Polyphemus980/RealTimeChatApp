import 'package:chatapp_frontend/core/api/api_service.dart';
import 'package:chatapp_frontend/core/api/conversation_api_service.dart';
import 'package:chatapp_frontend/core/api/user_api_service.dart';
import 'package:chatapp_frontend/core/auth/auth_token_service.dart';
import 'package:get_it/get_it.dart';

bool setUpEnded = false;

final getIt = GetIt.instance;

void getItSetUp() {
  if (!setUpEnded) {
    getIt
      ..registerSingleton(AuthTokenService())
      ..registerSingleton(
        ApiService(
          tokenService: getIt.get<AuthTokenService>(),
        ),
      )
      ..registerSingleton(
        UserApiService(
          apiService: getIt.get<ApiService>(),
        ),
      )
      ..registerSingleton(
        ConversationApiService(
          apiService: getIt.get<ApiService>(),
        ),
      );
  }
}
