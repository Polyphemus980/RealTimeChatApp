import 'package:chatapp_frontend/core/auth/auth_token_service.dart';
import 'package:chatapp_frontend/core/result/result.dart';
import 'package:dio/dio.dart';

class ApiService {
  ApiService({required AuthTokenService tokenService}) {
    _dio = Dio(
      BaseOptions(
        baseUrl: 'http://10.0.2.2:5244/',
        connectTimeout: const Duration(seconds: 10),
        receiveTimeout: const Duration(seconds: 10),
      ),
    )..interceptors.add(_authInterceptor);
    tokenService.tokenStream.listen(updateToken);
  }

  late final Dio _dio;
  String? _token;

  InterceptorsWrapper get _authInterceptor => InterceptorsWrapper(
        onRequest: (options, handler) {
          if (_token != null) {
            options.headers['Authorization'] = 'Bearer $_token';
          }
          if (options.data != null) {
            options.headers['Content-Type'] = 'application/json';
          }
          return handler.next(options);
        },
      );

  void updateToken(String? token) {
    _token = token;
    _dio.interceptors.clear();
    _dio.interceptors.add(_authInterceptor);
  }

  Future<Result<T>> get<T>(String endpoint) async {
    try {
      final response = await _dio.get<T>(endpoint);

      final statusCode = response.statusCode;
      if (statusCode != null && statusCode >= 200 && statusCode <= 299) {
        if (response.data is T) {
          return Result<T>.success(response.data as T);
        } else {
          return Result<T>.failure(
            'Unexpected response type: Expected $T, got ${response.data.runtimeType}',
          );
        }
      } else {
        return Result<T>.failure(response.statusMessage ?? 'Unknown error');
      }
    } catch (err) {
      return Result<T>.failure('Request failed: $err');
    }
  }

  Future<Result<T>> post<T>(String endpoint, {dynamic data}) async {
    try {
      final response = await _dio.post<T>(endpoint, data: data);

      final statusCode = response.statusCode;
      if (statusCode != null && statusCode >= 200 && statusCode <= 299) {
        if (response.data is T) {
          return Result<T>.success(response.data as T);
        } else {
          return Result<T>.failure(
            'Unexpected response type: Expected $T, got ${response.data.runtimeType}',
          );
        }
      } else {
        return Result<T>.failure(response.statusMessage ?? 'Unknown error');
      }
    } catch (err) {
      return Result<T>.failure('Request failed: $err');
    }
  }
}
