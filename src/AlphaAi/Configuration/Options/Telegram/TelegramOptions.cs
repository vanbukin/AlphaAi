using System.ComponentModel.DataAnnotations;

namespace AlphaAi.Configuration.Options.Telegram;

public class TelegramOptions
{
    [Required]
    [MaxLength(10000)]
    public string BotToken { get; set; } = default!;

    [Required]
    [MinLength(1)]
    public long[] AllowedChatIds { get; set; } = default!;
}
