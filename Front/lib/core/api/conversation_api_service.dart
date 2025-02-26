import 'package:chatapp_frontend/core/api/api_service.dart';
import 'package:chatapp_frontend/core/result/result.dart';
import 'package:chatapp_frontend/data/dto/conversations/conversation_list_dto.dart';
import 'package:chatapp_frontend/data/dto/conversations/single_conversation_dto.dart';
import 'package:chatapp_frontend/domain/entities/conversations/conversation_list.dart';
import 'package:chatapp_frontend/domain/entities/conversations/single_conversation.dart';

class ConversationApiService {
  ConversationApiService({required ApiService apiService})
      : _apiService = apiService;
  final ApiService _apiService;

  Future<Result<List<ConversationList>>> getConversations() async {
    final result =
        await _apiService.get<List<Map<String, dynamic>>>('conversations');
    if (!result.isSuccess) {
      return Result<List<ConversationList>>.failure(result.errorMessage!);
    }
    try {
      final List<ConversationListDto> conversationList =
          (result.data!).map(ConversationListDto.fromJson).toList();

      final parsedConversationList = List<ConversationList>.from(
        conversationList.map(ConversationList.fromDto),
      );

      return Result<List<ConversationList>>.success(parsedConversationList);
    } catch (err) {
      return Result<List<ConversationList>>.failure(err.toString());
    }
  }

  Future<Result<SingleConversation>> getConversation(
    int conversationId,
  ) async {
    final result = await _apiService
        .get<Map<String, dynamic>>('conversation/$conversationId');
    if (!result.isSuccess) {
      return Result<SingleConversation>.failure(result.errorMessage!);
    }
    try {
      final parsedConversation = SingleConversationDto.fromJson(result.data!);
      return Result<SingleConversation>.success(
        SingleConversation.fromDto(parsedConversation),
      );
    } catch (err) {
      return Result<SingleConversation>.failure(err.toString());
    }
  }
}
