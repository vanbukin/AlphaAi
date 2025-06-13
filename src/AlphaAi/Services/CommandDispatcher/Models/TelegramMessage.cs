using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AlphaAi.Services.CommandDispatcher.Models;

public record TelegramMessage(Message Message, UpdateType Type);
