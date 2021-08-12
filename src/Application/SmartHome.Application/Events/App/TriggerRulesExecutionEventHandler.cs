using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Logging;
using SmartHome.Application.RuleEngine;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.Events.App
{
    public class TriggerRulesExecutionEventHandler: INotificationHandler<TriggerRulesExecutionEvent>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IRuleEngine _ruleEngine;
        private readonly ILogger<LoggingContext> _logger;

        public TriggerRulesExecutionEventHandler(IApplicationDbContext dbContext, IRuleEngine ruleEngine, ILogger<LoggingContext> logger)
        {
            _dbContext = dbContext;
            _ruleEngine = ruleEngine;
            _logger = logger;
        }

        public async Task Handle(TriggerRulesExecutionEvent notification, CancellationToken cancellationToken)
        {
            var rules = await _dbContext.Rules.AsNoTracking().Where(x => x.IsActive).ToListAsync(cancellationToken);
            foreach (var rule in rules)
            {
                var ruleNode = JsonSerializer.Deserialize<RuleNode>(rule.Body);
                var ruleOutputAction = JsonSerializer.Deserialize<RuleOutputAction>(rule.OutputAction);
                
                if (ruleNode == null || ruleOutputAction == null)
                {
                    _logger.LogWarning($"[TriggerRulesExecutionEventHandler] Invalid rule body or output action schema. Rule with Id: {rule.Id} was ignored");
                    continue;
                }
               
                await _ruleEngine.Execute(rule.Id, ruleNode, ruleOutputAction, cancellationToken);
            }
        }
    }
}
