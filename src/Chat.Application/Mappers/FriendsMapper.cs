using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Application.Mappers;

[Mapper]
public partial class FriendsMapper
{
    public partial FriendsResponseDto FriendToFriendsResponseDto(Friends friends);

    public partial Friends FriendsRequestDtoToFriends(FriendsRequestDto friendsRequestDto);
}
