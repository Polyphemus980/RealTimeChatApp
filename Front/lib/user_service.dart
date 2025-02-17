import 'package:chatapp_frontend/api_service.dart';
import 'package:chatapp_frontend/core/result_type/result.dart';

class UserApiService {
  UserApiService({required this.apiService});
  final ApiService apiService;

  Future<Result<bool>> checkIfNameFree(String name) async {
    final result =
        await apiService.get<Map<String, dynamic>>('user/check-name');
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

  Future<Result<bool>> verifyTokenAndCheckIfNewUser() async {
    final result = await apiService.get<Map<String, dynamic>>('auth/verify');
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
}
