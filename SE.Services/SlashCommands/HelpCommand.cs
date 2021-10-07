using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace SE.Services.SlashCommands
{
    public record HelpCommand(SocketSlashCommand Command) : IRequest;

    public class HelpCommandHandler : IRequestHandler<HelpCommand>
    {
        private readonly IMediator _mediator;

        public HelpCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(HelpCommand request, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }
    }
}
