﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Channel : BaseEntity
{
    [Column]
    public string Name { get; set; } = null!;

    public string ServerId { get; set; }
    public Server Server { get; set; } = null!;
    public ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();
}
