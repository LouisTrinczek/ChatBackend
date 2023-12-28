using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Domain.Mappers;

[Mapper]
public partial class FriendsMapper
{
    public partial FriendsResponseDto FriendToFriendsResponseDto(Friends friends);

    public partial Friends FriendsRequestDtoToFriends(FriendsRequestDto friendsRequestDto);
}
