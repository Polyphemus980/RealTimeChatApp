import 'package:chatapp_frontend/core/api/api_service.dart';
import 'package:chatapp_frontend/core/result/result.dart';
import 'package:chatapp_frontend/data/dto/conversations/conversation_list_dto.dart';
import 'package:chatapp_frontend/data/dto/conversations/single_conversation_dto.dart';

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
      return Result<ConversationListDto>.success(parsedConversationList);
    } catch (err) {
      return Result<ConversationListDto>.failure(err.toString());
    }
  }

  Future<Result<SingleConversationDto>> getConversation(
    int conversationId,
  ) async {
    final result = await _apiService
        .get<Map<String, dynamic>>('conversation/$conversationId');
    if (!result.isSuccess) {
      return Result<SingleConversationDto>.failure(result.errorMessage!);
    }
    try {
      final parsedConversation = SingleConversationDto.fromJson(result.data!);
      return Result<SingleConversationDto>.success(parsedConversation);
    } catch (err) {
      return Result<SingleConversationDto>.failure(err.toString());
    }
  }
}
