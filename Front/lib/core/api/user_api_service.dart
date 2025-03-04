import 'dart:convert';

import 'package:chatapp_frontend/core/api/api_service.dart';
import 'package:chatapp_frontend/core/result/result.dart';
import 'package:chatapp_frontend/core/result/unit.dart';

class UserApiService {
  UserApiService({required ApiService apiService}) : _apiService = apiService;
  final ApiService _apiService;

  Future<Result<bool>> checkIfNameFree(String name) async {
    final requestUri = Uri.parse('user/check-name').replace(
      queryParameters: {
        'name': name,
      },
    );
    final result =
        await _apiService.get<Map<String, dynamic>>(requestUri.toString());
    if (!result.isSuccess) {
      return Result<bool>.failure(result.errorMessage!);
    }
    try {
      final isFree = result.data?['isFree'];
      if (isFree == null || isFree is! bool) {
        return Result<bool>.failure('Field "isFree" is missing or invalid.');
      }

      return Result<bool>.success(isFree);
    } catch (err) {
      return Result<bool>.failure('Error processing response: $err');
    }
  }

  Future<Result<bool>> checkIfNewUser() async {
    final result =
        await _apiService.get<Map<String, dynamic>>('user/check-new');
    if (!result.isSuccess) {
      return Result<bool>.failure(result.errorMessage!);
    }
    try {
      final isNew = result.data?['isNew'];
      if (isNew == null || isNew is! bool) {
        return Result<bool>.failure('Field "isNew" is missing or invalid.');
      }

      return Result<bool>.success(isNew);
    } catch (err) {
      return Result<bool>.failure('Error processing response: $err');
    }
  }

  Future<Result<Unit>> createUser(String displayName) async {
    final result = await _apiService.post<Map<String, dynamic>>(
      'auth/register',
      data: jsonEncode(
        {'displayName': displayName},
      ),
    );
    if (!result.isSuccess) {
      return Result<Unit>.failure(result.errorMessage!);
    } else {
      return Result<Unit>.success(Unit());
    }
  }

  void updateToken(String? token) {
    _apiService.updateToken(token);
  }
}
