import 'package:chatapp_frontend/api_service.dart';
import 'package:chatapp_frontend/user_api_service.dart';
import 'package:get_it/get_it.dart';

bool setUpEnded = false;

final getIt = GetIt.instance;

void getItSetUp() {
  if (!setUpEnded) {
    getIt
      ..registerSingleton(ApiService())
      ..registerSingleton(UserApiService(apiService: getIt.get<ApiService>()));
  }
}
