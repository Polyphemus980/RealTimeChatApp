import 'package:chatapp_frontend/core/enums/message_status.dart';
import 'package:json_annotation/json_annotation.dart';

part 'message_receiver_dto.g.dart';

@JsonSerializable()
class MessageReceiverDto {
  MessageReceiverDto({required this.userId, required this.status});

  factory MessageReceiverDto.fromJson(Map<String, dynamic> json) =>
      _$MessageReceiverDtoFromJson(json);

  Map<String, dynamic> toJson() => _$MessageReceiverDtoToJson(this);

  final String userId;
  final MessageStatus status;
}
