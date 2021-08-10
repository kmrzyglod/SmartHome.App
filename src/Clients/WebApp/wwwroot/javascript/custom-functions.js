////var rules_basic = {
////    condition: 'AND',
////    rules: [{
////        id: 'price',
////        operator: 'less',
////        value: 10.25
////    }, {
////        condition: 'OR',
////        rules: [{
////            id: 'category',
////            operator: 'equal',
////            value: 2
////        }, {
////            id: 'category',
////            operator: 'equal',
////            value: 1
////        }]
////    }]
////};

function renderJSON(jsonStr, divId) {
    setTimeout(() => {
            const formatter = new JSONFormatter(jsonStr);
            var container = document.getElementById(divId);
            if (container.children.length > 0) {
                container.replaceChild(formatter.render(), container.children);
                return;
            }
            container.appendChild(formatter.render());
        },
        1);
    return;
}

function initQueryBuilder(divId, serializedRules) {
    $('#' + divId).queryBuilder({
  
        filters: [{
            id: 'temperature',
            label: 'Temperature in greenhouse [°C]',
            type: 'double',
            operators: ['equal', 'not_equal', 'less', 'less_or_equal', 'greater', 'greater_or_equal', 'between', 'not_between'],
            validation: {
                min: -40,
                max: 50,
                step: 0.1
            }
        },{
            id: 'max_wind_speed',
            label: 'Max wind speed  [m/s]',
            type: 'double',
            operators: ['equal', 'not_equal', 'less', 'less_or_equal', 'greater', 'greater_or_equal', 'between', 'not_between'],
            validation: {
                min: 0,
                max: 40,
                step: 1
            }
        },  {
            id: 'is_raining',
            label: 'Is raining',
            type: 'boolean',
            input: 'radio',
            values: {
                true: 'True',
                false: 'False'
            },
            operators: ['equal']
        },  {
            id: 'cron_expression',
            label: 'Time schedule',
            type: 'string',
            placeholder: 'cron expression',
            operators: ['equal'],
            validation: {
                format: /^(\*|([0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])|\*\/([0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])) (\*|([0-9]|1[0-9]|2[0-3])|\*\/([0-9]|1[0-9]|2[0-3])) (\*|([1-9]|1[0-9]|2[0-9]|3[0-1])|\*\/([1-9]|1[0-9]|2[0-9]|3[0-1])) (\*|([1-9]|1[0-2])|\*\/([1-9]|1[0-2])) (\*|([0-6])|\*\/([0-6]))$/
            }
        }]
    });

    if (serializedRules) {
        var rules = JSON.parse(serializedRules);
        $('#' + divId).queryBuilder('setRules', rules);
    }

    return;
}


function getRules(divId) {
    var result =  $('#' + divId).queryBuilder('getRules');
  
    if (!$.isEmptyObject(result)) {
        return(JSON.stringify(result, null, 2));
    }

    return "";
}

function validateRules(divId) {
    var result = $('#' + divId).queryBuilder('getRules');
  
    if (!$.isEmptyObject(result)) {
        return true;
    }

    return false;
}