using MediatR;

namespace SmartHome.Application.Shared.Queries.Rules.GetRuleDetails
{
    public class GetRuleDetailsQuery : IRequest<RuleDetailsVm?>
    {
        public long Id { get; set; }
    }
}