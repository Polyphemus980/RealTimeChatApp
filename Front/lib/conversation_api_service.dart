import 'package:chatapp_frontend/api_service.dart';
import 'package:chatapp_frontend/core/result_type/result.dart';
import 'package:chatapp_frontend/dto/conversations/conversation_list_dto.dart';

class ConversationApiService {
  ConversationApiService({required ApiService apiService})
      : _apiService = apiService;
  final ApiService _apiService;

  Future<Result<ConversationListDto>> getConversations() async {
    final result = await _apiService.get<Map<String, dynamic>>('conversations');
    if (!result.isSuccess) {
      return Result<ConversationListDto>.failure(result.errorMessage!);
    }
    try {
      final parsedConversationList = ConversationListDto.fromJson(result.data!);
      return Result.success(parsedConversationList);
    } catch (err) {
      return Result<ConversationListDto>.failure(err.toString());
    }
  }
}
