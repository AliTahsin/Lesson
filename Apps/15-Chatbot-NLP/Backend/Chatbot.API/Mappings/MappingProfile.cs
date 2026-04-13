using AutoMapper;
using Chatbot.API.Models;
using Chatbot.API.DTOs;

namespace Chatbot.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Conversation, ConversationDto>();
            CreateMap<ChatMessage, ChatMessageDto>();
            CreateMap<Intent, IntentDto>();
            CreateMap<CreateIntentDto, Intent>();
            CreateMap<UpdateIntentDto, Intent>();
        }
    }
}