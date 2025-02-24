using AutoMapper;
using ChatApp.Backend.Core.Messages.DTOs;
using ChatApp.Backend.Core.Users.DTOs;
using ChatApp.Backend.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Core.Conversations.DTOs;

public class ConversationProfile : Profile
{
    public ConversationProfile()
    {
        CreateMap<Conversation, ConversationListDto>()
            .ForMember(
                dest => dest.Members,
                opt =>
                    opt.MapFrom(src =>
                        src.Users.Select(u => new UserDto(u.Id, u.DisplayName, null))
                    )
            )
            .ForMember(
                dest => dest.LastMessage,
                opt =>
                    opt.MapFrom(src =>
                        src.Messages.Count > 0
                            ? new LastMessageDto(
                                src.Messages.First().Id,
                                src.Messages.First().Content,
                                src.Messages.First().CreatedAt,
                                src.Messages.First().SenderId
                            )
                            : null
                    )
            );

        CreateMap<Conversation, SingleConversationDto>()
            .ForMember(
                dest => dest.Members,
                opt =>
                    opt.MapFrom(src =>
                        src.Users.Select(u => new UserDto(
                            u.Id,
                            u.DisplayName,
                            new UserWithGroupDto(
                                u.UserConversations.First().Nickname,
                                (bool)u.UserConversations.First().IsAdmin!
                            )
                        ))
                    )
            )
            .ForMember(
                dest => dest.Messages,
                opt =>
                    opt.MapFrom(src =>
                        src.Messages.Select(m => new ConversationMessageDto(
                            m.Id,
                            m.MessageStatuses.Select(mr => new MessageReceiverDTO(
                                    mr.UserId,
                                    mr.Status
                                ))
                                .ToList(),
                            m.Content,
                            m.SenderId,
                            m.CreatedAt
                        ))
                    )
            );
    }
}
